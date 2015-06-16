using System.Collections.Generic;
using System.Linq;
using Atrico.Lib.Common.Collections;
using Atrico.Lib.Common.Collections.Tree;

namespace Atrico.Lib.Common.RegEx.Elements
{
    public abstract partial class RegExElement
    {
        private class RegExSequence : RegExComposite
        {
            public static RegExElement Create(IEnumerable<RegExElement> elements)
            {
                return Create(elements, els => new RegExSequence(els));
            }

            public override RegExElement Simplify()
            {
                // Simplify children
                var elements = SimplifyChildren();
                // Merge equivalent composites
                elements = MergeComposites<RegExSequence>(elements);
                return Create(elements);
            }

            private RegExSequence(IEnumerable<RegExElement> elements)
                : base(elements)
            {
            }

            protected override void AddNodeToTree(Tree<string>.IModifiableNode root)
            {
                AddNodeToTree(root, "AND");
            }

            public override string ToString()
            {
                var braces = Elements.Count() > 1;
                return Elements.ToCollectionString("", "", "", false);
            }
        }
    }
}