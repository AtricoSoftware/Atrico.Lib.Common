using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atrico.Lib.Common.Collections;
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

            private readonly Tree<RegExElement.CharNode>.IModifiableNode _tree = Tree<RegExElement.CharNode>.Create(false);
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

            private static IEnumerable<RegExElement.CharNode> GetDigits(uint u)
            {
                var digits = u.ToString("D").ToCharArray().Select(ch=>new RegExElement.CharNode(ch)).ToArray();
                digits.Last().IsTerminator = true;
                return digits;
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

            internal Tree<RegExElement.CharNode>.INode CharacterTree
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