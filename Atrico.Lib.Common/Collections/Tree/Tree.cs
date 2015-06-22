using Atrico.Lib.Common.Collections.Tree.Implementation;

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
    }
}
