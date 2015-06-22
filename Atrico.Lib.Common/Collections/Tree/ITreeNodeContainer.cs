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
        ITreeNode Clone(bool deep = false);

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
        ITreeNode Transform(Func<ITreeNode, ITreeNode> transform);

        /// <summary>
        /// Add a child node with the specified data
        /// </summary>
        /// <param name="data">The node data</param>
        /// <returns>New node</returns>
        ITreeNode Add(object data);

        /// <summary>
        /// Adds the specified path of nodes
        /// </summary>
        /// <param name="path">The path</param>
        /// <returns>New leaf node</returns>
        ITreeNode Add(IEnumerable<object> path);

        /// <summary>
        /// Inserts a node with the specified data
        /// </summary>
        /// <param name="data">Node data</param>
        /// <returns>New node</returns>
        ITreeNode Insert(object data);

        /// <summary>
        /// Removes the specified child node
        /// </summary>
        /// <param name="data">The data by which to identify the node</param>
        void Remove(object data);
    }
}