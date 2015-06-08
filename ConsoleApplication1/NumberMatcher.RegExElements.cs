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
                return new RegExAnd(lhs, rhs);
            }

            public static RegExElement Or(RegExElement lhs, RegExElement rhs)
            {
                if (lhs is RegExNone) return rhs;
                if (rhs is RegExNone) return lhs;
                return new RegExOr(lhs, rhs);
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
                private readonly RegExElement _lhs;
                private readonly RegExElement _rhs;

                protected RegExComposite(RegExElement lhs, RegExElement rhs)
                {
                    _lhs = lhs;
                    _rhs = rhs;
                }

                protected override void Display(int depth, ICollection<string> lines)
                {
                    var line = new StringBuilder(PadLine(depth));
                    line.Append(Separator);
                    lines.Add(line.ToString());
                    _lhs.Display(depth + 1, lines);
                    _rhs.Display(depth + 1, lines);
                }

                public override int GetHashCode()
                {
                    return _lhs.GetHashCode() ^ _rhs.GetHashCode();
                }

                protected bool EqualsImpl(RegExComposite other)
                {
                    if (other == null) return false;
                    return _lhs.Equals(other._lhs) && _rhs.Equals(other._rhs);
                }

                public override string ToString()
                {
                    return string.Format("({0}{2}{1})", _lhs, _rhs, Separator);
                }

                protected abstract string Separator { get; }
            }

            private class RegExAnd : RegExComposite
            {
                public RegExAnd(RegExElement lhs, RegExElement rhs)
                    : base(lhs, rhs)
                {
                }

                public override bool Equals(object obj)
                {
                    return EqualsImpl(obj as RegExAnd);
                }

                protected override string Separator
                {
                    get { return " "; }
                }
            }

            private class RegExOr : RegExComposite
            {
                public RegExOr(RegExElement lhs, RegExElement rhs)
                    : base(lhs, rhs)
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