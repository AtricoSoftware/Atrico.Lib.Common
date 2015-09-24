using Atrico.Lib.Common.zzImplementation.Collections.Tree;

namespace Atrico.Lib.Common.Collections.Tree
{
    /// <summary>
    ///     Tree
    /// </summary>
    public static class Tree
    {
        /// <summary>
        ///     Creates a tree
        /// </summary>
        /// <param name="allowDuplicateNodes">if set to <c>true</c> allow duplicate nodes</param>
        /// <returns>Rootnode of new tree</returns>
        public static ITreeNodeContainer Create(bool allowDuplicateNodes)
        {
            return new NodeContainer(allowDuplicateNodes);
        }
        /// <summary>
        ///     Creates a typed tree
        /// </summary>
        /// <param name="allowDuplicateNodes">if set to <c>true</c> allow duplicate nodes</param>
        /// <returns>Rootnode of new tree</returns>
        public static ITreeNodeContainer<T> Create<T>(bool allowDuplicateNodes)
        {
            return new NodeContainer<T>(new NodeContainer(allowDuplicateNodes));
        }
    }
}
