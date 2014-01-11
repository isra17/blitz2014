﻿using com.coveo.blitz.thrift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thrift.Collections;

namespace com.coveo.blitz.thrift
{
	partial class Album : IDocument
	{
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

		public THashSet<string> GetFacet(string name)
		{
			throw new NotImplementedException();
		}
	}
}