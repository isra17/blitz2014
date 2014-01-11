using com.coveo.blitz.thrift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlitzIndex
{
	sealed class QueryResponseBuilder
	{
		private sealed class FacetResultBuilder
		{
			public readonly FacetResult Result;
			private readonly Dictionary<string, FacetValue> values
				= new Dictionary<string, FacetValue>(StringComparer.Ordinal);

			public FacetResultBuilder(string name)
			{
				Result = new FacetResult();
				Result.Values = new List<FacetValue>();
				Result.MetadataName = name;
			}

			public void AddValue(string value)
			{
				FacetValue facetValue;
				if (!values.TryGetValue(value, out facetValue))
				{
					facetValue = new FacetValue();
					facetValue.Value = value;
					Result.Values.Add(facetValue);
				}

				facetValue.Count++;
			}
		}

		private readonly QueryResponse response;
		private readonly Dictionary<string, FacetResultBuilder> facetResults
			= new Dictionary<string, FacetResultBuilder>(StringComparer.Ordinal);

		public QueryResponseBuilder()
		{
			response = new QueryResponse();
			response.Results = new List<QueryResult>();
			response.Facets = new List<FacetResult>();
		}

		public void AddNewDocument(SearchResult searchResult)
		{
            var document = searchResult.Document;
			response.Results.Add(new QueryResult
			{
                DocumentType = document.Type,
                Id = document.Id,
			});

            foreach (var facetName in document.FacetNames)
			{
				var values = document.GetFacetValues(facetName);
				if (values != null)
				{
					var facetResult = GetFacetResult(facetName);
					foreach (string value in values)
						facetResult.AddValue(value.Trim());
				}
			}
		}

		private FacetResultBuilder GetFacetResult(string name)
		{
			FacetResultBuilder facetResult;
			if (!facetResults.TryGetValue(name, out facetResult))
			{
				facetResult = new FacetResultBuilder(name);
				facetResults.Add(name, facetResult);
				response.Facets.Add(facetResult.Result);
			}
			return facetResult;
		}

		public QueryResponse Build()
		{
			response.Results.Sort((a, b) => string.Compare(a.Id, b.Id, StringComparison.InvariantCultureIgnoreCase));
			return response;
		}
	}
}
