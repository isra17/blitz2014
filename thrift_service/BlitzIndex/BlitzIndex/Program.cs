using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.coveo.blitz.thrift;
using Thrift.Server;
using Thrift.Transport;

using System.Threading;
using System.Threading.Tasks;

namespace BlitzIndex
{
    class IndexerHandler : Indexer.Iface
    {
        private readonly Database m_db = new Database();
		private readonly List<Task> m_tasks = new List<Task>();

        public void indexArtist(Artist artistToIndex)
        {
            indexDocument(artistToIndex);
        }

        public void indexAlbum(Album albumToIndex)
        {
            indexDocument(albumToIndex);
        }

        public void indexDocument(IDocument document)
        {
			Task t = Task.Factory.StartNew(() => m_db.Insert(document));
			m_tasks.Add(t);
        }

        private HashSet<IDocument> EvaluateQuery(Dictionary<int, QueryTreeNode> inputNodes, QueryTreeNode specificNode)
        {
            if (specificNode.Type == NodeType.LITERAL)
            {
                return m_db.Query(specificNode.Value);
            }

            var operatorName = specificNode.Value.ToUpperInvariant().Trim();
            var left = EvaluateQuery(inputNodes, inputNodes[specificNode.LeftPart]);
            var right = EvaluateQuery(inputNodes, inputNodes[specificNode.RightPart]);
            if (operatorName == "AND")
            {
                left.IntersectWith(right);
            }
            else if (operatorName == "OR")
            {
                left.UnionWith(right);
            }
            else
            {
                throw new NotImplementedException();
            }
            return left;
        }

        public QueryResponse query(Query query)
        {
            //Console.WriteLine("{0} documents in db", m_db.Count);
            //PrettyPrint.PrintQuery(query);
			foreach(Task t in m_tasks)
				t.Wait();
			m_tasks.Clear();

            QueryResponse response = new QueryResponse();
            response.Results = new List<QueryResult>();
            response.Facets = new List<FacetResult>();
            Dictionary<int, QueryTreeNode> treeNodes = query.QueryTreeNodes.ToDictionary(n => n.Id);

            var responseBuilder = new QueryResponseBuilder();

            foreach (IDocument document in EvaluateQuery(treeNodes, treeNodes[query.RootId]))
            {
                //Console.WriteLine(document.Id + ":");
                //Console.WriteLine(document.Text);
                //Console.WriteLine();

                responseBuilder.AddNewDocument(document);
            }

            return responseBuilder.Build();
        }

        public void reset()
        {
            Console.WriteLine("Reset");
            m_db.Reset();
        }

        public void ping()
        {
            Console.WriteLine("Ping");
        }
		
		public Album GetAlbum(string id)
		{
			return (Album)m_db.GetDocument(id);
		}
		
		public Artist GetArtist(string id)
		{
			return (Artist)m_db.GetDocument(id);
		}
    }

    class Program
    {
        static void Main(string[] args)
        {
            IndexerHandler handler = new IndexerHandler();
            Indexer.Processor processor = new Indexer.Processor(handler);

            TServerTransport serverTransport = new TServerSocket(9090);
            TServer server = new TSimpleServer(processor, serverTransport);
            Console.WriteLine("Server started!");
            server.Serve();
        }
    }
}