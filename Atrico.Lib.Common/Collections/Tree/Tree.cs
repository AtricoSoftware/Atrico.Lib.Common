using Atrico.Lib.Common.Collections.Tree.Implementation;

namespace Atrico.Lib.Common.Collections.Tree
{
    /// <summary>
    ///     Typed tree
    /// </summary>
    public static class Tree
    {
        /// <summary>
        ///     Creates the tree
        /// </summary>
        /// <param name="allowDuplicateNodes">if set to <c>true</c> allow duplicate nodes</param>
        /// <returns>Rootnode of new tree</returns>
        public static ITreeNodeContainer Create(bool allowDuplicateNodes)
        {
            return new NodeContainer(allowDuplicateNodes);
        }
    }
}