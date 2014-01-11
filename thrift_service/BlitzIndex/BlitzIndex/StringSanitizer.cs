using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thrift.Collections;

namespace com.coveo.blitz.thrift
{
	static class StringSanitizer
	{
		public static string Sanitize(string str)
		{
			return str == null ? null : str.ToLowerInvariant().Trim();
		}

		public static void Sanitize(THashSet<string> strings)
		{
			if (strings == null) return;

			List<KeyValuePair<string, string>> sanitizedStrings = null;
			foreach (string str in strings)
			{
				string sanitized = Sanitize(str);
				if (sanitized != str)
				{
					if (sanitizedStrings == null) sanitizedStrings = new List<KeyValuePair<string, string>>();
					sanitizedStrings.Add(new KeyValuePair<string, string>(str, sanitized));
				}
			}

			if (sanitizedStrings != null)
			{
				foreach (var entry in sanitizedStrings)
				{
					strings.Remove(entry.Key);
					strings.Add(entry.Value);
				}
			}
		}
	}
}
