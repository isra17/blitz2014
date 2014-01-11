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
		public static TextToken? GetNextToken(string text, int index)
		{
			if (text == null) return null;

			int wordStartIndex = -1;
			bool hasLetterOrDigit = false;
			while (true)
			{
				if (index >= text.Length) break;

				char c = text[index];
				if (char.IsLetterOrDigit(c))
				{
					if (wordStartIndex == -1) wordStartIndex = index;
					hasLetterOrDigit = true;
				}
				else if (c == '-' || c == '_')
				{
					if (wordStartIndex == -1) wordStartIndex = index;
				}
				else
				{
					if (wordStartIndex >= 0)
					{
						if (hasLetterOrDigit) break;
						hasLetterOrDigit = false;
						wordStartIndex = -1;
					}
				}

				index++;
			}

			if (wordStartIndex == -1 || !hasLetterOrDigit) return null;

			string tokenText = text.Substring(wordStartIndex, index - wordStartIndex).ToUpperInvariant();
			return new TextToken(wordStartIndex, tokenText);
		}

		public static IEnumerable<TextToken> Tokenize(string text)
		{
			int index = 0;
			while (true)
			{
				var token = GetNextToken(text, index);
				if (!token.HasValue) break;

				yield return token.Value;
				index = token.Value.EndIndex;
			}
		}
	}
}
