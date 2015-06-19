using System;
using System.Collections.Generic;

namespace Atrico.Lib.Common.Collections.Tree
{
    public partial class TreeT<T>
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
            /// <param name="deep">If true, clone children</param>
            /// <returns>Copy of node</returns>
            IModifiableNode Clone(bool deep = false);

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

            /// <summary>
            /// Transforms this node and all children (children first)
            /// </summary>
            /// <param name="transform">The transform</param>
            /// <returns>New node</returns>
            INode Transform(Func<IModifiableNode, INode> transform);
        }

        /// <summary>
        ///     Interface to node that can be modified
        /// </summary>
        public interface IModifiableNode : INode
        {
            new T Data { get; set; }
            IModifiableNode Add(T data);
            IModifiableNode Add(IEnumerable<T> path);
            IModifiableNode Insert(T data);
            void Remove(T data);
        }
    }
}