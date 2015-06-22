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

        protected readonly bool AllowDuplicateNodes;
        protected readonly IList<Node> Children;

        IEnumerable<ITreeNode> ITreeNodeContainer.Children
        {
            get { return Children; }
        }

        #region Construction

        public NodeContainer(bool allowDuplicateNodes, IEnumerable<Node> children = null)
        {
            AllowDuplicateNodes = allowDuplicateNodes;
            Children = new List<Node>(children ?? new Node[] {});
        }

        #endregion

        #region Modify

        public ITreeNode Add(object data)
        {
            if (!AllowDuplicateNodes)
            {
                var existing = Children.FirstOrDefault(n => n.Data.Equals(data));
                if (existing != null) return existing;
            }
            var node = new Node(AllowDuplicateNodes, data, this);
            Children.Add(node);
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
            var found = Children.FirstOrDefault(n => n.Data.Equals(data));
            if (found != null) Children.Remove(found);
        }

        #endregion

        #region Traversal

        public void DepthFirst(Action<ITreeNode> action)
        {
            var remaining = new Stack<ITreeNode>();
            GetNodes().Reverse().ForEach(remaining.Push);
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
            GetNodes().ForEach(remaining.Enqueue);
            while (remaining.Any())
            {
                var container = remaining.Dequeue();
                var node = container as ITreeNode;
                if (node != null) action(node);
                container.Children.ForEach(remaining.Enqueue);
            }
        }

        protected virtual IEnumerable<ITreeNode> GetNodes()
        {
            // Add children
            return Children;
        }
        #endregion

        public void ReplaceNode(Node oldNode, Node newNode)
        {
            for (var i = 0; i < Children.Count; ++i)
            {
                if (!ReferenceEquals(Children[i], oldNode)) continue;
                Children[i] = newNode;
                break;
            }
        }

        #region Equality

        protected override int GetHashCodeImpl()
        {
            return Children.GetHashCode();
        }

        protected override bool EqualsImpl(NodeContainer other)
        {
            return Children.SequenceEqual(other.Children);
        }

        #endregion
    }
}