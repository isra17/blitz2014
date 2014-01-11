﻿using BlitzIndex;
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

		private bool MatchesFullText(SearchResult result, string[] words, int occurenceIndex)
		{
			int nextTokenStartIndex = occurenceIndex + words[0].Length;
			for (int wordIndex = 1; wordIndex < words.Length; ++wordIndex)
			{
				var token = TextTokenizer.GetNextToken(result.Document.Text, nextTokenStartIndex);
				if (!token.HasValue || token.Value.Value != words[wordIndex])
					return false;
			}

			return true;
		}

		private bool MatchesFullText(SearchResult result, string[] words)
		{
			// At least one of the occurence should also match the other words
			foreach (var occurenceIndex in result.Occurrences)
				if (MatchesFullText(result, words, occurenceIndex))
					return true;
			return false;
		}

        private HashSet<SearchResult> FindDocuments(Database db, QueryTreeNode specificNode)
        {
            if (specificNode.Type == NodeType.LITERAL)
            {
				var words = TextTokenizer.Tokenize(specificNode.Value).Select(t => t.Value).ToArray();
				if (words.Length == 0) return new HashSet<SearchResult>();

				var results = db.Query(words[0]);
				if (words.Length > 1)
					results.RemoveWhere(result => !MatchesFullText(result, words));

				return results;
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