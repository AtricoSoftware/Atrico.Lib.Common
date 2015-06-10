﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atrico.Lib.Common.Collections.Tree
{
    public partial class Tree<T>
    {
        internal const char FirstChildNode = '\u250C'; // '/'
        internal const char MidChildNode = '\u251c'; // '|-'
        internal const char LastChildNode = '\u2514'; // '\'
        internal const char SingleRoot = '\u2500'; // '-'
        internal const char FirstOfDoubleRoot = '\u252C'; // '¬-'
        internal const char MidRoot = '\u253C'; // '+'
        internal const char Dash = '\u2500'; // '-'
        internal const char VerticalLine = '\u2502'; // '|'
        internal const char Space = ' '; // ' '

        private class Node : INode<T>
        {
            
            private readonly Node _parent;
            private readonly T _data;
            private readonly bool _allowDuplicateNodes;
            private readonly IList<Node> _children = new List<Node>();
            private bool _isRootNode { get { return _parent == null; } }

            public T Data
            {
                get { return _data; }
            }

            public INode<T> Add(T data)
            {
                var node = _allowDuplicateNodes ? null : _children.FirstOrDefault(n => n.Equals(data));
                if (node != null) return node;
                node = new Node(data, this, _allowDuplicateNodes);
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

            protected Node(bool allowDuplicateNodes)
                : this(default(T), null, allowDuplicateNodes)
            {
            }

            private Node(T data, Node parent, bool allowDuplicateNodes)
            {
                _data = data;
                _parent = parent;
                _allowDuplicateNodes = allowDuplicateNodes;
            }

            public bool Equals(T other)
            {
                return _data.Equals(other);
            }

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
                var nextDepth = _isRootNode ? depth : depth + 1;
                var child = 0;
                for (; child < _children.Count() / 2; ++child)
                {
                    _children[child].GetDepths(nextDepth, depths, _isRootNode ? ParentDirection.None : ParentDirection.Down);
                }
                if (!_isRootNode) depths.Add(Tuple.Create(depth, parentDirection));
                for (; child < _children.Count(); ++child)
                {
                    _children[child].GetDepths(nextDepth, depths, _isRootNode ? ParentDirection.None : ParentDirection.Up);
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
                var nextDepth = _isRootNode ? depth : depth + 1;
                var child = 0;
                for (; child < _children.Count() / 2; ++child)
                {
                    var type = NodeType.Middle;
                    if (child == 0)
                    {
                        if (_isRootNode && _children.Count() == 2) type = NodeType.FirstOfDoubleRoot;
                        else type = NodeType.First;
                    }
                    _children[child].ToMultilineString(nextDepth, lines, depths, type);
                }
                if (!_isRootNode)
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
                        if (_isRootNode && _children.Count() == 1) type = NodeType.SingleRoot;
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

            public override string ToString()
            {
                return string.Format("{0}:{1}", _isRootNode ? "" : _data.ToString(), _children.ToCollectionString());
            }
        }

        private class RootNode : Node
        {
            public RootNode(bool allowDuplicateNodes)
                : base(allowDuplicateNodes)
            {
            }

         }
    }
}