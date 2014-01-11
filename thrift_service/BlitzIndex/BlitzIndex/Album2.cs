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
		private static readonly string[] facetNames = { "name", "artists", "release date", "genres", "track names" };

		public DocumentType Type
		{
			get { return DocumentType.ALBUM; }
		}

		public IEnumerable<string> Keywords
		{
			get
			{
				return TextTokenizer.Tokenize(Text);
			}
		}

		public IEnumerable<string> FacetNames
		{
			get { return facetNames; }
		}

		public THashSet<string> GetFacetValues(string name)
		{
			if (name == "name") return _name;
			if (name == "artists") return _artists;
			if (name == "release date") return _release_date;
			if (name == "genres") return _genres;
			if (name == "track names") return _track_names;
			return null;
		}
	}
}
