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

            #region simplify

            public override RegExElement Simplify()
            {
                var elements = SimplifyComposite();
                elements = CreateRepeats(elements);
                return Create(elements);
            }

            private static IEnumerable<RegExElement> CreateRepeats(IEnumerable<RegExElement> elements)
            {
                var elementsA = elements as RegExElement[] ?? elements.ToArray();
                var groups = elementsA.PartitionBy(el => el).ToArray();
                return groups.Any(gr => gr.Count() > 1) ? groups.Select(@group => @group.Count() < 2 ? @group.Key : CreateRepeat(@group.Key, @group.Count())) : elementsA;
            }

            #endregion

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