namespace Atrico.Lib.Common.Collections.Tree
{
    /// <summary>
    ///     Interface to a node
    /// </summary>
    public interface ITreeNode : ITreeNodeContainer
    {
        /// <summary>
        ///     The Node data
        /// </summary>
        object Data { get; set; }

        /// <summary>
        ///     Parent node
        /// </summary>
        ITreeNodeContainer Parent { get; }

        /// <summary>
        ///     Clones this instance and makes modifiable
        /// </summary>
        /// <param name="deep">If true, clone children</param>
        /// <returns>Copy of node</returns>
        ITreeNode Clone(bool deep = false);

        /// <summary>
        ///     Inserts a node with the specified data
        /// </summary>
        /// <param name="data">Node data</param>
        /// <returns>New node</returns>
        ITreeNode Insert(object data);
    }

    /// <summary>
    ///     Interface to a typed node
    /// </summary>
    public interface ITreeNode<T> : ITreeNodeContainer<T>
    {
        /// <summary>
        ///     The Node data
        /// </summary>
        T Data { get; set; }

        /// <summary>
        ///     Parent node
        /// </summary>
        ITreeNodeContainer<T> Parent { get; }

        /// <summary>
        ///     Clones this instance and makes modifiable
        /// </summary>
        /// <param name="deep">If true, clone children</param>
        /// <returns>Copy of node</returns>
        ITreeNode<T> Clone(bool deep = false);

        /// <summary>
        ///     Inserts a node with the specified data
        /// </summary>
        /// <param name="data">Node data</param>
        /// <returns>New node</returns>
        ITreeNode<T> Insert(T data);
    }
}
