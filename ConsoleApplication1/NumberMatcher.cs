using System.Collections.Generic;
using System.Linq;
using Atrico.Lib.Common.ResettableCache;

namespace ConsoleApplication1
{
    public partial class NumberMatcher
    {
        private DigitMatcher _root = new DigitMatcher();
        private readonly ResettableCache<string> _regex;

        private string CreateRegEx()
        {
            var regex = _root.GetRegex();
            return regex.ToString();
        }

        private int _digits = 1;

        public NumberMatcher()
        {
            _regex = new ResettableCache<string>(CreateRegEx);
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
}