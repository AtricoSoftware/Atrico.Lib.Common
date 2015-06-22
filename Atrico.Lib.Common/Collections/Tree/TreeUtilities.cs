using System.Collections.Generic;
using System.Linq;

namespace Atrico.Lib.Common.Collections.Tree
{
    /// <summary>
    ///     Tree utilities
    /// </summary>
    public static class TreeUtilities
    {
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
            var node = container as ITreeNode;
            var nodeData = node != null ? new[] {node.Data} : new object[] {};
            var leaves = new List<IEnumerable<object>>();
            foreach (var childLeaves in container.Children.Select(child => GetNodesImpl(child, includeNonTerminal)))
            {
                leaves.AddRange(childLeaves.Select(nodeData.Concat));
            }
            if (node != null && (includeNonTerminal || !leaves.Any())) leaves.Add(new[] {node.Data});
            return leaves;
        }
    }
}