using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Atrico.Lib.Common.Collections.Tree;
using Atrico.Lib.Common.RegEx.Elements;
using Atrico.Lib.Testing;

namespace Atrico.Lib.Common.Test.RegEx
{
    public class RegExTestFixtureBase : TestFixtureBase
    {
        protected class TestElement : RegExElement
        {
            private readonly string _text;

            public TestElement(string text = "test")
            {
                _text = text;
            }

            protected override int GetHashCodeImpl()
            {
                return _text.GetHashCode();
            }

            protected override bool EqualsImpl(RegExElement obj)
            {
                var other = obj as TestElement;
                return !ReferenceEquals(other, null) && _text.Equals(other._text);
            }

            protected override int CompareToImpl(RegExElement obj)
            {
                var other = obj as TestElement;
                return ReferenceEquals(other, null) ? 1 : String.Compare(_text, other._text, StringComparison.Ordinal);
            }

            public override RegExElement Simplify()
            {
                return this;
            }

            protected override void AddNodeToTree(ITreeNodeContainer root)
            {
                root.Add(ToString());
            }

            public override string ToString()
            {
                return _text;
            }
        }

        protected static void DisplayElement(RegExElement element)
        {
            // As tree
            DisplayTree(element.ToTree());
            // Regex
            Debug.WriteLine(element);
        }

        protected static void DisplayTree(ITreeNodeContainer tree)
        {
            foreach (var line in tree.ToMultilineString())
            {
                Debug.WriteLine(line);
            }
        }
       protected static void DisplayTree<T>(ITreeNodeContainer<T> tree)
        {
            foreach (var line in tree.ToMultilineString())
            {
                Debug.WriteLine(line);
            }
        }

        protected Regex CreateRegex(RegExElement element)
        {
            return new Regex("^" + element + "$", RegexOptions.ExplicitCapture);
        }
    }
}