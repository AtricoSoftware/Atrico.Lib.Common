using System;

namespace Atrico.Lib.Common.Collections.Tree
{
    /// <summary>
    ///     Interface to node
    /// </summary>
    public interface INode<T> : INodeContainer<T>, IEquatable<T>
    {
        T Data { get; }
    }
}