using System.Diagnostics;
using System.Linq;
using Atrico.Lib.Assertions;
using Atrico.Lib.Assertions.Constraints;
using Atrico.Lib.Assertions.Elements;
using Atrico.Lib.Common.Collections.Tree;
using Atrico.Lib.Testing;
using Atrico.Lib.Testing.NUnitAttributes;

namespace Atrico.Lib.Common.Test.Collections.Trees
{
    [TestFixture]
    public class TestToMultiline : TestFixtureBase
    {
        [Test]
        public void TestSingleNode()
        {
            // Arrange
            var tree = new Tree<string>();
            tree.Add("root");

            // Act
            var lines = tree.ToMultilineString().ToArray();

            // Assert
            foreach (var line in lines) Debug.WriteLine(line);
            Assert.That(Value.Of(lines).Count().Is().EqualTo(1), "Number of lines");
            Assert.That(Value.Of(lines[0]).Is().EqualTo("+-root"), "Root node");
        }

        [Test]
        public void TestOneNodeSingleBranch()
        {
            // Arrange
            var tree = new Tree<string>();
            var root = tree.Add("root");
            root.Add("branch");

            // Act
            var lines = tree.ToMultilineString().ToArray();

            // Assert
            foreach (var line in lines) Debug.WriteLine(line);
            Assert.That(Value.Of(lines).Count().Is().EqualTo(2), "Number of lines");
            Assert.That(Value.Of(lines[0]).Is().EqualTo("+-root"), "Root");
            Assert.That(Value.Of(lines[1]).Is().EqualTo("  +-branch"), "Branch");
        }

        [Test]
        public void TestMultipleSingleBranches()
        {
            // Arrange
            var tree = new Tree<string>();
            var root = tree.Add("root");
            var one = root.Add("one");
            var two = one.Add("two");
            two.Add("three");

            // Act
            var lines = tree.ToMultilineString().ToArray();

            // Assert
            foreach (var line in lines) Debug.WriteLine(line);
            Assert.That(Value.Of(lines).Count().Is().EqualTo(4), "Number of lines");
            Assert.That(Value.Of(lines[0]).Is().EqualTo("+-root"), "Root");
            Assert.That(Value.Of(lines[1]).Is().EqualTo("  +-one"), "One");
            Assert.That(Value.Of(lines[2]).Is().EqualTo("    +-two"), "Two");
            Assert.That(Value.Of(lines[3]).Is().EqualTo("      +-three"), "Three");
        }

        [Test]
        public void TestOneNodeDoubleBranch()
        {
            // Arrange
            var tree = new Tree<string>();
            var root = tree.Add("root");
            root.Add("one");
            root.Add("two");

            // Act
            var lines = tree.ToMultilineString().ToArray();

            // Assert
            foreach (var line in lines) Debug.WriteLine(line);
            Assert.That(Value.Of(lines).Count().Is().EqualTo(3), "Number of lines");
            Assert.That(Value.Of(lines[0]).Is().EqualTo("  +-one"), "One");
            Assert.That(Value.Of(lines[1]).Is().EqualTo("+-root"), "Root");
            Assert.That(Value.Of(lines[2]).Is().EqualTo("  +-two"), "Two");
        }

        [Test]
        public void TestMultipleBranches()
        {
            // Arrange
            var tree = new Tree<string>();
            var root = tree.Add("root");
            var one = root.Add("one");
            one.Add("one1");
            one.Add("one2");
            var two = root.Add("two");
            two.Add("two1");
            two.Add("two2");

            // Act
            var lines = tree.ToMultilineString().ToArray();

            // Assert
            foreach (var line in lines) Debug.WriteLine(line);
            Assert.That(Value.Of(lines).Count().Is().EqualTo(7), "Number of lines");
            Assert.That(Value.Of(lines[0]).Is().EqualTo("    +-one1"), "One1");
            Assert.That(Value.Of(lines[1]).Is().EqualTo("  +-one"), "One");
            Assert.That(Value.Of(lines[2]).Is().EqualTo("  | +-one2"), "One2");
            Assert.That(Value.Of(lines[3]).Is().EqualTo("+-root"), "Root");
            Assert.That(Value.Of(lines[4]).Is().EqualTo("  | +-two1"), "Two1");
            Assert.That(Value.Of(lines[5]).Is().EqualTo("  +-two"), "Two");
            Assert.That(Value.Of(lines[6]).Is().EqualTo("    +-two2"), "Two2");
        }

        [Test]
        public void TestMultipleRoots()
        {
            // Arrange
            var tree = new Tree<string>();
            var root1 = tree.Add("root1");
            var one1 = root1.Add("one1");
            var two1 = one1.Add("two1");
            two1.Add("three1");
            var root2 = tree.Add("root2");
            var one2 = root2.Add("one2");
            var two2 = one2.Add("two2");
            two2.Add("three2");

            // Act
            var lines = tree.ToMultilineString().ToArray();

            // Assert
            foreach (var line in lines) Debug.WriteLine(line);
            Assert.That(Value.Of(lines).Count().Is().EqualTo(8), "Number of lines");
            Assert.That(Value.Of(lines[0]).Is().EqualTo("+-root1"), "Root1");
            Assert.That(Value.Of(lines[1]).Is().EqualTo("| +-one1"), "One1");
            Assert.That(Value.Of(lines[2]).Is().EqualTo("| | +-two1"), "Two1");
            Assert.That(Value.Of(lines[3]).Is().EqualTo("| |   +-three1"), "Three1");
            Assert.That(Value.Of(lines[4]).Is().EqualTo("+-root2"), "Root2");
            Assert.That(Value.Of(lines[5]).Is().EqualTo("  +-one2"), "One2");
            Assert.That(Value.Of(lines[6]).Is().EqualTo("    +-two2"), "Two2");
            Assert.That(Value.Of(lines[7]).Is().EqualTo("      +-three2"), "Three2");
        }

        [Test]
        public void TestAllOverThePlace()
        {
            // Arrange
            var tree = new Tree<string>();
            var root1 = tree.Add("root1");
            var one1 = root1.Add("one1");
            var one11 = one1.Add("one11");
            var one12 = one1.Add("one12");
            var one2 = root1.Add("one2");
            var one21 = one2.Add("one21");
            var root2 = tree.Add("root2");

            // Act
            var lines = tree.ToMultilineString().ToArray();

            // Assert
            foreach (var line in lines) Debug.WriteLine(line);
            Assert.That(Value.Of(lines).Count().Is().EqualTo(7), "Number of lines");
            Assert.That(Value.Of(lines[0]).Is().EqualTo("    +-one11"), "One11");
            Assert.That(Value.Of(lines[1]).Is().EqualTo("  +-one1"), "One1");
            Assert.That(Value.Of(lines[2]).Is().EqualTo("  | +-one12"), "One12");
            Assert.That(Value.Of(lines[3]).Is().EqualTo("+-root1"), "Root1");
            Assert.That(Value.Of(lines[4]).Is().EqualTo("| +-one2"), "One2");
            Assert.That(Value.Of(lines[5]).Is().EqualTo("| | +-one21"), "One21");
            Assert.That(Value.Of(lines[6]).Is().EqualTo("+-root2"), "Root2");
        }
    }
}