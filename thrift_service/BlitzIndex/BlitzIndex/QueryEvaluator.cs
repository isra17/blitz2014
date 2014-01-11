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
        private readonly AST.Node expression;

        public static IEnumerable<IDocument> EvaluateQuery(Database db, Query query)
        {
            return new QueryEvaluator(query).Evaluate(db);
        }

        public QueryEvaluator(Query query)
        {
			this.query = query;
            expression = AST.Node.BuildAst(query);

			// sanitize facet filters
			if (query.FacetFilters != null)
			{
				foreach (var facetFilter in query.FacetFilters)
				{
					facetFilter.MetadataName = facetFilter.MetadataName.ToLowerInvariant().Trim();
					if (facetFilter.Values != null)
					{
						for (int i = 0; i < facetFilter.Values.Count; ++i)
						{
							facetFilter.Values[i] = facetFilter.Values[i].Trim();
						}
					}
				}
			}
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

			foreach (var document in expression.Evaluate(db))
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
				// We need at least one of the facet filter values
				var documentFacetValues = document.GetFacetValues(facetFilter.MetadataName);
				if (documentFacetValues == null) return false;

				foreach (var facetFilterValue in facetFilter.Values)
				{
					foreach (string documentFacetValue in documentFacetValues)
					{
						if (string.Equals(documentFacetValue, facetFilterValue, StringComparison.OrdinalIgnoreCase))
							goto passes;
					}
				}

				return false;

			passes: ;
			}

			return true;
		}
    }
}
