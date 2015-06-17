using System.Collections.Generic;
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
                var elements = SimplifyComposite();
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
                return Elements.ToCollectionString("", "", "", false);
            }
        }
    }
}