using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thrift.Collections;

namespace com.coveo.blitz.thrift
{
	partial class Artist : IDocument
	{
		private static readonly string[] facetNames = { "name", "origin", "active_start", "active_end", "genres", "labels", "albums", "group_names", "instruments_played" };

		public DocumentType Type
		{
			get { return DocumentType.ARTIST; }
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
			Album.AppendSet(builder, _name);
			Album.AppendSet(builder, _origin);
			Album.AppendSet(builder, _active_start);
			Album.AppendSet(builder, _active_end);
			Album.AppendSet(builder, _genres);
			Album.AppendSet(builder, _labels);
			Album.AppendSet(builder, _albums);
			Album.AppendSet(builder, _group_names);
			Album.AppendSet(builder, _instruments_played);
			_text = builder.ToString();
		}

		public THashSet<string> GetFacetValues(string name)
		{
			if (name == "name") return _name;
			if (name == "origin") return _origin;
			if (name == "active_start") return _active_start;
			if (name == "active_end") return _active_end;
			if (name == "genres") return _genres;
			if (name == "labels") return _labels;
			if (name == "albums") return _albums;
			if (name == "group_names") return _group_names;
			if (name == "instruments_played") return _instruments_played;
			return null;
		}
	}
}
