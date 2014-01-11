using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.coveo.blitz.thrift;
using Thrift.Server;
using Thrift.Transport;

namespace BlitzIndex
{
    class IndexerHandler : Indexer.Iface
    {
        private readonly Database m_db = new Database();

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
            m_db.Insert(document);
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
            QueryResponse response = new QueryResponse();
            response.Results = new List<QueryResult>();
            response.Facets = new List<FacetResult>();
            Dictionary<int, QueryTreeNode> treeNodes = query.QueryTreeNodes.ToDictionary(n => n.Id);

            var facetResults = new Dictionary<string, FacetResult>();

            foreach (IDocument document in EvaluateQuery(treeNodes, treeNodes[query.RootId]))
            {
                //Console.WriteLine(document.Id + ":");
                //Console.WriteLine(document.Text);
                //Console.WriteLine();

                QueryResult result = new QueryResult();
                result.DocumentType = document.Type;
                result.Id = document.Id;
                response.Results.Add(result);
                
                foreach (var facetName in document.FacetNames)
                {
                    FacetResult facetResult;
                    if (!facetResults.TryGetValue(facetName, out facetResult))
                    {
                        facetResult = new FacetResult();
                        facetResult.Values = new List<FacetValue>();
                        facetResult.MetadataName = facetName;
                        facetResults.Add(facetName, facetResult);
                        response.Facets.Add(facetResult);
                    }

                    var values = document.GetFacetValues(facetName);
                    if (values != null)
                    {
                        foreach (string value in values)
                        {
                            var facetValue = facetResult.Values.FirstOrDefault(v => v.Value == value);
                            if (facetValue == null)
                            {
                                facetValue = new FacetValue();
                                facetValue.Value = value;
                                facetValue.Count = 0;
                                facetResult.Values.Add(facetValue);
                            }

                            facetValue.Count++;
                        }
                    }
                }
            }

            response.Results.Sort((a, b) => string.Compare(a.Id, b.Id));
            return response;
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

            TServerTransport serverTransport = new TThreadPoolServer(9090);
            TServer server = new TSimpleServer(processor, serverTransport);
            Console.WriteLine("Server started!");
            server.Serve();
        }
    }
}