using System;
using System.Collections.Generic;
using System.Linq;
using Atrico.Lib.Common.Collections;

namespace Atrico.Lib.Common.RegEx.Elements
{
    public abstract partial class RegExElement
    {
        private abstract class RegExComposite : RegExElement
        {
            private readonly IEnumerable<RegExElement> _elements;

            protected static RegExElement Create<T>(IEnumerable<RegExElement> elements, Func<IEnumerable<RegExElement>, T> creator) where T : RegExComposite
            {
                if (elements == null)
                {
                    return null;
                }
                var nonNull = elements.Where(el => el != null).ToArray();
                return nonNull.Count() > 1 ? creator(nonNull) : nonNull.FirstOrDefault();
            }

            protected RegExComposite(IEnumerable<RegExElement> elements)
            {
                _elements = elements;
            }

            public override string ToString()
            {
                var braces = _elements.Count() > 1;
                return _elements.ToCollectionString(braces ? StartGroup : "", braces ? EndGroup : "", Separator, false);
            }

            public abstract string Separator { get; }
            public abstract string StartGroup { get; }
            public abstract string EndGroup { get; }
        }
    }
}