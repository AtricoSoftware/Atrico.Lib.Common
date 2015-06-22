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

            private class AlternateElementData : EquatableObject<AlternateElementData>
            {
                public IEnumerable<RegExElement> ElementsBefore = new RegExElement[] {};
                public IEnumerable<RegExElement> ElementsAfter = new RegExElement[] {};

                protected override int GetHashCodeImpl()
                {
                    return (ElementsBefore.FirstOrDefault() ?? ElementsAfter.FirstOrDefault() ?? new RegExChars()).GetHashCode();
                }

                protected override bool EqualsImpl(AlternateElementData other)
                {
                    return ElementsBefore.SequenceEqual(other.ElementsBefore) && ElementsAfter.SequenceEqual(other.ElementsAfter);
                }
            }

            private struct EqualElementData
            {
                public RegExElement Element;
                public RegExSequence Parent;
            }

            private static IEnumerable<RegExElement> FactoriseAnds(IEnumerable<RegExElement> elements)
            {
                // (X & A) | (X & B) => X & (A | B)
                // (A & X) | (B & X) => (A | B) & X
                // (X & Y & A) | (X & Y & B) => X & Y & (A | B)
                // (A & X & Y) | (B & X & Y) => (A | B) & X & Y

                var elementsL = elements.ToList();
                var ands = elementsL.OfType<RegExSequence>().ToArray();
                if (ands.Count() < 2) return elementsL;
                var groups = new Dictionary<AlternateElementData, IList<EqualElementData>>();
                foreach (var and in ands)
                {
                    for (var i = 0; i < and.Elements.Count(); ++i)
                    {
                        var before = and.Elements.Take(i);
                        var option = and.Elements.ElementAt(i);
                        var after = and.Elements.Skip(i + 1);
                        IList<EqualElementData> list;
                        var key = new AlternateElementData {ElementsBefore = before, ElementsAfter = after};
                        if (!groups.TryGetValue(key, out list))
                        {
                            list = new List<EqualElementData>();
                            groups.Add(key, list);
                        }
                        list.Add(new EqualElementData {Element = option, Parent = and});
                    }
                }
                foreach (var entry in groups.Where(entry => entry.Value.Count > 1))
                {
                    var or = CreateAlternation(entry.Value.Select(ent => ent.Element).ToArray()).Simplify();

                    var and = CreateSequence(entry.Key.ElementsBefore.Concat(new[] {or}).Concat(entry.Key.ElementsAfter).ToArray()).Simplify();
                    elementsL.RemoveAll(el => entry.Value.Select(dat => dat.Parent).Contains(el));
                    elementsL.Add(and);
                }
                return elementsL;
            }

            #endregion

            protected override void AddNodeToTree(ITreeNodeContainer root)
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