using System;
using System.Collections.Generic;
using System.Linq;
using Atrico.Lib.Common.Collections;
using Atrico.Lib.Common.Collections.Tree;

namespace Atrico.Lib.Common.RegEx.Elements
{
    public abstract partial class RegExElement
    {
        private abstract class RegExComposite : RegExElement
        {
            public readonly IEnumerable<RegExElement> Elements;

            protected RegExComposite(IEnumerable<RegExElement> elements)
            {
                Elements = elements;
            }

            protected static RegExElement Create(IEnumerable<RegExElement> elements, Func<IEnumerable<RegExElement>, RegExElement> creator)
            {
                if (elements == null) return null;
                var elementsA = elements as RegExElement[] ?? elements.ToArray();
                var nonNull = elementsA.Where(el => el != null).ToArray();
                return nonNull.Count() > 1 ? creator(elementsA) : nonNull.FirstOrDefault();
            }

            protected void AddNodeToTree(TreeT<string>.IModifiableNode root, string nodeName)
            {
                var thisNode = root.Add(nodeName);
                foreach (var element in Elements)
                {
                    element.AddNodeToTree(thisNode);
                }
            }

            #region Simplify

            protected IEnumerable<RegExElement> SimplifyComposite()
            {
                // Simplify children
                var elements = SimplifyChildren();
                // Merge equivalent composites
                elements = MergeComposites(elements);
                return elements;
            }

            private IEnumerable<RegExElement> SimplifyChildren()
            {
                // Simplify children
                return Elements.Select(el => el.Simplify());
            }

            private IEnumerable<RegExElement> MergeComposites(IEnumerable<RegExElement> originalElements)
            {
                var elements = new Stack<RegExElement>(originalElements.Reverse());
                var newElements = new List<RegExElement>();
                while (elements.Any())
                {
                    var element = elements.Pop();
                    var elAlt = element as RegExComposite;
                    if (ReferenceEquals(elAlt, null) || GetType() != elAlt.GetType()) newElements.Add(element);
                    else elAlt.Elements.Reverse().ForEach(elements.Push);
                }
                return newElements;
            }

            #endregion

            #region Comparison

            protected override int GetHashCodeImpl()
            {
                return Elements.Aggregate(0, (current, child) => current ^ child.GetHashCode());
            }

            protected override bool EqualsImpl(RegExElement obj)
            {
                var other = obj as RegExComposite;
                return !ReferenceEquals(other, null) && GetType() == other.GetType() && Elements.SequenceEqual(other.Elements);
            }

            protected override int CompareToImpl(RegExElement obj)
            {
                var other = obj as RegExComposite;
                // Other types always smaller, otherwise Compare character set as string
                return !ReferenceEquals(other, null) && GetType() == other.GetType() ? Elements.SequenceCompare(other.Elements) : 1;
            }

            #endregion
        }
    }
}