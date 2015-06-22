using System.Collections.Generic;
using System.Linq;

namespace Atrico.Lib.Common.Collections.Tree.Implementation
{
    internal class Node : NodeContainer, ITreeNode
    {
        private readonly NodeContainer _parent;

        public ITreeNodeContainer Parent
        {
            get { return _parent; }
        }

        public object Data { get; set; }

        public Node(bool allowDuplicateNodes, object data, NodeContainer parent, IEnumerable<Node> children = null)
            : base(allowDuplicateNodes, children)
        {
            _parent = parent;
            Data = data;
        }

        #region Clone

        public ITreeNode Clone(bool deep)
        {
            return CloneNode(newChildren: (deep ? null : _children));
        }

        private Node CloneNode(Node newParent = null, IEnumerable<Node> newChildren = null)
        {
            return CloneNode(Data, newParent, newChildren);
        }

        private Node CloneNode(object newData, Node newParent = null, IEnumerable<Node> newChildren = null)
        {
            return new Node(_allowDuplicateNodes, newData, newParent ?? _parent, newChildren ?? _children.Select(ch => ch.CloneNode()));
        }

        #endregion

        #region Equality

        protected override bool EqualsImpl(NodeContainer obj)
        {
            var other = obj as Node;
            if (other == null) return false;
            var dataMatch = ReferenceEquals(Data, null) ? ReferenceEquals(other.Data, null) : Data.Equals(other.Data);
            return dataMatch && base.EqualsImpl(obj);
        }

        #endregion

        public ITreeNode Insert(object data)
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

        public override string ToString()
        {
            return string.Format("{0}:{1}", this.IsRoot() ? "" : Data.ToString(), _children.ToCollectionString());
        }
    }
}