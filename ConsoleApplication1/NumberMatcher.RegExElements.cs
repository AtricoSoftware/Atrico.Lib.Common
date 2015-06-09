using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atrico.Lib.Common.Collections.Tree;

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
                return new RegExAnd(new[] {lhs, rhs});
            }

            public static RegExElement Or(RegExElement lhs, RegExElement rhs)
            {
                if (lhs is RegExNone) return rhs;
                if (rhs is RegExNone) return lhs;
                var lhsO = lhs as RegExOr;
                var rhsO = rhs as RegExOr;
                if (lhsO != null && rhsO != null) return new RegExOr(lhsO.Elements.Concat(rhsO.Elements));
                return new RegExOr(new[] {lhs, rhs});
            }

            public static RegExElement Digits(IEnumerable<char> digits)
            {
                return new RegExDigits(digits);
            }

            public Tree<string> ToTree()
            {
                var tree = new Tree<string>();
                AddNodeToTree(tree);
                return tree;
            }

            protected virtual INode<string> AddNodeToTree(INodeContainer<string> container)
            {
                return container.Add(ToString());
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

            protected override INode<string> AddNodeToTree(INodeContainer<string> container)
            {
                var node = container.Add(Separator);
                foreach (var element in Elements)
                {
                    element.AddNodeToTree(node);
                }
                return node;
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
                    var andDigits = this is RegExAnd && Elements.All(el => el is RegExDigits);
                    var text = new StringBuilder();
                    if (!andDigits) text.Append('(');
                    foreach (var element in Elements)
                    {
                        if (text.Length > 1) text.Append(Separator);
                        text.Append(element);
                    }
                    if (!andDigits) text.Append(')');
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

                public override int GetHashCode()
                {
                    return base.GetHashCode();
                }

                public override bool Equals(object obj)
                {
                    return EqualsImpl(obj as RegExAnd);
                }

                protected override string Separator
                {
                    get { return ""; }
                }
            }

            private class RegExOr : RegExComposite
            {
                public RegExOr(IEnumerable<RegExElement> elements)
                    : base(elements)
                {
                }

                public override int GetHashCode()
                {
                    return base.GetHashCode();
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
                private readonly Lazy<string> _regex;

                public RegExDigits(IEnumerable<char> digits)
                {
                    _digits = digits.Distinct().OrderBy(ch => ch);
                    _regex = new Lazy<string>(CreateRegex);
                }

                private string CreateRegex()
                {
                    return _digits.Count() == 10 ? @"\d" : new Simplifier(_digits).ToString();
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
                    return _regex.Value;
                }

                private class Simplifier
                {
                    private enum State
                    {
                        Empty,
                        AddingChar,
                        NoRange,
                        Range,
                        EndingRange
                    }

                    private State _state = State.Empty;
                    private readonly StringBuilder _regex = new StringBuilder();
                    private char _last = '\0';

                    public Simplifier(IEnumerable<char> chars)
                    {
                        foreach (var ch in chars) AddChar(ch);
                    }

                    private void AddChar(char ch)
                    {
                        var done = false;
                        do
                        {
                            switch (_state)
                            {
                                case State.Empty:
                                    _regex.Append('[');
                                    _state = State.AddingChar;
                                    break;
                                case State.AddingChar:
                                    _regex.AppendFormat("{0}", ch);
                                    _state = State.NoRange;
                                    done = true;
                                    break;
                                case State.NoRange:
                                    if (ch == _last + 1)
                                    {
                                        _state = State.Range;
                                        done = true;
                                    }
                                    else
                                    {
                                        _regex.Append(',');
                                        _state = State.AddingChar;
                                    }
                                    break;
                                case State.Range:
                                    if (ch == _last + 1)
                                    {
                                        done = true;
                                    }
                                    else
                                    {
                                        _state = State.EndingRange;
                                    }
                                    break;
                                case State.EndingRange:
                                    _regex.AppendFormat("-{0}", _last);
                                    _state = State.NoRange;
                                    break;
                            }
                        } while (!done);

                        _last = ch;
                    }

                    public override string ToString()
                    {
                        switch (_state)
                        {
                            case State.NoRange:
                                _regex.Append(']');
                                break;
                            case State.Range:
                                _regex.AppendFormat("-{0}]", _last);
                                break;
                        }
                        return _regex.ToString();
                    }
                }
            }
        }
    }
}