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
		Database m_db;

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
            QueryTreeNode treeNode = query.QueryTreeNodes[query.RootId];
            if (treeNode.Value == "*")
            {
                // dump everything
            }
            else
            {
                // identical match for a SINGLE keyword
                string identicalMatch = treeNode.Value;
            }
            return new QueryResponse();
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