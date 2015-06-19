namespace Atrico.Lib.Common.Collections.Tree
{
    /// <summary>
    ///     Interface to node that can be modified
    /// </summary>
    public interface IModifiableTreeNode : IModifiableTreeNodeContainer, ITreeNode
    {
        new object Data { get; set; }
    }
}