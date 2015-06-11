using System.Collections.Generic;
using System.Linq;

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

        private abstract partial class Node : INode
        {
            private readonly Node _parent;
            private readonly T _data;
            private readonly IList<Node> _children = new List<Node>();

            public T Data
            {
                get { return _data; }
            }

            public INode Parent
            {
                get { return _parent; }
            }

            public IEnumerable<INode> Children
            {
                get { return _children; }
            }

            public INode Add(T data)
            {
                return AddImpl(data, _children);
            }

            public INode Add(IEnumerable<T> path)
            {
                var pathArray = path as T[] ?? path.ToArray();
                if (pathArray.Length == 0) return this;
                var node = Add(pathArray[0]);
                return node.Add(pathArray.Skip(1));
            }

            public INode Insert(T data)
            {
                if (this.IsRoot())
                {
                    if (!_children.Any())
                    {
                        Add(data);
                    }
                    else
                    {
                        var node = CreateNewNode(data, this, _children);
                        _children.Clear();
                        _children.Add(node);
                    }
                    return this;
                }
                var newNode = CreateNewNode(data, _parent, new []{this});
                _parent.ReplaceNode(this, newNode);
                return newNode;
            }

            private void ReplaceNode(Node oldNode, Node newNode)
            {
                for (var i = 0; i < _children.Count; ++i)
                {
                    if (!ReferenceEquals(_children[i], oldNode)) continue;
                    _children[i] = newNode;
                    break;
                }
            }

            protected abstract Node CreateNewNode(T data, Node parent, IEnumerable<Node> children);

            internal static INode CreateNode(bool allowDuplicateNodes)
            {
                return allowDuplicateNodes ? new NodeAllowDuplicates() : new NodeMergeDuplicates() as Node;
            }

            protected Node(IEnumerable<Node> children = null)
                : this(default(T), null, children)
            {
            }

            private Node(T data, Node parent, IEnumerable<Node> children = null)
            {
                _data = data;
                _parent = parent;
                if (children != null)
                {
                    _children = new List<Node>(children);
                }
            }

            public bool Equals(T other)
            {
                return _data.Equals(other);
            }

            protected abstract INode AddImpl(T data, IList<Node> children);

            public override string ToString()
            {
                return string.Format("{0}:{1}", this.IsRoot() ? "" : _data.ToString(), _children.ToCollectionString());
            }

            private class NodeAllowDuplicates : Node
            {
                public NodeAllowDuplicates()
                {
                }

                private NodeAllowDuplicates(T data, Node parent, IEnumerable<Node> children = null)
                    : base(data, parent, children)
                {
                }

                protected override Node CreateNewNode(T data, Node parent, IEnumerable<Node> children)
                {
                    return new NodeAllowDuplicates(data, parent, children);
                }

                protected override INode AddImpl(T data, IList<Node> children)
                {
                    var node = new NodeAllowDuplicates(data, this);
                    children.Add(node);
                    return node;
                }
            }

            private class NodeMergeDuplicates : Node
            {
                public NodeMergeDuplicates()
                {
                }

                private NodeMergeDuplicates(T data, Node parent, IEnumerable<Node> children = null)
                    : base(data, parent, children)
                {
                }

                protected override Node CreateNewNode(T data, Node parent, IEnumerable<Node> children)
                {
                    return new NodeMergeDuplicates(data, parent, children);
                }

                protected override INode AddImpl(T data, IList<Node> children)
                {
                    var node = children.FirstOrDefault(n => n.Equals(data));
                    if (node != null) return node;
                    node = new NodeMergeDuplicates(data, this);
                    children.Add(node);
                    return node;
                }
            }
        }
    }
}