using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atrico.Lib.Common.Collections;
using Atrico.Lib.Common.Collections.Tree;

namespace Atrico.Lib.Common.RegEx.Elements
{
    public abstract partial class RegExElement
    {
        private class RegExChars : RegExElement
        {
            private readonly IEnumerable<char> _characters;
            private readonly Lazy<string> _regex;

            public RegExChars()
                : this(new char[]{})
            {
                
            }
            
           public RegExChars(IEnumerable<char> characters)
            {
                _characters = characters.Distinct().OrderBy(ch => ch);
                _regex = new Lazy<string>(CreateRegex);
            }

            public RegExChars Merge(RegExChars rhs)
            {
                return new RegExChars(_characters.Concat(rhs._characters));
            }

             private string CreateRegex()
            {
                return new Simplifier(_characters).ToString();
            }

            #region Comparison

            protected override int GetHashCodeImpl()
            {
                return _characters.Aggregate(0, (current, ch) => current ^ ch.GetHashCode());
            }

            protected override bool EqualsImpl(RegExElement obj)
            {
                var other = obj as RegExChars;
                return other != null && _characters.SequenceEqual(other._characters);
            }

            protected override int CompareToImpl(RegExElement obj)
            {
                var other = obj as RegExChars;
                // Other types always smaller, otherwise Compare characters
                return ReferenceEquals(other, null) ? 1 : _characters.SequenceCompare(other._characters);
            }

            #endregion

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
                    // Final simplfication - TODO other chars, not just digits
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

            protected override void AddNodeToTree(ITreeNodeContainer root)
            {
                root.Add(ToString());
            }
        }
    }
}