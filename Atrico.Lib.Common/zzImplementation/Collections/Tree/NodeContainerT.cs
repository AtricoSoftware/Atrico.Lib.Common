using System;
using System.Collections.Generic;
using System.Linq;
using Atrico.Lib.Common.Collections.Tree;

namespace Atrico.Lib.Common.zzImplementation.Collections.Tree
{
    internal class NodeContainer<T> : EquatableObject<NodeContainer<T>>, ITreeNodeContainer<T>
    {
        private readonly ITreeNodeContainer _implementation;

        public NodeContainer(ITreeNodeContainer implementation)
        {
            _implementation = implementation;
        }

        protected override int GetHashCodeImpl()
        {
            return _implementation.GetHashCode();
        }

        protected override bool EqualsImpl(NodeContainer<T> other)
        {
            return _implementation.Equals(other._implementation);
        }

        public IEnumerable<string> ToMultilineString()
        {
            return _implementation.ToMultilineString();
        }

        public IEnumerable<ITreeNode<T>> Children
        {
            get { return _implementation.Children.Select(ch => new Node<T>(ch)); }
        }

        public void DepthFirst(Action<ITreeNode<T>> action)
        {
            _implementation.DepthFirst(nd => action(new Node<T>(nd)));
        }

        public void BreadthFirst(Action<ITreeNode<T>> action)
        {
            _implementation.BreadthFirst(nd => action(new Node<T>(nd)));
        }

        public IEnumerable<IEnumerable<T>> GetNodes()
        {
            return _implementation.GetNodes().Select(lst => lst.Cast<T>());
        }

        public IEnumerable<IEnumerable<T>> GetLeaves()
        {
            return _implementation.GetLeaves().Select(lst => lst.Cast<T>());
        }

        public ITreeNode<T> Add(T data)
        {
            return new Node<T>(_implementation.Add(data));
        }

        public ITreeNode<T> AddPath(IEnumerable<T> path)
        {
            return new Node<T>(_implementation.AddPath(path.Cast<object>()));
        }

        public void Remove(T data)
        {
            _implementation.Remove(data);
        }
        public override string ToString()
        {
            return _implementation.ToString();
        }

    }
}
