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

		public static IEnumerable<string> Tokenize(string text)
		{
			Contract.Requires(text != null);

			Match match = tokenizeRegex.Match(text);
			while (match.Success)
			{
				bool valid = false;
				foreach (var c in match.Value)
				{
					if (c != '-' && c != '_')
					{
						valid = true;
						break;
					}
				}

				if (valid) yield return match.Value;

				match = match.NextMatch();
			}
		}
	}
}
