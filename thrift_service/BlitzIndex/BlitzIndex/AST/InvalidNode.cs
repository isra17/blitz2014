using BlitzIndex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.coveo.blitz.thrift.AST
{
	/// <summary>
	/// On aime mieux retourner une node qui fait rien que de crasher
	/// </summary>
	class InvalidNode : Node
	{
		public static readonly InvalidNode Instance = new InvalidNode();
		private static readonly HashSet<SearchResult> emptySet = new HashSet<SearchResult>();

		public override NodeType NodeType
		{
			get { return NodeType.Invalid; }
		}

		public override HashSet<SearchResult> Evaluate(Database db)
		{
			return emptySet;
		}
	}
}
