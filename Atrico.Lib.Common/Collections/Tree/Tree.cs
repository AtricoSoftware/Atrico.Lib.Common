using System.Collections.Generic;

namespace Atrico.Lib.Common.Collections.Tree
{
    /// <summary>
    ///     Tree of nodes
    /// </summary>
    public partial class Tree<T> : INodeContainer<T>
    {
        private readonly RootNode _rootNode;

        public Tree(bool allowDuplicateNodes)
        {
            _rootNode = new RootNode(allowDuplicateNodes);
        }

        public INode<T> Add(T data)
        {
            return _rootNode.Add(data);
        }

        public INode<T> Add(IEnumerable<T> path)
        {
            return _rootNode.Add(path);
        }

        public IEnumerable<string> ToMultilineString()
        {
            return _rootNode.ToMultilineString();
        }
    }
}