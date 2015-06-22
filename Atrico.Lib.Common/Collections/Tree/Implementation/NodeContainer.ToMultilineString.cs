using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atrico.Lib.Common.Collections.Tree.Implementation
{
    internal partial class NodeContainer
    {
        public IEnumerable<string> ToMultilineString()
        {
            var lines = new List<string>();
            var depths = new List<Tuple<int, ParentDirection>>();
            GetDepths(0, depths, ParentDirection.None);
            ToMultilineString(0, lines, depths, NodeType.Middle);
            return lines;
        }

        protected virtual void GetDepths(int depth, ICollection<Tuple<int, ParentDirection>> depths, ParentDirection parentDirection)
        {
            foreach (var child in Children)
            {
                child.GetDepths(depth, depths, ParentDirection.None);
            }
        }

        protected enum NodeType
        {
            First,
            Middle,
            Last,
            SingleRoot,
            FirstOfDoubleRoot
        }

        protected enum ParentDirection
        {
            None,
            Up,
            Down
        }

        protected virtual void ToMultilineString(int depth, ICollection<string> lines, IList<Tuple<int, ParentDirection>> depths, NodeType nodeType)
        {
            for (var child = 0; child < Children.Count(); ++child)
            {
                var type = NodeType.Middle;
                if (Children.Count() == 1)
                {
                    type = NodeType.SingleRoot;
                }
                else if (child == Children.Count() - 1)
                {
                    type = NodeType.Last;
                }
                else if (child == 0)
                {
                    type = Children.Count() == 2 ? NodeType.FirstOfDoubleRoot : NodeType.First;
                }
                 Children[child].ToMultilineString(depth, lines, depths, type);
            }
        }

        protected static bool IsBranchAtThisDepth(int depth, int lineNum, IList<Tuple<int, ParentDirection>> depths)
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