using System;
using System.Collections.Generic;
using System.Linq;
using Atrico.Lib.Common;
using Atrico.Lib.Common.Collections.Tree;
using Atrico.Lib.Common.ResettableCache;

namespace ConsoleApplication1
{
    public partial class NumberMatcher : IMultilineDisplayable
    {
        private readonly Tree<char> _tree = new Tree<char>(false);
        private readonly ResettableCache<string> _regex;

        private string CreateRegEx()
        {
            //var regex = _tree.GetRegex();
            //var tree = regex.ToTree();
            //foreach (var line in tree.ToMultilineString())
            //{
            //    Console.WriteLine(line);
            //}
            //Console.WriteLine();
            //return regex.ToString();
            return "";
        }

        private int _digits = 1;

        public NumberMatcher()
        {
            _regex = new ResettableCache<string>(CreateRegEx);
        }

        public NumberMatcher AddRange(uint from, uint to)
        {
            for (var i = from; i <= to; ++i)
            {
                var digits = GetDigits(i);
                _tree.Add(digits);
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
                _tree.Insert('0');
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

        public IEnumerable<string> ToMultilineString()
        {
            return _tree.ToMultilineString();
        }
    }
}