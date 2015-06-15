using System.Linq;
using Atrico.Lib.Common.Collections.Tree;

namespace Atrico.Lib.Common.RegEx.Elements
{
    public abstract partial class RegExElement
    {
        /// <summary>
        ///     Creates an element from a tree of characters
        /// </summary>
        /// <param name="node">The root node of the tree</param>
        /// <returns>New element</returns>
        public static RegExElement Create(Tree<char>.INode node)
        {
            RegExElement element = null;
            var children = node.Children.Select(Create).ToArray();
            if (children.Any())
            {
                // Alternation of all children
                element = RegExAlternation.Create(children);
            }
            if (!node.IsRoot())
            {
                // Sequence of data + children
                element = RegExSequence.Create(new[] {Create(node.Data), element});
            }
            return element;
        }

        /// <summary>
        ///     Creates an element from a list of characters
        /// </summary>
        /// <param name="chars">The characters</param>
        /// <returns>New element</returns>
        public static RegExElement Create(params char[] chars)
        {
            return new RegExChars(chars);
        }

        /// <summary>
        ///     Creates an Alternation element
        /// </summary>
        /// <param name="elements">The reg ex elements</param>
        /// <returns>New element</returns>
        public static RegExElement CreateOr(params RegExElement[] elements)
        {
            return RegExAlternation.Create(elements);
        }

        /// <summary>
        ///     Creates a Sequence element
        /// </summary>
        /// <param name="elements">The reg ex elements</param>
        /// <returns>New element</returns>
        public static RegExElement CreateSequence(params RegExElement[] elements)
        {
            return RegExSequence.Create(elements);
        }
    }
}