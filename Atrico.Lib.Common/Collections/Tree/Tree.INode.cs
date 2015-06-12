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
            IModifiableNode Parent { get; }
            IEnumerable<INode> Children { get; }

            /// <summary>
            /// Clones this instance and makes modifiable
            /// </summary>
            /// <returns>Copy of node</returns>
            IModifiableNode Clone();

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

        /// <summary>
        ///     Interface to node that can be modified
        /// </summary>
        public interface IModifiableNode : INode
        {
            IModifiableNode Add(T data);
            IModifiableNode Add(IEnumerable<T> path);
            IModifiableNode Insert(T data);
        }
    }
}