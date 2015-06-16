using System.Collections.Generic;
using System.Linq;
using Atrico.Lib.Common.Collections;
using Atrico.Lib.Common.Collections.Tree;

namespace Atrico.Lib.Common.RegEx.Elements
{
    public abstract partial class RegExElement
    {
        private class RegExAlternation : RegExComposite
        {
            public static RegExElement Create(IEnumerable<RegExElement> elements)
            {
                return Create(elements, els => new RegExAlternation(els));
            }

            private RegExAlternation(IEnumerable<RegExElement> elements)
                : base(new SortedSet<RegExElement>(elements))
            {
            }

            #region Simplify

            public override RegExElement Simplify()
            {
                var elements = SimplifyComposite();
                elements = MergeChars(elements);
                return Create(elements);
            }

            private static IEnumerable<RegExElement> MergeChars(IEnumerable<RegExElement> elements)
            {
                var elementsA = elements as RegExElement[] ?? elements.ToArray();
                var chars = elementsA.OfType<RegExChars>().ToArray();
                return chars.Count() < 2 ? elementsA : elementsA.Except(chars).Concat(new []{chars.Aggregate(new RegExChars(), (current, next)=>current.Merge(next))});
            }
            #endregion

            protected override void AddNodeToTree(Tree<string>.IModifiableNode root)
            {
                AddNodeToTree(root, "OR");
            }

            public override string ToString()
            {
                var braces = Elements.Count() > 1;
                return Elements.ToCollectionString(braces ? "(?:" : "", braces ? ")" : "", "|", false);
            }
        }
    }
}