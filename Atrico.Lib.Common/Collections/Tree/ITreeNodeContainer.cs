using System;
using System.Collections.Generic;

namespace Atrico.Lib.Common.Collections.Tree
{
    /// <summary>
    ///     Interface to typed node
    /// </summary>
    public interface ITreeNodeContainer : IEquatable<object>, IMultilineDisplayable
    {
        /// <summary>
        ///     Gets the children
        /// </summary>
        IEnumerable<ITreeNode> Children { get; }

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
        ///     Get all the nodes as paths
        /// </summary>
        /// <returns>List of paths (list of data)</returns>
        IEnumerable<IEnumerable<object>> GetNodes();

        /// <summary>
        ///     Get all the leaves (terminal nodes) as paths
        /// </summary>
        /// <returns>List of paths (list of data)</returns>
        IEnumerable<IEnumerable<object>> GetLeaves();

        /// <summary>
        ///     Add a child node with the specified data
        /// </summary>
        /// <param name="data">The node data</param>
        /// <returns>New node</returns>
        ITreeNode Add(object data);

        /// <summary>
        ///     Adds the specified path of nodes
        /// </summary>
        /// <param name="path">The path</param>
        /// <returns>New leaf node</returns>
        ITreeNode Add(IEnumerable<object> path);

        /// <summary>
        ///     Removes the specified child node
        /// </summary>
        /// <param name="data">The data by which to identify the node</param>
        void Remove(object data);
    }
}
