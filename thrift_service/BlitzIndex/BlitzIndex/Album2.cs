using com.coveo.blitz.thrift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thrift.Collections;

namespace com.coveo.blitz.thrift
{
	partial class Album : IDocument
	{
		private static readonly string[] facetNames = { "name", "artists", "release_date", "genres", "track_names" };

		public DocumentType Type
		{
			get { return DocumentType.ALBUM; }
		}

		public IEnumerable<TextToken> Keywords
		{
			get { return TextTokenizer.Tokenize(Text); }
		}

		public string[] FacetNames
		{
			get { return facetNames; }
		}

		public void FullText()
		{
			StringBuilder builder = new StringBuilder(_text);
			AppendSet(builder, _name);
			AppendSet(builder, _artists);
			AppendSet(builder, _release_date);
			AppendSet(builder, _genres);
			AppendSet(builder, _track_names);
			_text = builder.ToString();
		}

		public static void AppendSet(StringBuilder builder, THashSet<string> set)
		{
			if (set == null) return;
			builder.Append(';');
			foreach (var val in set)
			{
				builder.Append(val);
				builder.Append(';');
			}
		}

		public THashSet<string> GetFacetValues(string name)
		{
			if (name == "name") return _name;
			if (name == "artists") return _artists;
			if (name == "release_date") return _release_date;
			if (name == "genres") return _genres;
			if (name == "track_names") return _track_names;
			return null;
		}
	}
}
