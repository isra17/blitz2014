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

		public IEnumerable<string> Keywords
		{
			get { return TextTokenizer.Tokenize(Text); }
		}

		public IEnumerable<string> FacetNames
		{
			get { return facetNames; }
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
