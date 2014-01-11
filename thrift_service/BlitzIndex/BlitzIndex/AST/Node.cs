using BlitzIndex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.coveo.blitz.thrift.AST
{
    enum NodeType
    {
        Invalid,
        Literal,
		Wildcard,
        Not,
        And,
        Or
    }

    abstract class Node
    {
        public static Node BuildAst(Query query)
        {
            int root = query.RootId;
            Dictionary<int, QueryTreeNode> input = query.QueryTreeNodes.ToDictionary(n => n.Id);
            return NodeForQueryTreeNode(input, input[root]);
        }

        private static Node NodeForQueryTreeNode(Dictionary<int, QueryTreeNode> input, QueryTreeNode currentNode)
        {
            if (currentNode.Type == thrift.NodeType.LITERAL)
            {
				return currentNode.Value.Trim() == "*"
					? (Node)WildcardNode.Instance
					: (Node)new LiteralNode(currentNode.Value);
            }

            var op = currentNode.Value.Trim().ToUpperInvariant();
            if (op == "NOT")
            {
                int index = currentNode.LeftPart;
                if (index == -1)
                    index = currentNode.RightPart;

                Node child = NodeForQueryTreeNode(input, input[index]);
                return new NotNode(child);
            }
            
            Node left = NodeForQueryTreeNode(input, input[currentNode.LeftPart]);
            Node right = NodeForQueryTreeNode(input, input[currentNode.RightPart]);
            if (op == "AND") return new AndNode(left, right);
            if (op == "OR") return new OrNode(left, right);

            return InvalidNode.Instance;
        }

        public abstract NodeType NodeType { get; }
        public abstract HashSet<SearchResult> Evaluate(Database db);
    }
}
