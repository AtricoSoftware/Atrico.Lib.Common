using System.Linq;
using Atrico.Lib.Assertions;
using Atrico.Lib.Assertions.Constraints;
using Atrico.Lib.Assertions.Elements;
using Atrico.Lib.Common.Collections.Tree;
using Atrico.Lib.Testing.TestAttributes.NUnit;

namespace Atrico.Lib.Common.Test.Collections.Trees
{
    [TestFixture]
    public class TestAllowDuplicatesAdd : TreeTestFixtureBase
    {
        [Test]
        public void TestEmpty()
        {
            // Arrange

            // Act
            var tree = Tree.Create(true);

            // Assert
            Display(tree);
            Assert.That(Value.Of(tree.Parent).Is().Null(), "Tree parent");
            Assert.That(Value.Of(tree.Children).Count().Is().EqualTo(0), "Tree children count");
        }

        [Test]
        public void TestAddSingle()
        {
            // Arrange
            var tree = Tree.Create(true);

            // Act
            var node = tree.Add(1);

            // Assert
            Display(tree);
            Assert.That(Value.Of(tree.Parent).Is().Null(), "Tree parent");
            Assert.That(Value.Of(tree.Children).Count().Is().EqualTo(1), "Tree children count");
            Assert.That(Value.Of(tree.Children).Is().EqualTo(new[] {node}), "Tree children");
            Assert.That(Value.Of(node.Parent).Is().ReferenceEqualTo(tree), "Node parent");
            Assert.That(Value.Of(node.Children).Count().Is().EqualTo(0), "Node children count");
        }

        [Test]
        public void TestAddMultipleUnique()
        {
            // Arrange
            var tree = Tree.Create(true);

            // Act
            var node1 = tree.Add(1);
            var node11 = node1.Add(11);
            var node12 = node1.Add(12);
            var node13 = node1.Add(13);
            var node121 = node12.Add(121);
            var node122 = node12.Add(122);

            // Assert
            Display(tree);
            var nodes = tree.GetNodes().ToArray();
            Assert.That(Value.Of(nodes).Count().Is().EqualTo(6), "Node count");
            Assert.That(Value.Of(nodes[0]).Is().EqualTo(new[] {1, 11}), "Node 11");
            Assert.That(Value.Of(nodes[1]).Is().EqualTo(new[] {1, 12, 121}), "Node 121");
            Assert.That(Value.Of(nodes[2]).Is().EqualTo(new[] {1, 12, 122}), "Node 122");
            Assert.That(Value.Of(nodes[3]).Is().EqualTo(new[] {1, 12}), "Node 12");
            Assert.That(Value.Of(nodes[4]).Is().EqualTo(new[] {1, 13}), "Node 13");
            Assert.That(Value.Of(nodes[5]).Is().EqualTo(new[] {1}), "Node 1");
        }

        [Test]
        public void TestAddMultipleDuplicate()
        {
            // Arrange
            var tree = Tree.Create(true);

            // Act
            var node1 = tree.Add(1);
            var node1a = tree.Add(1);
            var node11 = node1.Add(11);
            var node12 = node1.Add(12);
            var node13 = node1.Add(13);
            var node13a = node1.Add(13);
            var node121 = node12.Add(121);
            var node122 = node12.Add(122);

            // Assert
            Display(tree);
            var nodes = tree.GetNodes().ToArray();
            Assert.That(Value.Of(nodes).Count().Is().EqualTo(8), "Node count");
            Assert.That(Value.Of(nodes[0]).Is().EqualTo(new[] {1, 11}), "Node 11");
            Assert.That(Value.Of(nodes[1]).Is().EqualTo(new[] {1, 12, 121}), "Node 121");
            Assert.That(Value.Of(nodes[2]).Is().EqualTo(new[] {1, 12, 122}), "Node 122");
            Assert.That(Value.Of(nodes[3]).Is().EqualTo(new[] {1, 12}), "Node 12");
            Assert.That(Value.Of(nodes[4]).Is().EqualTo(new[] {1, 13}), "Node 13");
            Assert.That(Value.Of(nodes[5]).Is().EqualTo(new[] {1, 13}), "Node 13a");
            Assert.That(Value.Of(nodes[6]).Is().EqualTo(new[] {1}), "Node 1");
            Assert.That(Value.Of(nodes[7]).Is().EqualTo(new[] {1}), "Node 1a");
        }
    }
}