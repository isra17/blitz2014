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

  /// <summary>
  /// Une "Query" est une requete faite a votre index. Votre index doit repondre de maniere appropriee
  /// et le plus rapidement possible.
  /// 
  /// Voir la documentation du concours pour savoir ce que signifie une reponse "appropriee".
  /// </summary>
  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class Query : TBase
  {
    private int _rootId;
    private List<QueryTreeNode> _queryTreeNodes;
    private List<FacetFilter> _facetFilters;

    /// <summary>
    /// L'identifiant du noeud racine de l'arbre syntaxique de cette requete.
    /// </summary>
    public int RootId
    {
      get
      {
        return _rootId;
      }
      set
      {
        __isset.rootId = true;
        this._rootId = value;
      }
    }

    /// <summary>
    /// Liste des noeuds composant l'arbre.
    /// </summary>
    public List<QueryTreeNode> QueryTreeNodes
    {
      get
      {
        return _queryTreeNodes;
      }
      set
      {
        __isset.queryTreeNodes = true;
        this._queryTreeNodes = value;
      }
    }

    /// <summary>
    /// Liste de filtres sur les facettes
    /// </summary>
    public List<FacetFilter> FacetFilters
    {
      get
      {
        return _facetFilters;
      }
      set
      {
        __isset.facetFilters = true;
        this._facetFilters = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool rootId;
      public bool queryTreeNodes;
      public bool facetFilters;
    }

    public Query() {
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
            if (field.Type == TType.I32) {
              RootId = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.List) {
              {
                QueryTreeNodes = new List<QueryTreeNode>();
                TList _list60 = iprot.ReadListBegin();
                for( int _i61 = 0; _i61 < _list60.Count; ++_i61)
                {
                  QueryTreeNode _elem62 = new QueryTreeNode();
                  _elem62 = new QueryTreeNode();
                  _elem62.Read(iprot);
                  QueryTreeNodes.Add(_elem62);
                }
                iprot.ReadListEnd();
              }
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.List) {
              {
                FacetFilters = new List<FacetFilter>();
                TList _list63 = iprot.ReadListBegin();
                for( int _i64 = 0; _i64 < _list63.Count; ++_i64)
                {
                  FacetFilter _elem65 = new FacetFilter();
                  _elem65 = new FacetFilter();
                  _elem65.Read(iprot);
                  FacetFilters.Add(_elem65);
                }
                iprot.ReadListEnd();
              }
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
      TStruct struc = new TStruct("Query");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.rootId) {
        field.Name = "rootId";
        field.Type = TType.I32;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(RootId);
        oprot.WriteFieldEnd();
      }
      if (QueryTreeNodes != null && __isset.queryTreeNodes) {
        field.Name = "queryTreeNodes";
        field.Type = TType.List;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteListBegin(new TList(TType.Struct, QueryTreeNodes.Count));
          foreach (QueryTreeNode _iter66 in QueryTreeNodes)
          {
            _iter66.Write(oprot);
          }
          oprot.WriteListEnd();
        }
        oprot.WriteFieldEnd();
      }
      if (FacetFilters != null && __isset.facetFilters) {
        field.Name = "facetFilters";
        field.Type = TType.List;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteListBegin(new TList(TType.Struct, FacetFilters.Count));
          foreach (FacetFilter _iter67 in FacetFilters)
          {
            _iter67.Write(oprot);
          }
          oprot.WriteListEnd();
        }
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("Query(");
      sb.Append("RootId: ");
      sb.Append(RootId);
      sb.Append(",QueryTreeNodes: ");
      sb.Append(QueryTreeNodes);
      sb.Append(",FacetFilters: ");
      sb.Append(FacetFilters);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
