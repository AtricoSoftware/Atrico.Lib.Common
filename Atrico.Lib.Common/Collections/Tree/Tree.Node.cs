using System;
using System.Collections.Generic;
using System.Linq;
using Atrico.Lib.DomainModel;

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

        private partial class Node : ValueObject<Node>, IModifiableNode
        {
            private readonly bool _allowDuplicateNodes;
            private readonly Node _parent;
            private readonly T _data;
            private readonly IList<Node> _children;

            public T Data
            {
                get { return _data; }
            }

            public IModifiableNode Parent
            {
                get { return _parent; }
            }

            public IEnumerable<INode> Children
            {
                get { return _children; }
            }

            #region Construction

            internal Node(bool allowDuplicateNodes)
                : this(allowDuplicateNodes, default(T), null)
            {
            }

            private Node(bool allowDuplicateNodes, T data, Node parent, IEnumerable<Node> children = null)
            {
                _allowDuplicateNodes = allowDuplicateNodes;
                _data = data;
                _parent = parent;
                _children = new List<Node>(children ?? new Node[] {});
            }

            public IModifiableNode Clone()
            {
                return CloneNode();
            }

            private Node CloneNode(Node newParent = null, IEnumerable<Node> newChildren = null)
            {
                return CloneNode(_data, newParent, newChildren);
            }

            private Node CloneNode(T newData, Node newParent = null, IEnumerable<Node> newChildren = null)
            {
                return new Node(_allowDuplicateNodes, newData, newParent ?? _parent, newChildren ?? _children.Select(ch => ch.CloneNode()));
            }

            #endregion

            #region Modify

            public IModifiableNode Add(T data)
            {
                if (!_allowDuplicateNodes)
                {
                    var existing = _children.FirstOrDefault(n => n.Equals(data));
                    if (existing != null) return existing;
                }
                var node = CloneNode(data, this, new Node[] {});
                _children.Add(node);
                return node;
            }

            public IModifiableNode Add(IEnumerable<T> path)
            {
                var pathArray = path as T[] ?? path.ToArray();
                if (pathArray.Length == 0) return this;
                var node = Add(pathArray[0]);
                return node.Add(pathArray.Skip(1));
            }

            public IModifiableNode Insert(T data)
            {
                if (this.IsRoot())
                {
                    var node = CloneNode(data, this);
                    _children.Clear();
                    _children.Add(node);
                    return this;
                }
                var newNode = CloneNode(data, newChildren: new[] {this});
                _parent.ReplaceNode(this, newNode);
                return newNode;
            }

            #endregion

            #region Traversal

            public void DepthFirst(Action<INode> action)
            {
                var remaining = new Stack<INode>();
                if (this.IsRoot()) _children.Reverse().ForEach(remaining.Push);
                else remaining.Push(this);
                while (remaining.Any())
                {
                    var node = remaining.Pop();
                    action(node);
                    node.Children.Reverse().ForEach(remaining.Push);
                }
            }

            public void BreadthFirst(Action<INode> action)
            {
                var remaining = new Queue<INode>();
                if (this.IsRoot()) _children.ForEach(remaining.Enqueue);
                else remaining.Enqueue(this);
                while (remaining.Any())
                {
                    var node = remaining.Dequeue();
                    action(node);
                    node.Children.ForEach(remaining.Enqueue);
                }
            }

            #endregion

            private void ReplaceNode(Node oldNode, Node newNode)
            {
                for (var i = 0; i < _children.Count; ++i)
                {
                    if (!ReferenceEquals(_children[i], oldNode)) continue;
                    _children[i] = newNode;
                    break;
                }
            }

            #region Equality

            protected override int GetHashCodeImpl()
            {
                return _data.GetHashCode() ^ _children.GetHashCode();
            }

            protected override bool EqualsImpl(Node other)
            {
                return Equals(other._data) && _children.SequenceEqual(other._children);
            }

            public bool Equals(T otherData)
            {
                return _data.Equals(otherData);
            }

            #endregion

            public override string ToString()
            {
                return string.Format("{0}:{1}", this.IsRoot() ? "" : _data.ToString(), _children.ToCollectionString());
            }
        }
    }
}