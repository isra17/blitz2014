using com.coveo.blitz.thrift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thrift.Collections;

namespace BlitzIndex
{
	public interface IDocument
	{
		string Id { get; }
		DocumentType Type { get; }
		IEnumerable<string> Keywords { get; }

		THashSet<string> GetFacet(string name);
	}
}
