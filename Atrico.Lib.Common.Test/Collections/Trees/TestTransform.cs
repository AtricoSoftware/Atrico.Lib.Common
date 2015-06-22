using System.Linq;
using Atrico.Lib.Assertions;
using Atrico.Lib.Assertions.Constraints;
using Atrico.Lib.Assertions.Elements;
using Atrico.Lib.Common.Collections.Tree;
using Atrico.Lib.Testing.TestAttributes.NUnit;

namespace Atrico.Lib.Common.Test.Collections.Trees
{
    [TestFixture]
    public class TestTransform : TreeTestFixtureBase
    {
        private static ITreeNode Identity(ITreeNode node)
        {
            return node;
        }

        private static ITreeNode ReverseString(ITreeNode node)
        {
            if (!ReferenceEquals(node.Data, null)) node.Data = (int)node.Data * 10;
            return node;
        }
        private static ITreeNode MergeChildren(ITreeNode node)
        {
            if (!node.IsRoot())
            {
            var leafData = node.Children.Where(ch => ch.IsLeaf()).Select(n=>n.Data).ToArray();
            if (leafData.Count() > 1)
            {
                node.Remove(leafData[0]);
                node.Remove(leafData[1]);
                node.Add((int)leafData[0]+(int)leafData[1]);
            }
                
            }
            return node;
        }

        [Test]
        public void TestIdentityEmpty()
        {
            // Arrange
            var tree = Tree.Create(true);

            // Act
            var newTree = tree.Transform(Identity);

            // Assert
            Display(tree);
            Assert.That(Value.Of(newTree).Is().EqualTo(tree), "Identical copy");
            Assert.That(Value.Of(newTree).Is().Not().ReferenceEqualTo(tree), "Not the same object");
        }

        [Test]
        public void TestIdentityFromRoot()
        {
            // Arrange
            var tree = Tree.Create(true);
            var node1 = tree.Add(1);
            var node11 = node1.Add(11);
            var node12 = node1.Add(12);
            var node13 = node1.Add(13);
            var node121 = node12.Add(121);
            var node122 = node12.Add(122);

            // Act
            var newTree = tree.Transform(Identity);

            // Assert
            Display(tree);
            Assert.That(Value.Of(newTree).Is().EqualTo(tree), "Identical copy");
            Assert.That(Value.Of(newTree).Is().Not().ReferenceEqualTo(tree), "Not the same object");
        }

        [Test]
        public void TestIdentityFromMid()
        {
            // Arrange
            var tree = Tree.Create(true);
            var node1 = tree.Add(1);
            var node11 = node1.Add(11);
            var node12 = node1.Add(12);
            var node13 = node1.Add(13);
            var node121 = node12.Add(121);
            var node122 = node12.Add(122);

            // Act
            var newNode = node12.Transform(Identity);

            // Assert
            Display(tree);
            Assert.That(Value.Of(newNode).Is().EqualTo(node12), "Identical copy");
            Assert.That(Value.Of(newNode).Is().Not().ReferenceEqualTo(node12), "Not the same object");
        }

        [Test]
        public void TestChangeDataEmpty()
        {
            // Arrange
            var tree = Tree.Create(true);

            // Act
            var newTree = tree.Transform(ReverseString);

            // Assert
            Display(tree);
            Assert.That(Value.Of(newTree).Is().EqualTo(tree), "Identical copy");
            Assert.That(Value.Of(newTree).Is().Not().ReferenceEqualTo(tree), "Not the same object");
        }

