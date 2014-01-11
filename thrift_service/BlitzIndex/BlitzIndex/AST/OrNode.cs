using BlitzIndex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.coveo.blitz.thrift.AST
{
	class OrNode : Node
	{
		public OrNode(Node left, Node right)
		{
			LeftHand = left;
			RightHand = right;
		}

		public Node LeftHand { get; private set; }
		public Node RightHand { get; private set; }

		public override NodeType NodeType
		{
			get { return AST.NodeType.Or; }
		}

		public override HashSet<SearchResult> Evaluate(Database db)
		{
			var left = LeftHand.Evaluate(db);
			var right = RightHand.Evaluate(db);
			left.UnionWith(right);
			return left;
		}
	}
}
