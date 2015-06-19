using System;
using System.Collections.Generic;

namespace Atrico.Lib.Common.Collections.Tree
{
    /// <summary>
    ///     Interface to node
    /// </summary>
    public interface ITreeNodeContainer : IEquatable<object>, IMultilineDisplayable
    {
        /// <summary>
        /// Gets the children
        /// </summary>
        IEnumerable<ITreeNode> Children { get; }

        /// <summary>
        /// Clones this instance and makes modifiable
        /// </summary>
        /// <param name="deep">If true, clone children</param>
        /// <returns>Copy of node</returns>
        IModifiableTreeNode Clone(bool deep = false);

        /// <summary>
        ///     Perform action on each node in depth first order
        /// </summary>
        /// <param name="action">The action to perform</param>
        void DepthFirst(Action<ITreeNode> action);

        /// <summary>
        ///     Perform action on each node in breadth first order
        /// </summary>
        /// <param name="action">The action to perform</param>
        void BreadthFirst(Action<ITreeNode> action);

        /// <summary>
        /// Transforms this node and all children (children first)
        /// </summary>
        /// <param name="transform">The transform</param>
        /// <returns>New node</returns>
        ITreeNode Transform(Func<IModifiableTreeNode, ITreeNode> transform);
    }
}