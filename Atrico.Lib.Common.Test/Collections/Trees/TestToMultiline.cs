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
    }
}