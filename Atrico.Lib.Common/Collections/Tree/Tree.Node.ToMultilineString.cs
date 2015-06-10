using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atrico.Lib.Common.Collections.Tree
{
    public partial class Tree<T>
    {
        private abstract partial class Node
        {
            public IEnumerable<string> ToMultilineString()
            {
                var lines = new List<string>();
                var depths = new List<Tuple<int, ParentDirection>>();
                GetDepths(0, depths, ParentDirection.None);
                ToMultilineString(0, lines, depths, NodeType.Middle);
                return lines;
            }

            private void GetDepths(int depth, ICollection<Tuple<int, ParentDirection>> depths, ParentDirection parentDirection)
            {
                var nextDepth = this.IsRoot() ? depth : depth + 1;
                var child = 0;
                for (; child < _children.Count() / 2; ++child)
                {
                    _children[child].GetDepths(nextDepth, depths, this.IsRoot() ? ParentDirection.None : ParentDirection.Down);
                }
                if (!this.IsRoot()) depths.Add(Tuple.Create(depth, parentDirection));
                for (; child < _children.Count(); ++child)
                {
                    _children[child].GetDepths(nextDepth, depths, this.IsRoot() ? ParentDirection.None : ParentDirection.Up);
                }
            }

            private enum NodeType
            {
                First,
                Middle,
                Last,
                SingleRoot,
                FirstOfDoubleRoot
            }

            private enum ParentDirection
            {
                None,
                Up,
                Down
            }

            private void ToMultilineString(int depth, ICollection<string> lines, IList<Tuple<int, ParentDirection>> depths, NodeType nodeType)
            {
                var lineNum = lines.Count();
                var nextDepth = this.IsRoot() ? depth : depth + 1;
                var child = 0;
                for (; child < _children.Count() / 2; ++child)
                {
                    var type = NodeType.Middle;
                    if (child == 0)
                    {
                        if (this.IsRoot() && _children.Count() == 2) type = NodeType.FirstOfDoubleRoot;
                        else type = NodeType.First;
                    }
                    _children[child].ToMultilineString(nextDepth, lines, depths, type);
                }
                if (!this.IsRoot())
                {
                    var line = new StringBuilder();
                    for (var i = 0; i < depth; ++i)
                    {
                        line.Append(IsBranchAtThisDepth(i, lineNum, depths) ? VerticalLine : Space);
                        line.Append(Space);
                    }
                    switch (nodeType)
                    {
                        case NodeType.First:
                            line.Append(FirstChildNode);
                            break;
                        case NodeType.Last:
                            line.Append(LastChildNode);
                            break;
                        case NodeType.SingleRoot:
                            line.Append(SingleRoot);
                            break;
                        case NodeType.FirstOfDoubleRoot:
                            line.Append(FirstOfDoubleRoot);
                            break;
                        default:
                            line.Append(depth == 0 ? MidRoot : MidChildNode);
                            break;
                    }
                    line.Append(Dash);
                    line.Append(_data);
                    lines.Add(line.ToString());
                }
                for (; child < _children.Count(); ++child)
                {
                    var type = NodeType.Middle;
                    if (child == _children.Count() - 1)
                    {
                        if (this.IsRoot() && _children.Count() == 1) type = NodeType.SingleRoot;
                        else type = NodeType.Last;
                    }
                    _children[child].ToMultilineString(nextDepth, lines, depths, type);
                }
            }

            private static bool IsBranchAtThisDepth(int depth, int lineNum, IList<Tuple<int, ParentDirection>> depths)
            {
                Tuple<int, ParentDirection> above = null;
                for (var i = lineNum - 1; i >= 0; --i)
                {
                    if (depths[i].Item1 > depth) continue;
                    if (depths[i].Item1 < (depth - 1)) return false;
                    above = depths[i];
                    break;
                }
                Tuple<int, ParentDirection> below = null;
                for (var i = lineNum + 1; i < depths.Count; ++i)
                {
                    if (depths[i].Item1 > depth) continue;
                    if (depths[i].Item1 < (depth - 1)) return false;
                    below = depths[i];
                    break;
                }
                if (above == null || below == null) return false;
                return (((above.Item1 == depth && above.Item2 != ParentDirection.Up) || above.Item1 == depth - 1)
                        && ((below.Item1 == depth && below.Item2 != ParentDirection.Down) || below.Item1 == depth - 1))
                       || (above.Item1 == depth && below.Item1 == depth && above.Item2 == below.Item2);
            }
        }
    }
}