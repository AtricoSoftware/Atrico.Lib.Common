using System.Collections.Generic;

namespace Atrico.Lib.Common.Collections.Tree
{
    /// <summary>
    ///     Interface to a container of nodes
    /// </summary>
    public interface INodeContainer<T> : IMultilineDisplayable
    {
        INode<T> Add(T data);
        INode<T> Add(IEnumerable<T> path);
    }
}