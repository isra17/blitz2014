using BlitzIndex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.coveo.blitz.thrift
{
    class QueryEvaluator
    {
        private Dictionary<int, QueryTreeNode> treeNodes;
        private int rootId;

        public static IEnumerable<IDocument> EvaluateQuery(Database db, Query query)
        {
            return new QueryEvaluator(query).Evaluate(db);
        }

        public QueryEvaluator(Query query)
        {
            treeNodes = query.QueryTreeNodes.ToDictionary(n => n.Id);
            rootId = query.RootId;
        }

        private QueryTreeNode RootNode
        {
            get { return treeNodes[rootId]; }
        }

        public IEnumerable<IDocument> Evaluate(Database db)
        {
            // tri par pertinence
            Dictionary<IDocument, double> scores = new Dictionary<IDocument, double>();

            foreach (var result in FindDocuments(db, RootNode))
            {
                double tf = Math.Log10(result.Occurrences.Count + 1);
                double idf = Math.Log10(db.Count / (db.CountDocumentsWithTerm(result.Term) + 1));
                double product = tf * idf;
                if (scores.ContainsKey(result.Document))
                {
                    scores[result.Document] += product;
                }
                else
                {
                    scores[result.Document] = product;
                }
            }

            return scores
                .OrderByDescending(pair => pair.Value)
                .ThenBy(pair => pair.Key.Id, StringComparer.InvariantCultureIgnoreCase)
                .Select(pair => pair.Key);
        }

        private HashSet<SearchResult> FindDocuments(Database db, QueryTreeNode specificNode)
        {
            if (specificNode.Type == NodeType.LITERAL)
            {
                return db.Query(specificNode.Value);
            }

            var operatorName = specificNode.Value.ToUpperInvariant().Trim();
            var left = FindDocuments(db, treeNodes[specificNode.LeftPart]);
            var right = FindDocuments(db, treeNodes[specificNode.RightPart]);
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
    }
}
