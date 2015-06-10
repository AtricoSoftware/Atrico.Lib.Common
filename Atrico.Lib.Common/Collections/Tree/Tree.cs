using System.Collections.Generic;
using System.Linq;

namespace Atrico.Lib.Common.Collections.Tree
{
    /// <summary>
    ///     Tree utilities
    /// </summary>
    public static class Tree
    {
        /// <summary>
        ///     Determines whether this instance is root
        /// </summary>
        /// <typeparam name="T">Type of node data</typeparam>
        /// <param name="node">The node.</param>
        /// <returns>True if node is root of tree</returns>
        public static bool IsRoot<T>(this Tree<T>.INode node)
        {
            return node.Parent == null;
        }

        /// <summary>
        ///     Get all the nodes as paths
        /// </summary>
        /// <typeparam name="T">Type of node data</typeparam>
        /// <param name="node">The root of tree</param>
        /// <returns>List of paths (list of data)</returns>
        public static IEnumerable<IEnumerable<T>> GetNodes<T>(this Tree<T>.INode node)
        {
            return GetNodesImpl(node, true);
        }

        /// <summary>
        ///     Get all the leaves (terminal nodes) as paths
        /// </summary>
        /// <typeparam name="T">Type of node data</typeparam>
        /// <param name="node">The root of tree</param>
        /// <returns>List of paths (list of data)</returns>
        public static IEnumerable<IEnumerable<T>> GetLeaves<T>(this Tree<T>.INode node)
        {
            return GetNodesImpl(node, false);
        }

        private static IEnumerable<IEnumerable<T>> GetNodesImpl<T>(Tree<T>.INode node, bool includeNonTerminal)
        {
            var leaves = new List<IEnumerable<T>>();
            foreach (var childLeaves in node.Children.Select(child => GetNodesImpl(child, includeNonTerminal)))
            {
                leaves.AddRange(childLeaves.Select(leaf => node.IsRoot() ? leaf : new[] {node.Data}.Concat(leaf)));
            }
            if (!node.IsRoot() && (includeNonTerminal || !leaves.Any())) leaves.Add(new[] {node.Data});
            return leaves;
        }
    }

    /// <summary>
    ///     Typed tree utilities
    /// </summary>
    public static partial class Tree<T>
    {
        /// <summary>
        ///     Creates the tree
        /// </summary>
        /// <param name="allowDuplicateNodes">if set to <c>true</c> allow duplicate nodes</param>
        /// <returns>Rootnode of new tree</returns>
        public static INode Create(bool allowDuplicateNodes)
        {
            return Node.CreateNode(allowDuplicateNodes);
        }
    }
}