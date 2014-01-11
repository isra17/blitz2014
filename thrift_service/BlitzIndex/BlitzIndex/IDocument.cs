using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlitzIndex
{
	public interface IDocument
	{
		IEnumerable<string> Keywords { get; }
	}
}
