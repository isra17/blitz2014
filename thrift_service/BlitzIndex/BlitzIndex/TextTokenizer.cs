using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace com.coveo.blitz.thrift
{
	public static class TextTokenizer
	{
		private static readonly Regex tokenizeRegex = new Regex(
			@"[\p{L}-_\d]+", RegexOptions.Compiled | RegexOptions.CultureInvariant);

		public static IEnumerable<KeyValuePair<int, string>> Tokenize(string text)
		{
			if (text == null) yield break;

			Match match = tokenizeRegex.Match(text);
			while (match.Success)
			{
				// Check that it's not just dashes or underscores
				bool valid = false;
				foreach (var c in match.Value)
				{
					if (c != '-' && c != '_')
					{
						valid = true;
						break;
					}
				}

				if (valid) yield return new KeyValuePair<int, string>(match.Index, match.Value.ToUpperInvariant());

				match = match.NextMatch();
			}
		}
	}
}
