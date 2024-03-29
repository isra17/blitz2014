﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thrift.Collections;

namespace com.coveo.blitz.thrift
{
	public interface IDocument
	{
		string Id { get; }
		string Text { get; }
		DocumentType Type { get; }
		IEnumerable<TextToken> Keywords { get; }
		string[] FacetNames { get; }

		void FullText();
		THashSet<string> GetFacetValues(string name);
	}
}
