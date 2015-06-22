namespace Atrico.Lib.Common.Collections.Tree
{
    /// <summary>
    ///     Interface to node
    /// </summary>
    public interface ITreeNode : ITreeNodeContainer
    {
        /// <summary>
        ///     The Node data
        /// </summary>
        object Data { get; set; }

        /// <summary>
        ///     Parent node
        ///     null if root node
        /// </summary>
        ITreeNode Parent { get; }
    }
}