using System.Collections.Generic;
using Atrico.Lib.Assertions;
using Atrico.Lib.Assertions.Constraints;
using Atrico.Lib.Assertions.Elements;
using Atrico.Lib.Common.Collections.Tree;
using Atrico.Lib.Testing.TestAttributes.NUnit;

namespace Atrico.Lib.Common.Tests.Collections.Trees
{
    [TestFixture]
    public class TestBreadthFirst : TreeTestFixtureBase
    {
        [Test]
        public void TestEmpty()
        {
            // Arrange
            var tree = Tree.Create(true);
            var list = new List<string>();

            // Act
            tree.BreadthFirst(el => list.Add(el.ToString()));

            // Assert
            Display(tree);
            Assert.That(Value.Of(list).Count().Is().EqualTo(0), "Nothing in list");
        }

        [Test]
        public void TestFromRoot()
        {
            // Arrange
            var tree = Tree.Create(true);
            var node1 = tree.Add(1);
            var node11 = node1.Add(11);
            var node12 = node1.Add(12);
            var node13 = node1.Add(13);
            var node121 = node12.Add(121);
            var node122 = node12.Add(122);
            var list = new List<string>();

            // Act
            tree.BreadthFirst(el => list.Add(el.Data.ToString()));

            // Assert
            Display(tree);
            Assert.That(Value.Of(list).Is().EqualTo(new[] {"1", "11", "12", "13", "121", "122"}), "List has entry per node");
        }

        [Test]
        public void TestFromMid()
        {
            // Arrange
            var tree = Tree.Create(true);
            var node1 = tree.Add(1);
            var node11 = node1.Add(11);
            var node12 = node1.Add(12);
            var node13 = node1.Add(13);
            var node121 = node12.Add(121);
            var node122 = node12.Add(122);
            var list = new List<string>();

            // Act
            node12.BreadthFirst(el => list.Add(el.Data.ToString()));

            // Assert
            Display(tree);
            Assert.That(Value.Of(list).Is().EqualTo(new[] {"12", "121", "122"}), "List has entry per node");
        }
    }

    [TestFixture]
    public class TestBreadthFirstT : TreeTestFixtureBase
    {
        [Test]
        public void TestEmpty()
        {
            // Arrange
            var tree = Tree.Create<int>(true);
            var list = new List<string>();

            // Act
            tree.BreadthFirst(el => list.Add(el.ToString()));

            // Assert
            Display(tree);
            Assert.That(Value.Of(list).Count().Is().EqualTo(0), "Nothing in list");
        }

        [Test]
        public void TestFromRoot()
        {
            // Arrange
            var tree = Tree.Create<int>(true);
            var node1 = tree.Add(1);
            var node11 = node1.Add(11);
            var node12 = node1.Add(12);
            var node13 = node1.Add(13);
            var node121 = node12.Add(121);
            var node122 = node12.Add(122);
            var list = new List<string>();

            // Act
            tree.BreadthFirst(el => list.Add(el.Data.ToString()));

            // Assert
            Display(tree);
            Assert.That(Value.Of(list).Is().EqualTo(new[] {"1", "11", "12", "13", "121", "122"}), "List has entry per node");
        }

        [Test]
        public void TestFromMid()
        {
            // Arrange
            var tree = Tree.Create<int>(true);
            var node1 = tree.Add(1);
            var node11 = node1.Add(11);
            var node12 = node1.Add(12);
            var node13 = node1.Add(13);
            var node121 = node12.Add(121);
            var node122 = node12.Add(122);
            var list = new List<string>();

            // Act
            node12.BreadthFirst(el => list.Add(el.Data.ToString()));

            // Assert
            Display(tree);
            Assert.That(Value.Of(list).Is().EqualTo(new[] {"12", "121", "122"}), "List has entry per node");
        }
    }
}
