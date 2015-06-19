using System.Collections.Generic;

namespace Atrico.Lib.Common.Collections.Tree
{
    /// <summary>
    ///     Interface to node that can be modified
    /// </summary>
    public interface IModifiableTreeNodeContainer : ITreeNodeContainer
    {
        IModifiableTreeNode Add(object data);
        IModifiableTreeNode Add(IEnumerable<object> path);
        IModifiableTreeNode Insert(object data);
        void Remove(object data);
    }
}