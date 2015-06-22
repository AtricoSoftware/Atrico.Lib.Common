using System;
using System.Collections.Generic;
using System.Linq;
using Atrico.Lib.Common.Collections.Tree.Implementation;

namespace Atrico.Lib.Common.Collections.Tree
{
    /// <summary>
    ///     Tree utilities
    /// </summary>
    public static class TreeUtilities
    {
        /// <summary>
        ///     Determines whether this node is root
        /// </summary>
        /// <param name="node">The node</param>
        /// <returns>True if node is root of tree</returns>
        public static bool IsRoot(this ITreeNode node)
        {
            return node.Parent == null;
        }
        public static bool IsRoot(this ITreeNodeContainer container)
        {
            var node = container as ITreeNode;
            return node == null || IsRoot(node);
        }
       /// <summary>
        ///     Determines whether this node is leaf (terminal node)
        /// </summary>
        /// <param name="node">The node</param>
        /// <returns>True if node is terminal (has no children)</returns>
        public static bool IsLeaf(this ITreeNode node)
        {
            return !node.Children.Any();
        }

        /// <summary>
        ///     Get all the nodes as paths
        /// </summary>
        /// <param name="container">The root of tree</param>
        /// <returns>List of paths (list of data)</returns>
        public static IEnumerable<IEnumerable<object>> GetNodes(this ITreeNodeContainer container)
        {
            return GetNodesImpl(container, true);
        }

        /// <summary>
        ///     Get all the leaves (terminal nodes) as paths
        /// </summary>
        /// <param name="container">The root of tree</param>
        /// <returns>List of paths (list of data)</returns>
        public static IEnumerable<IEnumerable<object>> GetLeaves(this ITreeNodeContainer container)
        {
            return GetNodesImpl(container, false);
        }
 
 
        private static IEnumerable<IEnumerable<object>> GetNodesImpl(ITreeNodeContainer container, bool includeNonTerminal)
        {
            var leaves = new List<IEnumerable<object>>();
            foreach (var childLeaves in container.Children.Select(child => GetNodesImpl(child, includeNonTerminal)))
            {
                leaves.AddRange(childLeaves.Select(leaf => container.IsRoot() ? leaf : new[] { (container as ITreeNode).Data }.Concat(leaf)));
            }
            var node = container as ITreeNode;
            if (node != null && !node.IsRoot() && (includeNonTerminal || !leaves.Any())) leaves.Add(new[] {node.Data});
            return leaves;
        }
    }
}