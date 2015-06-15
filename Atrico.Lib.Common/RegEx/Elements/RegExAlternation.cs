using System.Collections.Generic;

namespace Atrico.Lib.Common.RegEx.Elements
{
    public abstract partial class RegExElement
    {
        private class RegExAlternation : RegExComposite
        {
            public static RegExElement Create(IEnumerable<RegExElement> elements)
            {
                return Create(elements, CreateImpl);
            }

            private static RegExAlternation CreateImpl(IEnumerable<RegExElement> elements)
            {
                return new RegExAlternation(elements);
            }

            private RegExAlternation(IEnumerable<RegExElement> elements)
                : base(elements)
            {
            }

            public override string Separator
            {
                get { return "|"; }
            }

            public override string StartGroup
            {
                get { return "(?:"; }
            }

            public override string EndGroup
            {
                get { return ")"; }
            }
        }
    }
}