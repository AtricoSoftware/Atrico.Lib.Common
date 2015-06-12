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
        private readonly Tree<RegExDigits>.IModifiableNode _tree = Tree<RegExDigits>.Create(false);
        private readonly ResettableCache<string> _regex;

        private string CreateRegEx()
        {
            var mergedTree = _tree.Transform(MergeDigits);
            // TODO - start
            Console.WriteLine("RegEx");
            foreach (var line in mergedTree.ToMultilineString())
            {
                Console.WriteLine(line);
            }
            Console.WriteLine();
            // TODO - end
            return TreeToRegex(mergedTree);
        }

        private static Tree<RegExDigits>.INode MergeDigits(Tree<RegExDigits>.IModifiableNode node)
        {
            if (node.IsLeaf()) return node;
            var leaves = node.GetLeaves().Where(l => l.Any());
            if (!node.IsRoot()) leaves = leaves.Select(l => l.Skip(1));

            var leafGroups = leaves.GroupBy(n => n.Skip(1), n => n.First(), new ListSequenceEqualityComparer<RegExDigits>());
            foreach (var group in leafGroups)
            {
                var digits = new RegExDigits();
                foreach (var item in group)
                {
                    digits = digits.Merge(item);
                    node.Remove(item);
                }
                node.Add(new[] {digits}.Concat(group.Key));
            }

            return node;
        }

        private static string TreeToRegex(Tree<RegExDigits>.INode node)
        {
            var text = new StringBuilder();
            if (!node.IsRoot()) text.Append(node.Data);
            if (node.Children.Count() == 1) text.Append(TreeToRegex(node.Children.First()));
            else if (node.Children.Count() > 1)
            {
                var first = true;
                foreach (var child in node.Children)
                {
                    text.Append(first ? "(?:" : " | ");
                    first = false;
                    text.Append(TreeToRegex(child));
                }
                text.Append(')');
            }
            return text.ToString();
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
                _tree.Add(digits.Select(dg => new RegExDigits(dg)));
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