using System;
using System.Diagnostics;
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

            protected override void AddNodeToTree(Tree<string>.IModifiableNode root)
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
            var tree = element.ToTree();
            foreach (var line in tree.ToMultilineString())
            {
                Debug.WriteLine(line);
            }
            // Regex
            Debug.WriteLine(element);
        }
    }
}