        [Test]
        public void TestChangeDataFromRoot()
        {
            // Arrange
            var tree = Tree.Create(true);
            var node1 = tree.Add(1);
            var node11 = node1.Add(11);
            var node12 = node1.Add(12);
            var node13 = node1.Add(13);
            var node121 = node12.Add(121);
            var node122 = node12.Add(122);

            // Act
            var newTree = tree.Transform(ReverseString);

            // Assert
            Display(newTree);
            var nodes = newTree.GetNodes().ToArray();
            Assert.That(Value.Of(nodes).Count().Is().EqualTo(6), "Node count");
            Assert.That(Value.Of(nodes[0]).Is().EqualTo(new[] {10, 110}), "Node 11");
            Assert.That(Value.Of(nodes[1]).Is().EqualTo(new[] {10, 120, 1210}), "Node 121");
            Assert.That(Value.Of(nodes[2]).Is().EqualTo(new[] {10, 120, 1220}), "Node 122");
            Assert.That(Value.Of(nodes[3]).Is().EqualTo(new[] {10, 120}), "Node 12");
            Assert.That(Value.Of(nodes[4]).Is().EqualTo(new[] {10, 130}), "Node 13");
            Assert.That(Value.Of(nodes[5]).Is().EqualTo(new[] {10}), "Node 1");
        }

        [Test]
        public void TestChangeDataFromMid()
        {
            // Arrange
            var tree = Tree.Create(true);
            var node1 = tree.Add(1);
            var node11 = node1.Add(11);
            var node12 = node1.Add(12);
            var node13 = node1.Add(13);
            var node121 = node12.Add(121);
            var node122 = node12.Add(122);

            // Act
            var newNode = node12.Transform(ReverseString);

            // Assert
            Display(newNode);
            var nodes = newNode.GetNodes().ToArray();
            Assert.That(Value.Of(nodes).Count().Is().EqualTo(3), "Node count");
            Assert.That(Value.Of(nodes[0]).Is().EqualTo(new[] {120, 1210}), "Node 121");
            Assert.That(Value.Of(nodes[1]).Is().EqualTo(new[] {120, 1220}), "Node 122");
            Assert.That(Value.Of(nodes[2]).Is().EqualTo(new[] {120}), "Node 12");
        }
        [Test]
        public void TestChangeNodesEmpty()
        {
            // Arrange
            var tree = Tree.Create(true);

            // Act
            var newTree = tree.Transform(MergeChildren);

            // Assert
            Display(tree);
            Assert.That(Value.Of(newTree).Is().EqualTo(tree), "Identical copy");
            Assert.That(Value.Of(newTree).Is().Not().ReferenceEqualTo(tree), "Not the same object");
        }

        [Test]
        public void TestChangeNodesFromRoot()
        {
            // Arrange
            var tree = Tree.Create(true);
            var node1 = tree.Add(1);
            var node11 = node1.Add(11);
            var node12 = node1.Add(12);
            var node13 = node1.Add(13);
            var node121 = node12.Add(121);
            var node122 = node12.Add(122);

            // Act
            var newTree = tree.Transform(MergeChildren);

            // Assert
            Display(newTree);
            var nodes = newTree.GetNodes().ToArray();
            Assert.That(Value.Of(nodes).Count().Is().EqualTo(4), "Node count");
            Assert.That(Value.Of(nodes[0]).Is().EqualTo(new[] {1, 12, 243}), "Node 121");
            Assert.That(Value.Of(nodes[1]).Is().EqualTo(new[] {1, 12}), "Node 12");
            Assert.That(Value.Of(nodes[2]).Is().EqualTo(new[] {1, 24}), "Node 11");
            Assert.That(Value.Of(nodes[3]).Is().EqualTo(new[] {1}), "Node 1");
        }

        [Test]
        public void TestChangeNodesFromMid()
        {
            // Arrange
            var tree = Tree.Create(true);
            var node1 = tree.Add(1);
            var node11 = node1.Add(11);
            var node12 = node1.Add(12);
            var node13 = node1.Add(13);
            var node121 = node12.Add(121);
            var node122 = node12.Add(122);

            // Act
            var newNode = node12.Transform(MergeChildren);

            // Assert
            Display(newNode);
            var nodes = newNode.GetNodes().ToArray();
            Assert.That(Value.Of(nodes).Count().Is().EqualTo(2), "Node count");
            Assert.That(Value.Of(nodes[0]).Is().EqualTo(new[] {12, 243}), "Node 121");
            Assert.That(Value.Of(nodes[1]).Is().EqualTo(new[] {12}), "Node 12");
        }
    }
}