using com.coveo.blitz.thrift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlitzIndex
{
    static class PrettyPrint
    {
        private static string RecursivePrintTree(Dictionary<int, QueryTreeNode> inputNodes, QueryTreeNode specificNode)
        {
            if (specificNode.Type == NodeType.LITERAL)
            {
                return specificNode.Value.Trim();
            }

            var op = specificNode.Value.ToUpperInvariant();
            if (op == "NOT")
            {
                return string.Format("NOT {0}", RecursivePrintTree(inputNodes, inputNodes[specificNode.LeftPart]));
            }

            return string.Format("({0} {1} {2})",
                RecursivePrintTree(inputNodes, inputNodes[specificNode.LeftPart]),
                op,
                RecursivePrintTree(inputNodes, inputNodes[specificNode.RightPart]));
        }

        public static void PrintQuery(Query query)
        {
            Dictionary<int, QueryTreeNode> inputNodes = query.QueryTreeNodes.ToDictionary(n => n.Id);
            Console.WriteLine(RecursivePrintTree(inputNodes, inputNodes[query.RootId]));
        }
    }
}
