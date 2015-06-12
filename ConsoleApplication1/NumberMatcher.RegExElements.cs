using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    public partial class NumberMatcher
    {
        private abstract class RegExElements
        {
        }

        private class RegExDigits : RegExElements
        {
            private readonly IEnumerable<char> _digits;
            private readonly Lazy<string> _regex;

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
                    var regex = _regex.ToString();
                    // Final simplfication
                    if (regex == "[0-9]") return @"\d";
                    if (regex.Length == 3) return regex[1].ToString();
                    return regex;
                }
            }
        }
    }
}