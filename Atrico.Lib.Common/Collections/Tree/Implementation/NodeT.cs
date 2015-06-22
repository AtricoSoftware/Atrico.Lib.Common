using System;
using System.Collections.Generic;
using System.Linq;

namespace Atrico.Lib.Common.Collections.Tree.Implementation
{
    internal class Node<T> : ITreeNode<T>
    {
        private readonly ITreeNode _implementation;

        public Node(ITreeNode implementation)
        {
            _implementation = implementation;
        }

        public T Data
        {
            get { return (T) _implementation.Data; }
            set { _implementation.Data = value; }
        }

        public ITreeNodeContainer<T> Parent
        {
            get { return new NodeContainer<T>(_implementation.Parent); }
        }

        public ITreeNode<T> Clone(bool deep = false)
        {
            return new Node<T>(_implementation.Clone(deep));
        }

        public ITreeNode<T> Insert(T data)
        {
            return new Node<T>(_implementation.Insert(data));
        }

        public IEnumerable<string> ToMultilineString()
        {
            return _implementation.ToMultilineString();
        }

        public bool Equals(T other)
        {
            return _implementation.Equals(other);
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

        public override bool Equals(object obj)
        {
            var other = obj as Node<T>;
            if (other == null) return false;
            return _implementation.Equals(other._implementation);
        }

        public override int GetHashCode()
        {
            return _implementation.GetHashCode();
        }

        public override string ToString()
        {
            return _implementation.ToString();
        }
    }
}
