using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atrico.Lib.Common.Collections.Tree
{
    public partial class Tree<T>
    {
        private class Node : INode<T>
        {
            private readonly T _data;
            private readonly IList<Node> _children = new List<Node>();

            public T Data
            {
                get { return _data; }
            }

            public INode<T> Add(T data)
            {
                var node = _children.FirstOrDefault(n => n.Equals(data));
                if (node != null) return node;
                node = new Node(data);
                _children.Add(node);
                return node;
            }

            public INode<T> Add(IEnumerable<T> path)
            {
                var pathArray = path as T[] ?? path.ToArray();
                if (pathArray.Length == 0) return this;
                var node = Add(pathArray[0]);
                return node.Add(pathArray.Skip(1));
            }

            protected Node(T data)
            {
                _data = data;
            }

            public bool Equals(T other)
            {
                return _data.Equals(other);
            }

            public IEnumerable<string> ToMultilineString()
            {
                var lines = new List<string>();
                var depths = new List<int>();
                GetDepths(0, depths);
                ToMultilineString(0, lines, depths, NodeType.Middle);
                return lines;
            }

            private void GetDepths(int depth, ICollection<int> depths)
            {
                var nextDepth = (this is RootNode) ? depth : depth + 1;
                var child = 0;
                for (; child < _children.Count() / 2; ++child)
                {
                    _children[child].GetDepths(nextDepth, depths);
                }
                if (!(this is RootNode)) depths.Add(depth);
                for (; child < _children.Count(); ++child)
                {
                    _children[child].GetDepths(nextDepth, depths);
                }
            }

            enum NodeType
            {
                First,
                Middle,
                Last
            }
            private void ToMultilineString(int depth, ICollection<string> lines, IList<int> depths, NodeType nodeType)
            {
                var lineNum = lines.Count();
                var nextDepth = (this is RootNode) ? depth : depth + 1;
                var child = 0;
                for (; child < _children.Count() / 2; ++child)
                {
                    _children[child].ToMultilineString(nextDepth, lines, depths, child == 0 ? NodeType.First : NodeType.Middle);
                }
                if (!(this is RootNode))
                {
                    var line = new StringBuilder();
                    for (var i = 0; i < depth; ++i)
                    {
                        line.Append(IsBranchAtThisDepth(i, lineNum, depths) ? "| " : "  ");
                    }
                    switch (nodeType)
                    {
                        case NodeType.First:
                            line.Append(depth == 0 ? "+-" : "/-");
                            break;
                        case NodeType.Last:
                            line.Append(depth == 0 ? "+-" : @"\-");
                            break;
                        default:
                            line.Append("+-");
                            break;
                    }
                    line.Append(_data);
                    lines.Add(line.ToString());
                }
                for (; child < _children.Count(); ++child)
                {
                    _children[child].ToMultilineString(nextDepth, lines, depths, child == _children.Count() - 1? NodeType.Last : NodeType.Middle);
                }
            }

            private static bool IsBranchAtThisDepth(int depth, int lineNum, IList<int> depths)
            {
                int? above = null;
                for (var i = lineNum - 1; i >= 0; --i)
                {
                    if (depths[i] > depth) continue;
                    if (depths[i] < (depth - 1)) return false;
                    above = depths[i];
                    break;
                }
                int? below = null;
                for (var i = lineNum + 1; i < depths.Count; ++i)
                {
                    if (depths[i] > depth) continue;
                    if (depths[i] < (depth - 1)) return false;
                    below = depths[i];
                    break;
                }
                return above.HasValue && below.HasValue;
            }
        }

        private class RootNode : Node
        {
            public RootNode()
                : base(default(T))
            {
            }
        }
    }
}