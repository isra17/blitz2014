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
        public void indexArtist(Artist artistToIndex)
        {
            Console.WriteLine("Artist " + artistToIndex.Id);
        }

        public void indexAlbum(Album albumToIndex)
        {
            Console.WriteLine("Album " + albumToIndex.Id);
        }

        public QueryResponse query(Query query)
        {
            Console.WriteLine("Query " + query.RootId);
            return new QueryResponse();
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
