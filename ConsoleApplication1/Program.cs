using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atrico.Lib.Common.ResettableCache;

namespace ConsoleApplication1
{
    public class NumberMatcher
    {
        private class DigitMatcher
        {
            private readonly IDictionary<char, DigitMatcher> _next = new Dictionary<char, DigitMatcher>();

            public DigitMatcher Add(IEnumerable<char> value)
            {
                var valueArray = value as char[] ?? value.ToArray();
                if (!valueArray.Any()) return this;
                if (!_next.ContainsKey(valueArray[0]))
                {
                    _next.Add(valueArray[0], new DigitMatcher());
                }
                return _next[valueArray[0]].Add(valueArray.Skip(1));
            }

            public DigitMatcher()
            {
            }

            private DigitMatcher(DigitMatcher before)
                : this()
            {
                _next['0'] = before;
            }

            public string GetRegex()
            {
                var options = new Dictionary<string, ISet<char>>();
                foreach (var entry in _next)
                {
                    var reg = entry.Value.GetRegex();
                    if (!options.ContainsKey(reg)) options.Add(reg, new HashSet<char>());
                    options[reg].Add(entry.Key);
                }
                var regex = new StringBuilder();
                foreach (var option in options)
                {
                    if (regex.Length > 0) regex.Append(" | ");
                    regex.AppendFormat("({0}{1})", ToRegex(option.Value), option.Key);
                }
                return regex.ToString();
            }

            private enum State
            {
                Empty,
                AddingChar,
                NoRange,
                Range,
                EndingRange
            }

            private class StateMachine
            {
                private State _state = State.Empty;
                private readonly StringBuilder _regex = new StringBuilder();
                private char _last = '\0';

                public void AddChar(char ch)
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

            private static string ToRegex(IEnumerable<char> chars)
            {
                var charsArray = chars as char[] ?? chars.ToArray();
                if (!charsArray.Any()) return "";
                var stateMachine = new StateMachine();
                foreach (var ch in charsArray.OrderBy(ch => ch))
                {
                    stateMachine.AddChar(ch);
                }
                return stateMachine.ToString();
            }

            internal IEnumerable<string> Display()
            {
                var lines = new List<string>();
                Display(0, lines);
                return lines;
            }

            private void Display(int depth, ICollection<string> lines)
            {
                foreach (var child in _next)
                {
                    var line = new StringBuilder(" ");
                    for (var i = 0; i < depth; ++i)
                    {
                        line.Append(i == depth - 1 ? "+-" : "| ");
                    }
                    line.Append(child.Key);
                    lines.Add(line.ToString());
                    child.Value.Display(depth + 1, lines);
                }
            }

            public override string ToString()
            {
                var text = new StringBuilder();
                foreach (var child in _next)
                {
                    if (text.Length > 0) text.Append(',');
                    text.AppendFormat("{0}->[{1}]", child.Key, child.Value);
                }
                return text.ToString();
            }

            public static DigitMatcher Before(DigitMatcher before)
            {
                return before._next.Any() ? new DigitMatcher(before) : before;
            }
        }

        private DigitMatcher _root = new DigitMatcher();
        private readonly ResettableCache<string> _regex;
        private int _digits = 1;

        public NumberMatcher()
        {
            _regex = new ResettableCache<string>(() => _root.GetRegex());
        }

        internal IEnumerable<string> Display()
        {
            return _root.Display();
        }

        public NumberMatcher AddRange(uint from, uint to)
        {
            for (var i = from; i <= to; ++i)
            {
                var digits = GetDigits(i);
                _root.Add(digits);
            }
            _regex.Reset();
            return this;
        }

        private IEnumerable<char> GetDigits(uint u)
        {
            var digits = u.ToString("D").ToCharArray().ToList();
            // Pad if too small
            while (digits.Count < _digits) digits.Insert(0, '0');
            // Pad tree if too big
            while (digits.Count > _digits)
            {
                _root = DigitMatcher.Before(_root);
                ++_digits;
            }
            return digits;
        }

        public string GetRegex()
        {
            return _regex.Value;
        }

        public override string ToString()
        {
            return GetRegex();
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            var nm = new NumberMatcher();
            nm.AddRange(123, 321);
            foreach (var line in nm.Display())
            {
                Console.WriteLine(line);
            }
            var regex = nm.GetRegex();
            Console.WriteLine(regex);
        }
    }
}