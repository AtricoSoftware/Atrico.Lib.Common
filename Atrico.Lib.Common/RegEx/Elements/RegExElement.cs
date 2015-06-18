using System.Linq;
using Atrico.Lib.Common.Collections.Tree;

namespace Atrico.Lib.Common.RegEx.Elements
{
    public abstract partial class RegExElement : ComparableObject<RegExElement>
    {
        public const char Terminator = '\0';

        /// <summary>
        ///     Creates an element from a tree of characters
        /// </summary>
        /// <param name="node">The root node of the tree</param>
        /// <returns>New element</returns>
        public static RegExElement Create(Tree<CharNode>.INode node)
        {
            RegExElement element = null;
            var children = node.Children.Select(Create).ToArray();
            if (children.Any())
            {
                // Alternation of all children
                element = RegExAlternation.Create(children);
            }
            if (node.IsRoot()) return element;
            // Sequence of data + children
            element = RegExSequence.Create(new[] {Create(node.Data.Char), element});
            if (node.Data.IsTerminator)
            {
                // Split here
                element = RegExAlternation.Create(new[] {Create(node.Data.Char), element});
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
        public static RegExElement CreateAlternation(params RegExElement[] elements)
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

        /// <summary>
        ///     Creates a Repeat element
        /// </summary>
        /// <param name="element">The reg ex element</param>
        /// <param name="repeats">Number of repeats</param>
        /// <returns>New element</returns>
        public static RegExElement CreateRepeat(RegExElement element, int repeats)
        {
            return RegExRepeat.Create(element, repeats);
        }

        /// <summary>
        ///     Simplifies this element (merge nodes, etc)
        /// </summary>
        /// <returns>New element</returns>
        public virtual RegExElement Simplify()
        {
            // No simplification
            return this;
        }

        /// <summary>
        ///     Convert to text tree
        ///     For debugging purposes
        /// </summary>
        /// <returns>Text tree</returns>
        internal Tree<string>.INode ToTree()
        {
            var tree = Tree<string>.Create(true);
            AddNodeToTree(tree);
            return tree;
        }

        protected abstract void AddNodeToTree(Tree<string>.IModifiableNode root);
    }
}