using BlitzIndex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.coveo.blitz.thrift.AST
{
	class NotNode : Node
	{
		public NotNode(Node child)
		{
			Child = child;
		}

		public Node Child { get; private set; }

		public override NodeType NodeType
		{
			get { return NodeType.Not; }
		}

		public override HashSet<SearchResult> Evaluate(Database db)
		{
			HashSet<SearchResult> not = db.Query("*");
			foreach (SearchResult negate in Child.Evaluate(db))
			{
				not.Remove(negate);
			}
			return not;
		}
	}
}
