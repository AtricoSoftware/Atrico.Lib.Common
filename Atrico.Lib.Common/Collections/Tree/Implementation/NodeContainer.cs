using System;
using System.Collections.Generic;
using System.Linq;

namespace Atrico.Lib.Common.Collections.Tree.Implementation
{
    internal partial class NodeContainer : EquatableObject<NodeContainer>, ITreeNodeContainer
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

        protected readonly bool _allowDuplicateNodes;
        protected readonly IList<Node> _children;

        public IEnumerable<ITreeNode> Children
        {
            get { return _children; }
        }

        #region Construction

        public NodeContainer(bool allowDuplicateNodes, IEnumerable<Node> children = null)
        {
            _allowDuplicateNodes = allowDuplicateNodes;
            _children = new List<Node>(children ?? new Node[] {});
        }

        #endregion

        #region Modify

        public ITreeNode Add(object data)
        {
            if (!_allowDuplicateNodes)
            {
                var existing = _children.FirstOrDefault(n => n.Data.Equals(data));
                if (existing != null) return existing;
            }
            var node = new Node(_allowDuplicateNodes, data, this);
            _children.Add(node);
            return node;
        }

        public ITreeNode Add(IEnumerable<object> path)
        {
            var pathArray = path as object[] ?? path.ToArray();
            if (pathArray.Length == 0) return this as ITreeNode;
            var node = Add(pathArray[0]);
            return node.Add(pathArray.Skip(1));
        }

        public void Remove(object data)
        {
            var found = _children.FirstOrDefault(n => n.Data.Equals(data));
            if (found != null) _children.Remove(found);
        }

        #endregion

        #region Traversal

        public void DepthFirst(Action<ITreeNode> action)
        {
            var remaining = new Stack<ITreeNodeContainer>();
            if (this.IsRoot()) _children.Reverse().ForEach(remaining.Push);
            else remaining.Push(this);
            while (remaining.Any())
            {
                var container = remaining.Pop();
                var node = container as ITreeNode;
                if (node != null) action(node);
                container.Children.Reverse().ForEach(remaining.Push);
            }
        }

        public void BreadthFirst(Action<ITreeNode> action)
        {
            var remaining = new Queue<ITreeNodeContainer>();
            if (this.IsRoot()) _children.ForEach(remaining.Enqueue);
            else remaining.Enqueue(this);
            while (remaining.Any())
            {
                var container = remaining.Dequeue();
                var node = container as ITreeNode;
                if (node != null) action(node);
                container.Children.ForEach(remaining.Enqueue);
            }
        }

        #endregion

        public void ReplaceNode(Node oldNode, Node newNode)
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
            return _children.GetHashCode();
        }

        protected override bool EqualsImpl(NodeContainer other)
        {
            return _children.SequenceEqual(other._children);
        }

        #endregion
    }
}