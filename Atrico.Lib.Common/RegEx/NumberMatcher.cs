using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atrico.Lib.Common.Collections.Tree;
using Atrico.Lib.Common.RegEx.Elements;
using Atrico.Lib.Common.ResettableCache;

namespace Atrico.Lib.Common.RegEx
{
    /// <summary>
    ///     Useful reg ex constants
    /// </summary>
    public static class RegExHelpers
    {
        public class NumberMatcher
        {
            private readonly Tree<char>.IModifiableNode _tree = Tree<char>.Create(false);
            private readonly ResettableCache<RegExElement> _element;
            private readonly ResettableCache<string> _regex;

            // TODO - parameterisable
            private const bool _wholeString = true;

            public NumberMatcher()
            {
                _regex = new ResettableCache<string>(CreateRegex);
                _element = new ResettableCache<RegExElement>(() => RegExElement.Create(_tree).Simplify(), _regex);
            }

            public NumberMatcher AddRange(uint from, uint to)
            {
                for (var i = from; i <= to; ++i)
                {
                    var digits = GetDigits(i);
                    _tree.Add(digits);
                }
                _element.Reset();
                return this;
            }

            private string CreateRegex()
            {
                return _element.Value.ToString();
            }

            private static IEnumerable<char> GetDigits(uint u)
            {
                return u.ToString("D").ToCharArray().ToList();
            }

            public override string ToString()
            {
                var regex = new StringBuilder(_regex.Value);
                if (_wholeString)
                {
                    regex.Insert(0, '^');
                    regex.Append('$');
                }
                return regex.ToString();
            }

            internal Tree<char>.INode CharacterTree
            {
                get { return _tree; }
            }

            internal RegExElement Element
            {
                get { return _element.Value; }
            }
        }
    }
}