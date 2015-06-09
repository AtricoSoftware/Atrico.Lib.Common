using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atrico.Lib.Common.Collections.Tree
{
    public partial class Tree<T>
    {
        private class Node : INode<T>
        {
            private readonly T _data;
            private readonly IList<Node> _children = new List<Node>();

            public T Data
            {
                get { return _data; }
            }

            public INode<T> Add(T data)
            {
                var node = _children.FirstOrDefault(n => n.Equals(data));
                if (node != null) return node;
                node = new Node(data);
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

            protected Node(T data)
            {
                _data = data;
            }

            public bool Equals(T other)
            {
                return _data.Equals(other);
            }

            public IEnumerable<string> ToMultilineString()
            {
                var lines = new List<string>();
                ToMultilineString(new bool[]{}, lines);
                return lines;
            }

            private void ToMultilineString(IEnumerable<bool> branches, ICollection<string> lines)
            {
                var branchesA = branches as bool[] ?? branches.ToArray();
                IEnumerable<bool> newBranches;
                if (!string.IsNullOrEmpty(MultilineValue))
                {
                    var line = new StringBuilder();
                    foreach (var branch in branchesA)
                    {
                        line.Append(branch ? "| " : "  ");
                    }
                    line.Append("+-");
                    line.Append(MultilineValue);
                    lines.Add(line.ToString());
                    newBranches = branchesA.Concat(new[] {false});
                }
                else newBranches = branchesA;
                foreach (var child in _children)
                {
                    child.ToMultilineString(newBranches, lines);
                }
            }

            protected virtual string MultilineValue
            {
                get { return _data.ToString(); }
            }
        }

        private class RootNode : Node
        {
            public RootNode()
                : base(default(T))
            {
            }

            protected override string MultilineValue
            {
                get { return null; }
            }
        }
    }
}