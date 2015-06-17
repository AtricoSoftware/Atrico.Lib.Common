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
            private readonly IComparable _value;

            public TestElement(int value = 0)
            {
                _value = value;
            }
            public TestElement(char value)
            {
                _value = value;
            }

            protected override int GetHashCodeImpl()
            {
                return _value.GetHashCode();
            }

            protected override bool EqualsImpl(RegExElement obj)
            {
                var other = obj as TestElement;
                return !ReferenceEquals(other, null) && _value.Equals(other._value);
            }

            protected override int CompareToImpl(RegExElement obj)
            {
                var other = obj as TestElement;
                return ReferenceEquals(other, null) ? 1 : _value.CompareTo(other._value);
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
                return string.Format("test{0}", _value);
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