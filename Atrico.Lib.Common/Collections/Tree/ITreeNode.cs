namespace Atrico.Lib.Common.Collections.Tree
{
    /// <summary>
    ///     Interface to node
    /// </summary>
    public interface ITreeNode : ITreeNodeContainer
    {
        object Data { get; }
        IModifiableTreeNode Parent { get; }
    }
}