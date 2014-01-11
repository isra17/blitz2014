﻿using System;
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

        public QueryResponse query(Query query)
        {
            QueryResponse response = new QueryResponse();
			response.Results = new List<QueryResult>();
			response.Facets = new List<FacetResult>();
            Dictionary<int, QueryTreeNode> treeNodes = query.QueryTreeNodes.ToDictionary(n => n.Id);
            QueryTreeNode treeNode = treeNodes[query.RootId];

			var facetResults = new Dictionary<string, FacetResult>();

            foreach (IDocument document in m_db.Query(treeNode.Value))
            {
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

					foreach (string value in document.GetFacetValues(facetName))
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

            TServerTransport serverTransport = new TServerSocket(9090);
            TServer server = new TSimpleServer(processor, serverTransport);
            Console.WriteLine("Server started!");
            server.Serve();
        }
    }
}