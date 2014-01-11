using BlitzIndex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.coveo.blitz.thrift
{
    class QueryEvaluator
    {
		private sealed class ScoredResult
		{
			public IDocument Document;
			public double Score;
		}

		private readonly Query query;
        private Dictionary<int, QueryTreeNode> treeNodes;
        private int rootId;

        public static IEnumerable<IDocument> EvaluateQuery(Database db, Query query)
        {
            return new QueryEvaluator(query).Evaluate(db);
        }

        public QueryEvaluator(Query query)
        {
			this.query = query;
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
			var results = GetScoredResults(db);

			FilterFacets(results);

			results.Sort((a, b) =>
				{
					int comparison = a.Score.CompareTo(b.Score);
					if (comparison == 0) comparison = string.Compare(a.Document.Id, b.Document.Id);
					return comparison;
				});
			return results.Select(result => result.Document);
        }

		private List<ScoredResult> GetScoredResults(Database db)
		{
			var results = new List<ScoredResult>();
			var scores = new Dictionary<IDocument, ScoredResult>();
			foreach (var document in FindDocuments(db, RootNode))
            {
                double tf = Math.Log10(document.Occurrences.Count + 1);
                double idf = Math.Log10((double)db.Count / (double)(db.CountDocumentsWithTerm(document.Term) + 1));
                double product = tf * idf;

				ScoredResult result;
				if (scores.TryGetValue(document.Document, out result))
				{
					result.Score += product;
				}
				else
				{
					result = new ScoredResult
					{
						Document = document.Document,
						Score = product
					};
					results.Add(result);
					scores.Add(document.Document, result);
				}
			}

			return results;
		}

		private void FilterFacets(List<ScoredResult> results)
		{
			if (query.FacetFilters == null) return;
			for (int i = results.Count - 1; i >= 0; --i)
			{
				var result = results[i];
				if (!PassesFacetFilters(result.Document))
				{
					results[i] = results[results.Count - 1];
					results.RemoveAt(results.Count - 1);
				}
			}
		}

		private bool PassesFacetFilters(IDocument document)
		{
			foreach (var facetFilter in query.FacetFilters)
			{
				var documentFacetValues = document.GetFacetValues(facetFilter.MetadataName);

				// We need at least one of the facet filter values
				bool passes = false;
				foreach (var facetFilterValue in facetFilter.Values)
				{
					if (documentFacetValues.Contains(facetFilterValue))
					{
						passes = true;
						break;
					}
				}

				if (!passes) return false;
			}

			return true;
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
                var searchString = specificNode.Value.Trim();
                if (searchString == "*")
                {
                    return db.Query(searchString);
                }

                var words = TextTokenizer.Tokenize(specificNode.Value).Select(t => t.Value).ToArray();
                if (words.Length == 0)
                    return new HashSet<SearchResult>();

                var results = db.Query(words[0]);
                if (words.Length > 1)
                    results.RemoveWhere(result => !MatchesFullText(result, words));

                return results;
            }

            var operatorName = specificNode.Value.ToUpperInvariant().Trim();
            if (operatorName == "NOT")
            {
                int index = specificNode.RightPart;
                if (index == -1)
                    index = specificNode.LeftPart;

                var set = FindDocuments(db, treeNodes[index]);
                HashSet<SearchResult> all = db.Query("*");
                foreach (SearchResult r in set)
                    all.Remove(r);
                return all;
            }
            else
            {
                var right = FindDocuments(db, treeNodes[specificNode.RightPart]);
                var left = FindDocuments(db, treeNodes[specificNode.LeftPart]);
                if (operatorName == "AND")
                {
                    left.IntersectWith(right);
                }
                else if (operatorName == "OR")
                {
                    left.UnionWith(right);
                }
                return left;
            }
        }
    }
}
