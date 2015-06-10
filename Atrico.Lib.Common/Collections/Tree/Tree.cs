using System.Collections.Generic;

namespace Atrico.Lib.Common.Collections.Tree
{
    /// <summary>
    ///     Tree of nodes
    /// </summary>
    public static partial class Tree<T>
    {
         public static INode Create(bool allowDuplicateNodes)
        {
            return new Node(allowDuplicateNodes);
        }

    }
}