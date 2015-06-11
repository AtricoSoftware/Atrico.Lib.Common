using System;
using System.Collections.Generic;

namespace Atrico.Lib.Common.Collections.Tree
{
    public partial class Tree<T>
    {
        /// <summary>
        ///     Interface to node
        /// </summary>
        public interface INode : IEquatable<T>, IMultilineDisplayable
        {
            T Data { get; }
            INode Parent { get; }
            IEnumerable<INode> Children { get; }

            INode Add(T data);
            INode Add(IEnumerable<T> path);
            INode Insert(T data);

            /// <summary>
            ///     Perform action on each node in depth first order
            /// </summary>
            /// <param name="action">The action to perform</param>
            void DepthFirst(Action<INode> action);

            /// <summary>
            ///     Perform action on each node in breadth first order
            /// </summary>
            /// <param name="action">The action to perform</param>
            void BreadthFirst(Action<INode> action);
        }
    }
}