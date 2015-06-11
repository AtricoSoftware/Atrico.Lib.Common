using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atrico.Lib.Common;
using Atrico.Lib.Common.Collections.Tree;
using Atrico.Lib.Common.ResettableCache;

namespace ConsoleApplication1
{
    public partial class NumberMatcher : IMultilineDisplayable
    {
        private class RegExDigits
        {
            public IEnumerable<char> Digits { get; private set; }
            private readonly Lazy<string> _regex;

            public RegExDigits(char digit)
                : this(new[]{digit})
            {
            }

            public RegExDigits(IEnumerable<char> digits)
            {
                Digits = digits.Distinct().OrderBy(ch => ch);
                _regex = new Lazy<string>(CreateRegex);
            }

            private string CreateRegex()
            {
                return new Simplifier(Digits).ToString();
            }

            public override int GetHashCode()
            {
                return Digits.Aggregate(0, (current, ch) => current ^ ch.GetHashCode());
            }

            public override bool Equals(object obj)
            {
                var other = obj as RegExDigits;
                if (other == null) return false;
                var thisEn = Digits.GetEnumerator();
                var otherEn = other.Digits.GetEnumerator();
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
                    return _regex.ToString().Replace("[0-9]", @"\d");
                }
            }
        }

        private readonly Tree<RegExDigits>.INode _tree = Tree<RegExDigits>.Create(false);
        private readonly ResettableCache<string> _regex;

        private string CreateRegEx()
        {
             foreach (var line in _tree.ToMultilineString())
            {
                Console.WriteLine(line);
            }
            Console.WriteLine();
            return "";
        }

        public NumberMatcher()
        {
            _regex = new ResettableCache<string>(CreateRegEx);
        }

        public NumberMatcher AddRange(uint from, uint to)
        {
            for (var i = from; i <= to; ++i)
            {
                var digits = GetDigits(i);
                _tree.Add(digits.Select(dg=>new RegExDigits(dg)));
            }
            _regex.Reset();
            return this;
        }

        private static IEnumerable<char> GetDigits(uint u)
        {
            return u.ToString("D").ToCharArray().ToList();
        }

        public string GetRegex()
        {
            return _regex.Value;
        }

        public override string ToString()
        {
            return GetRegex();
        }

        public IEnumerable<string> ToMultilineString()
        {
            return _tree.ToMultilineString();
        }
    }
}