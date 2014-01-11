using BlitzIndex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.coveo.blitz.thrift.AST
{
	class LiteralNode : Node
	{
		private readonly string[] words;

		public LiteralNode(string value)
		{
			words = TextTokenizer.Tokenize(value).Select(t => t.Value).ToArray();
		}

		public override NodeType NodeType
		{
			get { return NodeType.Literal; }
		}

		public override HashSet<SearchResult> Evaluate(Database db)
		{
			if (words.Length == 0) return new HashSet<SearchResult>();

			var results = db.Query(words[0]);
			if (words.Length > 1)
				results.RemoveWhere(result => !MatchesFullText(result));

			return results;
		}

		private bool MatchesFullText(SearchResult result, int occurenceIndex)
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

		private bool MatchesFullText(SearchResult result)
		{
			// At least one of the occurence should also match the other words
			foreach (var occurenceIndex in result.Occurrences)
				if (MatchesFullText(result, occurenceIndex))
					return true;
			return false;
		}
	}
}
