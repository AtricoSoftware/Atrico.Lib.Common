using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atrico.Lib.Common.Collections;

namespace ConsoleApplication1
{
    public partial class NumberMatcher
    {
        private abstract class RegExElement
        {
            public static RegExElement None()
            {
                return new RegExNone();
            }

            public static RegExElement And(RegExElement lhs, RegExElement rhs)
            {
                if (lhs is RegExNone) return rhs;
                if (rhs is RegExNone) return lhs;
                var lhsA = lhs as RegExAnd;
                var rhsA = rhs as RegExAnd;
                if (lhsA != null && rhsA != null) return new RegExAnd(lhsA.Elements.Concat(rhsA.Elements));
                return new RegExAnd(new []{lhs, rhs});
            }

            public static RegExElement Or(RegExElement lhs, RegExElement rhs)
            {
                if (lhs is RegExNone) return rhs;
                if (rhs is RegExNone) return lhs;
                var lhsO = lhs as RegExOr;
                var rhsO = rhs as RegExOr;
                if (lhsO != null && rhsO != null) return new RegExOr(lhsO.Elements.Concat(rhsO.Elements));
                return new RegExOr(new []{lhs, rhs});
            }

            public static RegExElement Digits(IEnumerable<char> digits)
            {
                return new RegExDigits(digits);
            }

            internal IEnumerable<string> Display()
            {
                var lines = new List<string>();
                Display(0, lines);
                return lines;
            }

            private static string PadLine(int depth)
            {
                var line = new StringBuilder(" ");
                for (var i = 0; i < depth; ++i)
                {
                    line.Append(i == depth - 1 ? "+-" : "| ");
                }
                return line.ToString();
            }

            protected virtual void Display(int depth, ICollection<string> lines)
            {
                var line = new StringBuilder(PadLine(depth));
                line.Append(ToString());
                lines.Add(line.ToString());
            }

            private class RegExNone : RegExElement
            {
                public override int GetHashCode()
                {
                    return 0;
                }

                public override bool Equals(object obj)
                {
                    return obj is RegExNone;
                }

                public override string ToString()
                {
                    return "None";
                }
            }

            private abstract class RegExComposite : RegExElement
            {
                internal readonly IEnumerable<RegExElement> Elements;

                protected RegExComposite(IEnumerable<RegExElement> elements)
                {
                    Elements = elements;
                }

                public override int GetHashCode()
                {
                    return Elements.Aggregate(0, (current, item) => current ^ item.GetHashCode());
                }

                protected bool EqualsImpl(RegExComposite other)
                {
                    if (other == null) return false;
                    var thisEn = Elements.GetEnumerator();
                    var otherEn = other.Elements.GetEnumerator();
                    var more = false;
                    do
                    {
                        more = thisEn.MoveNext();
                        if (otherEn.MoveNext() != more) return false;
                        if (!more) continue;
                        if (thisEn.Current != otherEn.Current) return false;
                    } while (more);
                    return true;
                }

                public override string ToString()
                {
                    var text = new StringBuilder();
                         text.Append('(');
                   foreach (var element in Elements)
                    {
                        if (text.Length > 0) text.Append(Separator);
                        text.Append(element);
                    }
                         text.Append(')');
                    return text.ToString();
                }

                protected abstract string Separator { get; }
            }

            private class RegExAnd : RegExComposite
            {
                public RegExAnd(IEnumerable<RegExElement> elements)
                    : base(elements)
                {
                }

                public override bool Equals(object obj)
                {
                    return EqualsImpl(obj as RegExAnd);
                }

                protected override string Separator
                {
                    get { return " & "; }
                }
            }

            private class RegExOr : RegExComposite
            {
                public RegExOr(IEnumerable<RegExElement> elements)
                    : base(elements)

                {
                }

                public override bool Equals(object obj)
                {
                    return EqualsImpl(obj as RegExOr);
                }

                protected override string Separator
                {
                    get { return " | "; }
                }
            }

            private class RegExDigits : RegExElement
            {
                private readonly IEnumerable<char> _digits;

                public RegExDigits(IEnumerable<char> digits)
                {
                    _digits = digits.Distinct().OrderBy(ch => ch);
                }

                public override int GetHashCode()
                {
                    return _digits.Aggregate(0, (current, ch) => current ^ ch.GetHashCode());
                }

                public override bool Equals(object obj)
                {
                    var other = obj as RegExDigits;
                    if (other == null) return false;
                    var thisEn = _digits.GetEnumerator();
                    var otherEn = other._digits.GetEnumerator();
                    var more = false;
                    do
                    {
                        more = thisEn.MoveNext();
                        if (otherEn.MoveNext() != more) return false;
                        if (!more) continue;
                        if (thisEn.Current != otherEn.Current) return false;
                    } while (more);
                    return true;
                }

                public override string ToString()
                {
                    return _digits.ToCollectionString();
                }
            }
        }
    }
}