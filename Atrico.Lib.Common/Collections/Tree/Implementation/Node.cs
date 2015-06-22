using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atrico.Lib.Common.Collections.Tree.Implementation
{
    internal sealed class Node : NodeContainer, ITreeNode
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
            return CloneNode(newChildren: (deep ? null : Children));
        }

        private Node CloneNode(NodeContainer newParent = null, IEnumerable<Node> newChildren = null)
        {
            return CloneNode(Data, newParent, newChildren);
        }

        private Node CloneNode(object newData, NodeContainer newParent = null, IEnumerable<Node> newChildren = null)
        {
            return new Node(AllowDuplicateNodes, newData, newParent ?? _parent, newChildren ?? Children.Select(ch => ch.CloneNode()));
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

        #region Traversal

        protected override IEnumerable<ITreeNode> GetNodes()
        {
            return new[] {this};
        }

        #endregion

        public ITreeNode Insert(object data)
        {
            var newNode = CloneNode(data, newChildren: new[] {this});
            _parent.ReplaceNode(this, newNode);
            return newNode;
        }

        #region ToMultiline

        protected override void GetDepths(int depth, ICollection<Tuple<int, ParentDirection>> depths, ParentDirection parentDirection)
        {
            var child = 0;
            for (; child < Children.Count() / 2; ++child)
            {
                Children[child].GetDepths(depth + 1, depths, ParentDirection.Down);
            }
            depths.Add(Tuple.Create(depth, parentDirection));
            for (; child < Children.Count(); ++child)
            {
                Children[child].GetDepths(depth + 1, depths, ParentDirection.Up);
            }
        }
        protected override void ToMultilineString(int depth, ICollection<string> lines, IList<Tuple<int, ParentDirection>> depths, NodeType nodeType)
        {
            var lineNum = lines.Count();
            var child = 0;
            for (; child < Children.Count() / 2; ++child)
            {
                var type = NodeType.Middle;
                if (child == 0) type = NodeType.First;
                Children[child].ToMultilineString(depth + 1, lines, depths, type);
            }
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
                line.Append(Data);
                lines.Add(line.ToString());
            
            for (; child < Children.Count(); ++child)
            {
                var type = NodeType.Middle;
                if (child == Children.Count() - 1) type = NodeType.Last;
                Children[child].ToMultilineString(depth + 1, lines, depths, type);
            }
        }

        #endregion

        public override string ToString()
        {
            return string.Format("{0}:{1}", Data, Children.ToCollectionString());
        }
    }
}