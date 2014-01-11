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
                return new LiteralNode(currentNode.Value);
            }

            var op = currentNode.Value.Trim().ToUpperInvariant();
            if (op == "NOT")
            {
                int index = currentNode.LeftPart;
                if (index == -1)
                    index = currentNode.RightPart;

                Node child = NodeForQueryTreeNode(input, input[index]);
                return new UnaryNode(child);
            }
            
            Node left = NodeForQueryTreeNode(input, input[currentNode.LeftPart]);
            Node right = NodeForQueryTreeNode(input, input[currentNode.RightPart]);
            if (op == "AND")
            {
                return new BinaryNode(NodeType.And, left, right);
            }

            if (op == "OR")
            {
                return new BinaryNode(NodeType.Or, left, right);
            }

            return InvalidNode.Instance;
        }

        public abstract NodeType NodeType { get; }
        public abstract HashSet<SearchResult> Evaluate(Database db);
    }

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

    class LiteralNode : Node
    {
        public LiteralNode(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }

        public override NodeType NodeType
        {
	        get { return NodeType.Literal; }
        }

        public override HashSet<SearchResult> Evaluate(Database db)
        {
 	        return db.Query(Value);
        }
    }

    class UnaryNode : Node
    {
        public UnaryNode(Node child)
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

    class BinaryNode : Node
    {
        private NodeType type;

        public BinaryNode(NodeType type, Node left, Node right)
        {
            LeftHand = left;
            RightHand = right;
            this.type = type;
        }

        public Node LeftHand { get; private set; }
        public Node RightHand { get; private set; }

        public override NodeType NodeType
        {
            get { return type; }
        }

        public override HashSet<SearchResult> Evaluate(Database db)
        {
            var left = LeftHand.Evaluate(db);
            var right = RightHand.Evaluate(db);
            if (type == AST.NodeType.And)
            {
                left.IntersectWith(right);
            }
            else // assume 'or'
            {
                left.UnionWith(right);
            }
            return left;
        }
    }
}
