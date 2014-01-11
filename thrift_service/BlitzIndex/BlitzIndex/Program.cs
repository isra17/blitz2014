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
		private Database m_db = new Database();

        public void indexArtist(Artist artistToIndex)
        {
            Console.WriteLine("Artist " + artistToIndex.Id);
			indexDocument(artistToIndex);
        }

        public void indexAlbum(Album albumToIndex)
        {
            Console.WriteLine("Album " + albumToIndex.Id);
			indexDocument(albumToIndex);
        }

		public void indexDocument(IDocument document)
		{
			Console.WriteLine("{0} {1}", document.Type, document.Id);
			m_db.Insert(document);
		}

        public QueryResponse query(Query query)
        {
            QueryResponse response = new QueryResponse();
            Dictionary<int, QueryTreeNode> treeNodes = query.QueryTreeNodes.ToDictionary(n => n.Id);
            QueryTreeNode treeNode = treeNodes[query.RootId];
            foreach (IDocument document in m_db.Query(treeNode.Value))
            {
                QueryResult result = new QueryResult();
                result.DocumentType = document.Type;
                result.Id = document.Id;
                response.Results.Add(result);
            }
            return response;
        }

        public void reset()
        {
            Console.WriteLine("Reset");
        }

        public void ping()
        {
            Console.WriteLine("Ping");
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