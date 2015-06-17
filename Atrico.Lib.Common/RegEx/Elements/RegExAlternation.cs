using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Atrico.Lib.Common.Collections;
using Atrico.Lib.Common.Collections.Tree;

namespace Atrico.Lib.Common.RegEx.Elements
{
    public abstract partial class RegExElement
    {
        private class RegExAlternation : RegExComposite
        {
            // TODO - Make parameterisable
            private const bool _explicitCapture = true;

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
                elements = FactoriseAnds(elements);
                return Create(elements);
            }

            private static IEnumerable<RegExElement> MergeChars(IEnumerable<RegExElement> elements)
            {
                var elementsA = elements as RegExElement[] ?? elements.ToArray();
                var chars = elementsA.OfType<RegExChars>().ToArray();
                return chars.Count() < 2 ? elementsA : elementsA.Except(chars).Concat(new[] {chars.Aggregate(new RegExChars(), (current, next) => current.Merge(next))});
            }

            private struct ElementData
            {
                public RegExElement Element;
                public RegExSequence Parent;
            }
            private static IEnumerable<RegExElement> FactoriseAnds(IEnumerable<RegExElement> elements)
            {
                // (A & B) | (A & C) => A & (B | C)
                var elementsL = elements.ToList();
                var ands = elementsL.OfType<RegExSequence>().ToArray();
                if( ands.Count() < 2) return  elementsL;
                var groups = new Dictionary<RegExElement, IList<ElementData>>();
                foreach (var and in ands)
                {
                    var same = and.Elements.First();
                    var or = and.Elements.Skip(1).Single();
                    IList<ElementData> list;
                    if (!groups.TryGetValue(same, out list))
                    {
                        list = new List<ElementData>();
                        groups.Add(same, list);
                    }
                    list.Add(new ElementData{Element = or, Parent = and});
                }
                foreach (var entry in groups.Where(entry => entry.Value.Count > 1))
                {
                    var or = CreateAlternation(entry.Value.Select(ent=>ent.Element).ToArray());
                    var and = CreateSequence(entry.Key, or);
                    elementsL.RemoveAll(el => entry.Value.Select(dat => dat.Parent).Contains(el));
                    elementsL.Add(and);
                }
                 return  elementsL;
            }

            #endregion

            protected override void AddNodeToTree(Tree<string>.IModifiableNode root)
            {
                AddNodeToTree(root, "OR");
            }

            public override string ToString()
            {
                var braces = Elements.Count() > 1;
                var openBrace = braces ? (_explicitCapture ? "(" : "(?:") : "";
                var closeBrace = braces ? ")" : "";
                return Elements.ToCollectionString(openBrace, closeBrace, "|", false);
            }
        }
    }
}