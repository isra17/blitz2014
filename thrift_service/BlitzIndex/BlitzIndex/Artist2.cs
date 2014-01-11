using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thrift.Collections;

namespace com.coveo.blitz.thrift
{
	partial class Artist : IDocument
	{
		private static readonly string[] facetNames = { "name", "origin", "active start", "active end", "genres", "labels", "albums", "group names", "instruments played" };

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

		public void Sanitize()
		{
			StringSanitizer.Sanitize(_name);
			StringSanitizer.Sanitize(_origin);
			StringSanitizer.Sanitize(_active_start);
			StringSanitizer.Sanitize(_active_end);
			StringSanitizer.Sanitize(_genres);
			StringSanitizer.Sanitize(_labels);
			StringSanitizer.Sanitize(_albums);
			StringSanitizer.Sanitize(_group_names);
			StringSanitizer.Sanitize(_instruments_played);
		}

		public THashSet<string> GetFacetValues(string name)
		{
			if (name == "name") return _name;
			if (name == "origin") return _origin;
			if (name == "active start") return _active_start;
			if (name == "active end") return _active_end;
			if (name == "genres") return _genres;
			if (name == "labels") return _labels;
			if (name == "albums") return _albums;
			if (name == "group names") return _group_names;
			if (name == "instruments played") return _instruments_played;
			return null;
		}
	}
}
