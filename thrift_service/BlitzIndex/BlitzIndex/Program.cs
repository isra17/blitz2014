using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.coveo.blitz.thrift;
using Thrift.Server;
using Thrift.Transport;
using System.Net;
using System.Threading;
using System.Globalization;
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

        public QueryResponse query(Query query)
        {
            //Console.WriteLine("{0} documents in db", m_db.Count);
            //PrettyPrint.PrintQuery(query);
			foreach(Task t in m_tasks)
				t.Wait();
			m_tasks.Clear();

            Dictionary<int, QueryTreeNode> treeNodes = query.QueryTreeNodes.ToDictionary(n => n.Id);

            var responseBuilder = new QueryResponseBuilder();

            var results = QueryEvaluator.EvaluateQuery(m_db, query);
            foreach (IDocument document in results)
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
			Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
			Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
			ServicePointManager.DefaultConnectionLimit = int.MaxValue;

            IndexerHandler handler = new IndexerHandler();
            Indexer.Processor processor = new Indexer.Processor(handler);

            TServerTransport serverTransport = new TServerSocket(9090);
            TServer server = new TSimpleServer(processor, serverTransport);
            Console.WriteLine("Server started!");
            server.Serve();
        }
    }
}