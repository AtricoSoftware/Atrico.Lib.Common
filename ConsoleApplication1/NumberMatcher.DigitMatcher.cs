using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atrico.Lib.Common.ResettableCache;

namespace ConsoleApplication1
{
    public partial class NumberMatcher
    {
        private class DigitMatcher
        {
            private readonly IDictionary<char, DigitMatcher> _next = new Dictionary<char, DigitMatcher>();
            private readonly ResettableCache<RegExElement> _regex;

            public DigitMatcher Add(IEnumerable<char> value)
            {
                var valueArray = value as char[] ?? value.ToArray();
                if (!valueArray.Any()) return this;
                _regex.Reset();
                if (!_next.ContainsKey(valueArray[0]))
                {
                    _next.Add(valueArray[0], new DigitMatcher());
                }
                return _next[valueArray[0]].Add(valueArray.Skip(1));
            }

            public DigitMatcher()
            {
                _regex = new ResettableCache<RegExElement>(GetRegex);
            }

            private DigitMatcher(DigitMatcher before)
                : this()
            {
                _next['0'] = before;
            }

            public RegExElement GetRegex()
            {
                var element = RegExElement.None();
                if (!_next.Any()) return element;
                var options = new Dictionary<RegExElement, ISet<char>>();
                foreach (var entry in _next)
                {
                    var reg = entry.Value.GetRegex();
                    if (!options.ContainsKey(reg)) options.Add(reg, new HashSet<char>());
                    options[reg].Add(entry.Key);
                }
                foreach (var option in options)
                {
                    var digits = RegExElement.And(RegExElement.Digits(option.Value), option.Key);
                    element = RegExElement.Or(element, digits);
                }
                return element;
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
    }
}