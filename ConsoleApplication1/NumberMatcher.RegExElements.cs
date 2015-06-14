using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atrico.Lib.Common.Collections;
using Atrico.Lib.Common.Collections.Tree;

namespace ConsoleApplication1
{
    public partial class NumberMatcher
    {
        private abstract class RegExElement
        {
            public static RegExElement Create(Tree<RegExDigits>.INode node)
            {
                RegExElement element = null;
                var children = node.Children.Select(Create).ToArray();
                if (children.Any())
                {
                    element = RegExOr.Create(children);
                }
                if (!node.IsRoot())
                {
                    element = RegExAnd.Create(node.Data, element);
                }
                return element;
            }

            public abstract bool IsChar { get; }
        }

        private class RegExDigits : RegExElement
        {
            private readonly IEnumerable<char> _digits;
            private readonly Lazy<string> _regex;

            public override bool IsChar
            {
                get { return true; }
            }

            public RegExDigits()
                : this(new char[] {})
            {
            }

            public RegExDigits(char digit)
                : this(new[] {digit})
            {
            }

            private RegExDigits(IEnumerable<char> digits)
            {
                _digits = digits.Distinct().OrderBy(ch => ch);
                _regex = new Lazy<string>(CreateRegex);
            }

            public RegExDigits Merge(RegExDigits rhs)
            {
                return new RegExDigits(_digits.Concat(rhs._digits));
            }

            private string CreateRegex()
            {
                return new Simplifier(_digits).ToString();
            }

            public override int GetHashCode()
            {
                return _digits.Aggregate(0, (current, ch) => current ^ ch.GetHashCode());
            }

            public override bool Equals(object obj)
            {
                var other = obj as RegExDigits;
                if (other == null)
                {
                    return false;
                }
                var thisEn = _digits.GetEnumerator();
                var otherEn = other._digits.GetEnumerator();
                var more = false;
                do
                {
                    more = thisEn.MoveNext();
                    if (otherEn.MoveNext() != more)
                    {
                        return false;
                    }
                    if (!more)
                    {
                        continue;
                    }
                    if (thisEn.Current != otherEn.Current)
                    {
                        return false;
                    }
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
                    foreach (var ch in chars)
                    {
                        AddChar(ch);
                    }
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
                    var regex = _regex.ToString();
                    // Final simplfication
                    if (regex == "[0-9]")
                    {
                        return @"\d";
                    }
                    if (regex.Length == 3)
                    {
                        return regex[1].ToString();
                    }
                    return regex;
                }
            }
        }

        private abstract class RegExComposite : RegExElement
        {
            private readonly IEnumerable<RegExElement> _elements;

            public override bool IsChar
            {
                get { return false; }
            }

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

        private class RegExOr : RegExComposite
        {
            public static RegExElement Create(params RegExElement[] elements)
            {
                return Create(elements, CreateImpl);
            }

            private static RegExOr CreateImpl(IEnumerable<RegExElement> elements)
            {
                return new RegExOr(elements);
            }

            private RegExOr(IEnumerable<RegExElement> elements)
                : base(elements)
            {
            }

            public override string Separator
            {
                get { return " | "; }
            }

            public override string StartGroup
            {
                get { return "("; }
            }

            public override string EndGroup
            {
                get { return ")"; }
            }
        }

        private class RegExAnd : RegExComposite
        {
            public static RegExElement Create(params RegExElement[] elements)
            {
                return Create(elements, CreateImpl);
            }

            private static RegExAnd CreateImpl(IEnumerable<RegExElement> elements)
            {
                // Check for sequences
                //var groups = nonNull.PartitionBy(el => el);
                return new RegExAnd(elements);
            }

            private RegExAnd(IEnumerable<RegExElement> elements)
                : base(elements)
            {
            }

            public override string Separator
            {
                get { return ""; }
            }

            public override string StartGroup
            {
                get { return ""; }
            }

            public override string EndGroup
            {
                get { return ""; }
            }
        }

        private class RegExSequence : RegExElement
        {
            private readonly char _ch;
            private readonly int _count;

            public override bool IsChar
            {
                get { return false; }
            }

            public RegExSequence(char ch, int count)
            {
                _ch = ch;
                _count = count;
            }

            public override string ToString()
            {
                return string.Format("{0}{{{1}}}", _ch, _count);
            }
        }
    }
}
