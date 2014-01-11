/**
 * Autogenerated by Thrift Compiler (0.9.0)
 *
 * DO NOT EDIT UNLESS YOU ARE SURE THAT YOU KNOW WHAT YOU ARE DOING
 *  @generated
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Thrift;
using Thrift.Collections;
using System.Runtime.Serialization;
using Thrift.Protocol;
using Thrift.Transport;

namespace com.coveo.blitz.thrift
{
  public partial class Indexer {
    /// <summary>
    /// L'Indexer est le service responsable d'indexer les artistes et les albums pousses par Coveo.
    /// Vous devez recevoir les artistes et les albums envoyes et les mettre dans votre index afin qu'ils
    /// puissent etre recherches.
    /// 
    /// Lors que l'execution des appels de type "index" se termine, vous devez avoir complete l'indexation de l'artiste
    /// ou de l'album et cet artiste ou ce document doit etre maintenant cherchable.
    /// </summary>
    public interface Iface {
      /// <summary>
      /// Demande a votre index d'indexer l'artiste passe en parametre.
      /// La methode indexArtist doit retourner uniquement lorsque le processus d'indexation est complete.
      /// </summary>
      /// <param name="artistToIndex"></param>
      void indexArtist(com.coveo.blitz.thrift.Artist artistToIndex);
      #if SILVERLIGHT
      IAsyncResult Begin_indexArtist(AsyncCallback callback, object state, com.coveo.blitz.thrift.Artist artistToIndex);
      void End_indexArtist(IAsyncResult asyncResult);
      #endif
      /// <summary>
      /// Demande a votre index d'indexer l'album passe en parametre.
      /// La methode indexAlbum doit retourner uniquement lorsque le processus d'indexation est complete.
      /// </summary>
      /// <param name="albumToIndex"></param>
      void indexAlbum(com.coveo.blitz.thrift.Album albumToIndex);
      #if SILVERLIGHT
      IAsyncResult Begin_indexAlbum(AsyncCallback callback, object state, com.coveo.blitz.thrift.Album albumToIndex);
      void End_indexAlbum(IAsyncResult asyncResult);
      #endif
      /// <summary>
      /// Demande a votre index de retourner les resultats de la requete passee en parametre.
      /// L'ordre dans lequel les resultats sont retournes est important.
      /// 
      /// Voir la documentation du concours pour savoir dans quel ordre les artistes et les albums doivent etre retournes.
      /// </summary>
      /// <param name="query"></param>
      com.coveo.blitz.thrift.QueryResponse query(com.coveo.blitz.thrift.Query query);
      #if SILVERLIGHT
      IAsyncResult Begin_query(AsyncCallback callback, object state, com.coveo.blitz.thrift.Query query);
      com.coveo.blitz.thrift.QueryResponse End_query(IAsyncResult asyncResult);
      #endif
      /// <summary>
      /// Demande a votre index de se reinitialiser. Tout le contenu de l'index doit etre efface.
      /// Cela permet de s'assurer que votre index ne garde pas des documents en trop entre deux evaluations, ce qui
      /// nuirait a votre score.
      /// </summary>
      void reset();
      #if SILVERLIGHT
      IAsyncResult Begin_reset(AsyncCallback callback, object state, );
      void End_reset(IAsyncResult asyncResult);
      #endif
      /// <summary>
      /// Verifie si votre index est operationel et pret a repondre.
      /// </summary>
      void ping();
      #if SILVERLIGHT
      IAsyncResult Begin_ping(AsyncCallback callback, object state, );
      void End_ping(IAsyncResult asyncResult);
      #endif
    }

    /// <summary>
    /// L'Indexer est le service responsable d'indexer les artistes et les albums pousses par Coveo.
    /// Vous devez recevoir les artistes et les albums envoyes et les mettre dans votre index afin qu'ils
    /// puissent etre recherches.
    /// 
    /// Lors que l'execution des appels de type "index" se termine, vous devez avoir complete l'indexation de l'artiste
    /// ou de l'album et cet artiste ou ce document doit etre maintenant cherchable.
    /// </summary>
    public class Client : Iface {
      public Client(TProtocol prot) : this(prot, prot)
      {
      }

      public Client(TProtocol iprot, TProtocol oprot)
      {
        iprot_ = iprot;
        oprot_ = oprot;
      }

      protected TProtocol iprot_;
      protected TProtocol oprot_;
      protected int seqid_;

      public TProtocol InputProtocol
      {
        get { return iprot_; }
      }
      public TProtocol OutputProtocol
      {
        get { return oprot_; }
      }


      
      #if SILVERLIGHT
      public IAsyncResult Begin_indexArtist(AsyncCallback callback, object state, com.coveo.blitz.thrift.Artist artistToIndex)
      {
        return send_indexArtist(callback, state, artistToIndex);
      }

      public void End_indexArtist(IAsyncResult asyncResult)
      {
        oprot_.Transport.EndFlush(asyncResult);
        recv_indexArtist();
      }

      #endif

      /// <summary>
      /// Demande a votre index d'indexer l'artiste passe en parametre.
      /// La methode indexArtist doit retourner uniquement lorsque le processus d'indexation est complete.
      /// </summary>
      /// <param name="artistToIndex"></param>
      public void indexArtist(com.coveo.blitz.thrift.Artist artistToIndex)
      {
        #if !SILVERLIGHT
        send_indexArtist(artistToIndex);
        recv_indexArtist();

        #else
        var asyncResult = Begin_indexArtist(null, null, artistToIndex);
        End_indexArtist(asyncResult);

        #endif
      }
      #if SILVERLIGHT
      public IAsyncResult send_indexArtist(AsyncCallback callback, object state, com.coveo.blitz.thrift.Artist artistToIndex)
      #else
      public void send_indexArtist(com.coveo.blitz.thrift.Artist artistToIndex)
      #endif
      {
        oprot_.WriteMessageBegin(new TMessage("indexArtist", TMessageType.Call, seqid_));
        indexArtist_args args = new indexArtist_args();
        args.ArtistToIndex = artistToIndex;
        args.Write(oprot_);
        oprot_.WriteMessageEnd();
        #if SILVERLIGHT
        return oprot_.Transport.BeginFlush(callback, state);
        #else
        oprot_.Transport.Flush();
        #endif
      }

      public void recv_indexArtist()
      {
        TMessage msg = iprot_.ReadMessageBegin();
        if (msg.Type == TMessageType.Exception) {
          TApplicationException x = TApplicationException.Read(iprot_);
          iprot_.ReadMessageEnd();
          throw x;
        }
        indexArtist_result result = new indexArtist_result();
        result.Read(iprot_);
        iprot_.ReadMessageEnd();
        return;
      }

      
      #if SILVERLIGHT
      public IAsyncResult Begin_indexAlbum(AsyncCallback callback, object state, com.coveo.blitz.thrift.Album albumToIndex)
      {
        return send_indexAlbum(callback, state, albumToIndex);
      }

      public void End_indexAlbum(IAsyncResult asyncResult)
      {
        oprot_.Transport.EndFlush(asyncResult);
        recv_indexAlbum();
      }

      #endif

      /// <summary>
      /// Demande a votre index d'indexer l'album passe en parametre.
      /// La methode indexAlbum doit retourner uniquement lorsque le processus d'indexation est complete.
      /// </summary>
      /// <param name="albumToIndex"></param>
      public void indexAlbum(com.coveo.blitz.thrift.Album albumToIndex)
      {
        #if !SILVERLIGHT
        send_indexAlbum(albumToIndex);
        recv_indexAlbum();

        #else
        var asyncResult = Begin_indexAlbum(null, null, albumToIndex);
        End_indexAlbum(asyncResult);

        #endif
      }
      #if SILVERLIGHT
      public IAsyncResult send_indexAlbum(AsyncCallback callback, object state, com.coveo.blitz.thrift.Album albumToIndex)
      #else
      public void send_indexAlbum(com.coveo.blitz.thrift.Album albumToIndex)
      #endif
      {
        oprot_.WriteMessageBegin(new TMessage("indexAlbum", TMessageType.Call, seqid_));
        indexAlbum_args args = new indexAlbum_args();
        args.AlbumToIndex = albumToIndex;
        args.Write(oprot_);
        oprot_.WriteMessageEnd();
        #if SILVERLIGHT
        return oprot_.Transport.BeginFlush(callback, state);
        #else
        oprot_.Transport.Flush();
        #endif
      }

      public void recv_indexAlbum()
      {
        TMessage msg = iprot_.ReadMessageBegin();
        if (msg.Type == TMessageType.Exception) {
          TApplicationException x = TApplicationException.Read(iprot_);
          iprot_.ReadMessageEnd();
          throw x;
        }
        indexAlbum_result result = new indexAlbum_result();
        result.Read(iprot_);
        iprot_.ReadMessageEnd();
        return;
      }

      
      #if SILVERLIGHT
      public IAsyncResult Begin_query(AsyncCallback callback, object state, com.coveo.blitz.thrift.Query query)
      {
        return send_query(callback, state, query);
      }

      public com.coveo.blitz.thrift.QueryResponse End_query(IAsyncResult asyncResult)
      {
        oprot_.Transport.EndFlush(asyncResult);
        return recv_query();
      }

      #endif

      /// <summary>
      /// Demande a votre index de retourner les resultats de la requete passee en parametre.
      /// L'ordre dans lequel les resultats sont retournes est important.
      /// 
      /// Voir la documentation du concours pour savoir dans quel ordre les artistes et les albums doivent etre retournes.
      /// </summary>
      /// <param name="query"></param>
      public com.coveo.blitz.thrift.QueryResponse query(com.coveo.blitz.thrift.Query query)
      {
        #if !SILVERLIGHT
        send_query(query);
        return recv_query();

        #else
        var asyncResult = Begin_query(null, null, query);
        return End_query(asyncResult);

        #endif
      }
      #if SILVERLIGHT
      public IAsyncResult send_query(AsyncCallback callback, object state, com.coveo.blitz.thrift.Query query)
      #else
      public void send_query(com.coveo.blitz.thrift.Query query)
      #endif
      {
        oprot_.WriteMessageBegin(new TMessage("query", TMessageType.Call, seqid_));
        query_args args = new query_args();
        args.Query = query;
        args.Write(oprot_);
        oprot_.WriteMessageEnd();
        #if SILVERLIGHT
        return oprot_.Transport.BeginFlush(callback, state);
        #else
        oprot_.Transport.Flush();
        #endif
      }

      public com.coveo.blitz.thrift.QueryResponse recv_query()
      {
        TMessage msg = iprot_.ReadMessageBegin();
        if (msg.Type == TMessageType.Exception) {
          TApplicationException x = TApplicationException.Read(iprot_);
          iprot_.ReadMessageEnd();
          throw x;
        }
        query_result result = new query_result();
        result.Read(iprot_);
        iprot_.ReadMessageEnd();
        if (result.__isset.success) {
          return result.Success;
        }
        throw new TApplicationException(TApplicationException.ExceptionType.MissingResult, "query failed: unknown result");
      }

      
      #if SILVERLIGHT
      public IAsyncResult Begin_reset(AsyncCallback callback, object state, )
      {
        return send_reset(callback, state);
      }

      public void End_reset(IAsyncResult asyncResult)
      {
        oprot_.Transport.EndFlush(asyncResult);
        recv_reset();
      }

      #endif

      /// <summary>
      /// Demande a votre index de se reinitialiser. Tout le contenu de l'index doit etre efface.
      /// Cela permet de s'assurer que votre index ne garde pas des documents en trop entre deux evaluations, ce qui
      /// nuirait a votre score.
      /// </summary>
      public void reset()
      {
        #if !SILVERLIGHT
        send_reset();
        recv_reset();

        #else
        var asyncResult = Begin_reset(null, null, );
        End_reset(asyncResult);

        #endif
      }
      #if SILVERLIGHT
      public IAsyncResult send_reset(AsyncCallback callback, object state, )
      #else
      public void send_reset()
      #endif
      {
        oprot_.WriteMessageBegin(new TMessage("reset", TMessageType.Call, seqid_));
        reset_args args = new reset_args();
        args.Write(oprot_);
        oprot_.WriteMessageEnd();
        #if SILVERLIGHT
        return oprot_.Transport.BeginFlush(callback, state);
        #else
        oprot_.Transport.Flush();
        #endif
      }

      public void recv_reset()
      {
        TMessage msg = iprot_.ReadMessageBegin();
        if (msg.Type == TMessageType.Exception) {
          TApplicationException x = TApplicationException.Read(iprot_);
          iprot_.ReadMessageEnd();
          throw x;
        }
        reset_result result = new reset_result();
        result.Read(iprot_);
        iprot_.ReadMessageEnd();
        return;
      }

      
      #if SILVERLIGHT
      public IAsyncResult Begin_ping(AsyncCallback callback, object state, )
      {
        return send_ping(callback, state);
      }

      public void End_ping(IAsyncResult asyncResult)
      {
        oprot_.Transport.EndFlush(asyncResult);
        recv_ping();
      }

      #endif

      /// <summary>
      /// Verifie si votre index est operationel et pret a repondre.
      /// </summary>
      public void ping()
      {
        #if !SILVERLIGHT
        send_ping();
        recv_ping();

        #else
        var asyncResult = Begin_ping(null, null, );
        End_ping(asyncResult);

        #endif
      }
      #if SILVERLIGHT
      public IAsyncResult send_ping(AsyncCallback callback, object state, )
      #else
      public void send_ping()
      #endif
      {
        oprot_.WriteMessageBegin(new TMessage("ping", TMessageType.Call, seqid_));
        ping_args args = new ping_args();
        args.Write(oprot_);
        oprot_.WriteMessageEnd();
        #if SILVERLIGHT
        return oprot_.Transport.BeginFlush(callback, state);
        #else
        oprot_.Transport.Flush();
        #endif
      }

      public void recv_ping()
      {
        TMessage msg = iprot_.ReadMessageBegin();
        if (msg.Type == TMessageType.Exception) {
          TApplicationException x = TApplicationException.Read(iprot_);
          iprot_.ReadMessageEnd();
          throw x;
        }
        ping_result result = new ping_result();
        result.Read(iprot_);
        iprot_.ReadMessageEnd();
        return;
      }

    }
    public class Processor : TProcessor {
      public Processor(Iface iface)
      {
        iface_ = iface;
        processMap_["indexArtist"] = indexArtist_Process;
        processMap_["indexAlbum"] = indexAlbum_Process;
        processMap_["query"] = query_Process;
        processMap_["reset"] = reset_Process;
        processMap_["ping"] = ping_Process;
      }

      protected delegate void ProcessFunction(int seqid, TProtocol iprot, TProtocol oprot);
      private Iface iface_;
      protected Dictionary<string, ProcessFunction> processMap_ = new Dictionary<string, ProcessFunction>();

      public bool Process(TProtocol iprot, TProtocol oprot)
      {
        try
        {
          TMessage msg = iprot.ReadMessageBegin();
          ProcessFunction fn;
          processMap_.TryGetValue(msg.Name, out fn);
          if (fn == null) {
            TProtocolUtil.Skip(iprot, TType.Struct);
            iprot.ReadMessageEnd();
            TApplicationException x = new TApplicationException (TApplicationException.ExceptionType.UnknownMethod, "Invalid method name: '" + msg.Name + "'");
            oprot.WriteMessageBegin(new TMessage(msg.Name, TMessageType.Exception, msg.SeqID));
            x.Write(oprot);
            oprot.WriteMessageEnd();
            oprot.Transport.Flush();
            return true;
          }
          fn(msg.SeqID, iprot, oprot);
        }
        catch (IOException)
        {
          return false;
        }
        return true;
      }

      public void indexArtist_Process(int seqid, TProtocol iprot, TProtocol oprot)
      {
        indexArtist_args args = new indexArtist_args();
        args.Read(iprot);
        iprot.ReadMessageEnd();
        indexArtist_result result = new indexArtist_result();
        iface_.indexArtist(args.ArtistToIndex);
        oprot.WriteMessageBegin(new TMessage("indexArtist", TMessageType.Reply, seqid)); 
        result.Write(oprot);
        oprot.WriteMessageEnd();
        oprot.Transport.Flush();
      }

      public void indexAlbum_Process(int seqid, TProtocol iprot, TProtocol oprot)
      {
        indexAlbum_args args = new indexAlbum_args();
        args.Read(iprot);
        iprot.ReadMessageEnd();
        indexAlbum_result result = new indexAlbum_result();
        iface_.indexAlbum(args.AlbumToIndex);
        oprot.WriteMessageBegin(new TMessage("indexAlbum", TMessageType.Reply, seqid)); 
        result.Write(oprot);
        oprot.WriteMessageEnd();
        oprot.Transport.Flush();
      }

      public void query_Process(int seqid, TProtocol iprot, TProtocol oprot)
      {
        query_args args = new query_args();
        args.Read(iprot);
        iprot.ReadMessageEnd();
        query_result result = new query_result();
        result.Success = iface_.query(args.Query);
        oprot.WriteMessageBegin(new TMessage("query", TMessageType.Reply, seqid)); 
        result.Write(oprot);
        oprot.WriteMessageEnd();
        oprot.Transport.Flush();
      }

      public void reset_Process(int seqid, TProtocol iprot, TProtocol oprot)
      {
        reset_args args = new reset_args();
        args.Read(iprot);
        iprot.ReadMessageEnd();
        reset_result result = new reset_result();
        iface_.reset();
        oprot.WriteMessageBegin(new TMessage("reset", TMessageType.Reply, seqid)); 
        result.Write(oprot);
        oprot.WriteMessageEnd();
        oprot.Transport.Flush();
      }

      public void ping_Process(int seqid, TProtocol iprot, TProtocol oprot)
      {
        ping_args args = new ping_args();
        args.Read(iprot);
        iprot.ReadMessageEnd();
        ping_result result = new ping_result();
        iface_.ping();
        oprot.WriteMessageBegin(new TMessage("ping", TMessageType.Reply, seqid)); 
        result.Write(oprot);
        oprot.WriteMessageEnd();
        oprot.Transport.Flush();
      }

    }


    #if !SILVERLIGHT
    [Serializable]
    #endif
    public partial class indexArtist_args : TBase
    {
      private com.coveo.blitz.thrift.Artist _artistToIndex;

      public com.coveo.blitz.thrift.Artist ArtistToIndex
      {
        get
        {
          return _artistToIndex;
        }
        set
        {
          __isset.artistToIndex = true;
          this._artistToIndex = value;
        }
      }


      public Isset __isset;
      #if !SILVERLIGHT
      [Serializable]
      #endif
      public struct Isset {
        public bool artistToIndex;
      }

      public indexArtist_args() {
      }

      public void Read (TProtocol iprot)
      {
        TField field;
        iprot.ReadStructBegin();
        while (true)
        {
          field = iprot.ReadFieldBegin();
          if (field.Type == TType.Stop) { 
            break;
          }
          switch (field.ID)
          {
            case 1:
              if (field.Type == TType.Struct) {
                ArtistToIndex = new com.coveo.blitz.thrift.Artist();
                ArtistToIndex.Read(iprot);
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            default: 
              TProtocolUtil.Skip(iprot, field.Type);
              break;
          }
          iprot.ReadFieldEnd();
        }
        iprot.ReadStructEnd();
      }

      public void Write(TProtocol oprot) {
        TStruct struc = new TStruct("indexArtist_args");
        oprot.WriteStructBegin(struc);
        TField field = new TField();
        if (ArtistToIndex != null && __isset.artistToIndex) {
          field.Name = "artistToIndex";
          field.Type = TType.Struct;
          field.ID = 1;
          oprot.WriteFieldBegin(field);
          ArtistToIndex.Write(oprot);
          oprot.WriteFieldEnd();
        }
        oprot.WriteFieldStop();
        oprot.WriteStructEnd();
      }

      public override string ToString() {
        StringBuilder sb = new StringBuilder("indexArtist_args(");
        sb.Append("ArtistToIndex: ");
        sb.Append(ArtistToIndex== null ? "<null>" : ArtistToIndex.ToString());
        sb.Append(")");
        return sb.ToString();
      }

    }


    #if !SILVERLIGHT
    [Serializable]
    #endif
    public partial class indexArtist_result : TBase
    {

      public indexArtist_result() {
      }

      public void Read (TProtocol iprot)
      {
        TField field;
        iprot.ReadStructBegin();
        while (true)
        {
          field = iprot.ReadFieldBegin();
          if (field.Type == TType.Stop) { 
            break;
          }
          switch (field.ID)
          {
            default: 
              TProtocolUtil.Skip(iprot, field.Type);
              break;
          }
          iprot.ReadFieldEnd();
        }
        iprot.ReadStructEnd();
      }

      public void Write(TProtocol oprot) {
        TStruct struc = new TStruct("indexArtist_result");
        oprot.WriteStructBegin(struc);

        oprot.WriteFieldStop();
        oprot.WriteStructEnd();
      }

      public override string ToString() {
        StringBuilder sb = new StringBuilder("indexArtist_result(");
        sb.Append(")");
        return sb.ToString();
      }

    }


    #if !SILVERLIGHT
    [Serializable]
    #endif
    public partial class indexAlbum_args : TBase
    {
      private com.coveo.blitz.thrift.Album _albumToIndex;

      public com.coveo.blitz.thrift.Album AlbumToIndex
      {
        get
        {
          return _albumToIndex;
        }
        set
        {
          __isset.albumToIndex = true;
          this._albumToIndex = value;
        }
      }


      public Isset __isset;
      #if !SILVERLIGHT
      [Serializable]
      #endif
      public struct Isset {
        public bool albumToIndex;
      }

      public indexAlbum_args() {
      }

      public void Read (TProtocol iprot)
      {
        TField field;
        iprot.ReadStructBegin();
        while (true)
        {
          field = iprot.ReadFieldBegin();
          if (field.Type == TType.Stop) { 
            break;
          }
          switch (field.ID)
          {
            case 1:
              if (field.Type == TType.Struct) {
                AlbumToIndex = new com.coveo.blitz.thrift.Album();
                AlbumToIndex.Read(iprot);
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            default: 
              TProtocolUtil.Skip(iprot, field.Type);
              break;
          }
          iprot.ReadFieldEnd();
        }
        iprot.ReadStructEnd();
      }

      public void Write(TProtocol oprot) {
        TStruct struc = new TStruct("indexAlbum_args");
        oprot.WriteStructBegin(struc);
        TField field = new TField();
        if (AlbumToIndex != null && __isset.albumToIndex) {
          field.Name = "albumToIndex";
          field.Type = TType.Struct;
          field.ID = 1;
          oprot.WriteFieldBegin(field);
          AlbumToIndex.Write(oprot);
          oprot.WriteFieldEnd();
        }
        oprot.WriteFieldStop();
        oprot.WriteStructEnd();
      }

      public override string ToString() {
        StringBuilder sb = new StringBuilder("indexAlbum_args(");
        sb.Append("AlbumToIndex: ");
        sb.Append(AlbumToIndex== null ? "<null>" : AlbumToIndex.ToString());
        sb.Append(")");
        return sb.ToString();
      }

    }


    #if !SILVERLIGHT
    [Serializable]
    #endif
    public partial class indexAlbum_result : TBase
    {

      public indexAlbum_result() {
      }

      public void Read (TProtocol iprot)
      {
        TField field;
        iprot.ReadStructBegin();
        while (true)
        {
          field = iprot.ReadFieldBegin();
          if (field.Type == TType.Stop) { 
            break;
          }
          switch (field.ID)
          {
            default: 
              TProtocolUtil.Skip(iprot, field.Type);
              break;
          }
          iprot.ReadFieldEnd();
        }
        iprot.ReadStructEnd();
      }

      public void Write(TProtocol oprot) {
        TStruct struc = new TStruct("indexAlbum_result");
        oprot.WriteStructBegin(struc);

        oprot.WriteFieldStop();
        oprot.WriteStructEnd();
      }

      public override string ToString() {
        StringBuilder sb = new StringBuilder("indexAlbum_result(");
        sb.Append(")");
        return sb.ToString();
      }

    }


    #if !SILVERLIGHT
    [Serializable]
    #endif
    public partial class query_args : TBase
    {
      private com.coveo.blitz.thrift.Query _query;

      public com.coveo.blitz.thrift.Query Query
      {
        get
        {
          return _query;
        }
        set
        {
          __isset.query = true;
          this._query = value;
        }
      }


      public Isset __isset;
      #if !SILVERLIGHT
      [Serializable]
      #endif
      public struct Isset {
        public bool query;
      }

      public query_args() {
      }

      public void Read (TProtocol iprot)
      {
        TField field;
        iprot.ReadStructBegin();
        while (true)
        {
          field = iprot.ReadFieldBegin();
          if (field.Type == TType.Stop) { 
            break;
          }
          switch (field.ID)
          {
            case 1:
              if (field.Type == TType.Struct) {
                Query = new com.coveo.blitz.thrift.Query();
                Query.Read(iprot);
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            default: 
              TProtocolUtil.Skip(iprot, field.Type);
              break;
          }
          iprot.ReadFieldEnd();
        }
        iprot.ReadStructEnd();
      }

      public void Write(TProtocol oprot) {
        TStruct struc = new TStruct("query_args");
        oprot.WriteStructBegin(struc);
        TField field = new TField();
        if (Query != null && __isset.query) {
          field.Name = "query";
          field.Type = TType.Struct;
          field.ID = 1;
          oprot.WriteFieldBegin(field);
          Query.Write(oprot);
          oprot.WriteFieldEnd();
        }
        oprot.WriteFieldStop();
        oprot.WriteStructEnd();
      }

      public override string ToString() {
        StringBuilder sb = new StringBuilder("query_args(");
        sb.Append("Query: ");
        sb.Append(Query== null ? "<null>" : Query.ToString());
        sb.Append(")");
        return sb.ToString();
      }

    }


    #if !SILVERLIGHT
    [Serializable]
    #endif
    public partial class query_result : TBase
    {
      private com.coveo.blitz.thrift.QueryResponse _success;

      public com.coveo.blitz.thrift.QueryResponse Success
      {
        get
        {
          return _success;
        }
        set
        {
          __isset.success = true;
          this._success = value;
        }
      }


      public Isset __isset;
      #if !SILVERLIGHT
      [Serializable]
      #endif
      public struct Isset {
        public bool success;
      }

      public query_result() {
      }

      public void Read (TProtocol iprot)
      {
        TField field;
        iprot.ReadStructBegin();
        while (true)
        {
          field = iprot.ReadFieldBegin();
          if (field.Type == TType.Stop) { 
            break;
          }
          switch (field.ID)
          {
            case 0:
              if (field.Type == TType.Struct) {
                Success = new com.coveo.blitz.thrift.QueryResponse();
                Success.Read(iprot);
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            default: 
              TProtocolUtil.Skip(iprot, field.Type);
              break;
          }
          iprot.ReadFieldEnd();
        }
        iprot.ReadStructEnd();
      }

      public void Write(TProtocol oprot) {
        TStruct struc = new TStruct("query_result");
        oprot.WriteStructBegin(struc);
        TField field = new TField();

        if (this.__isset.success) {
          if (Success != null) {
            field.Name = "Success";
            field.Type = TType.Struct;
            field.ID = 0;
            oprot.WriteFieldBegin(field);
            Success.Write(oprot);
            oprot.WriteFieldEnd();
          }
        }
        oprot.WriteFieldStop();
        oprot.WriteStructEnd();
      }

      public override string ToString() {
        StringBuilder sb = new StringBuilder("query_result(");
        sb.Append("Success: ");
        sb.Append(Success== null ? "<null>" : Success.ToString());
        sb.Append(")");
        return sb.ToString();
      }

    }


    #if !SILVERLIGHT
    [Serializable]
    #endif
    public partial class reset_args : TBase
    {

      public reset_args() {
      }

      public void Read (TProtocol iprot)
      {
        TField field;
        iprot.ReadStructBegin();
        while (true)
        {
          field = iprot.ReadFieldBegin();
          if (field.Type == TType.Stop) { 
            break;
          }
          switch (field.ID)
          {
            default: 
              TProtocolUtil.Skip(iprot, field.Type);
              break;
          }
          iprot.ReadFieldEnd();
        }
        iprot.ReadStructEnd();
      }

      public void Write(TProtocol oprot) {
        TStruct struc = new TStruct("reset_args");
        oprot.WriteStructBegin(struc);
        oprot.WriteFieldStop();
        oprot.WriteStructEnd();
      }

      public override string ToString() {
        StringBuilder sb = new StringBuilder("reset_args(");
        sb.Append(")");
        return sb.ToString();
      }

    }


    #if !SILVERLIGHT
    [Serializable]
    #endif
    public partial class reset_result : TBase
    {

      public reset_result() {
      }

      public void Read (TProtocol iprot)
      {
        TField field;
        iprot.ReadStructBegin();
        while (true)
        {
          field = iprot.ReadFieldBegin();
          if (field.Type == TType.Stop) { 
            break;
          }
          switch (field.ID)
          {
            default: 
              TProtocolUtil.Skip(iprot, field.Type);
              break;
          }
          iprot.ReadFieldEnd();
        }
        iprot.ReadStructEnd();
      }

      public void Write(TProtocol oprot) {
        TStruct struc = new TStruct("reset_result");
        oprot.WriteStructBegin(struc);

        oprot.WriteFieldStop();
        oprot.WriteStructEnd();
      }

      public override string ToString() {
        StringBuilder sb = new StringBuilder("reset_result(");
        sb.Append(")");
        return sb.ToString();
      }

    }


    #if !SILVERLIGHT
    [Serializable]
    #endif
    public partial class ping_args : TBase
    {

      public ping_args() {
      }

      public void Read (TProtocol iprot)
      {
        TField field;
        iprot.ReadStructBegin();
        while (true)
        {
          field = iprot.ReadFieldBegin();
          if (field.Type == TType.Stop) { 
            break;
          }
          switch (field.ID)
          {
            default: 
              TProtocolUtil.Skip(iprot, field.Type);
              break;
          }
          iprot.ReadFieldEnd();
        }
        iprot.ReadStructEnd();
      }

      public void Write(TProtocol oprot) {
        TStruct struc = new TStruct("ping_args");
        oprot.WriteStructBegin(struc);
        oprot.WriteFieldStop();
        oprot.WriteStructEnd();
      }

      public override string ToString() {
        StringBuilder sb = new StringBuilder("ping_args(");
        sb.Append(")");
        return sb.ToString();
      }

    }


    #if !SILVERLIGHT
    [Serializable]
    #endif
    public partial class ping_result : TBase
    {

      public ping_result() {
      }

      public void Read (TProtocol iprot)
      {
        TField field;
        iprot.ReadStructBegin();
        while (true)
        {
          field = iprot.ReadFieldBegin();
          if (field.Type == TType.Stop) { 
            break;
          }
          switch (field.ID)
          {
            default: 
              TProtocolUtil.Skip(iprot, field.Type);
              break;
          }
          iprot.ReadFieldEnd();
        }
        iprot.ReadStructEnd();
      }

      public void Write(TProtocol oprot) {
        TStruct struc = new TStruct("ping_result");
        oprot.WriteStructBegin(struc);

        oprot.WriteFieldStop();
        oprot.WriteStructEnd();
      }

      public override string ToString() {
        StringBuilder sb = new StringBuilder("ping_result(");
        sb.Append(")");
        return sb.ToString();
      }

    }

  }
}
