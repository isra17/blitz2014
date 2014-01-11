using BlitzIndex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.coveo.blitz.thrift.AST
{
	sealed class WildcardNode : Node
	{
		public static readonly WildcardNode Instance = new WildcardNode();

		public override NodeType NodeType
		{
			get { return AST.NodeType.Wildcard; }
		}

		public override HashSet<SearchResult> Evaluate(Database db)
		{
			return db.Query("*");
		}
	}
}
