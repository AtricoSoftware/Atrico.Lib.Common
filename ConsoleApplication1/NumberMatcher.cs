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
        private readonly Tree<char>.INode _tree = Tree<char>.Create(false);
        private readonly ResettableCache<string> _regex;

        private string CreateRegEx()
        {
            var regex = RegExElement.Create(_tree).Simplify();
            var tree = regex.ToTree();
            foreach (var line in tree.ToMultilineString())
            {
                Console.WriteLine(line);
            }
            Console.WriteLine();
            return regex.ToString();
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
                _tree.Add(digits);
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