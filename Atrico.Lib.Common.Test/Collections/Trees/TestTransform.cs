using System.Collections.Generic;
using Atrico.Lib.Assertions;
using Atrico.Lib.Assertions.Constraints;
using Atrico.Lib.Assertions.Elements;
using Atrico.Lib.Common.Collections.Tree;
using Atrico.Lib.Testing.NUnitAttributes;

namespace Atrico.Lib.Common.Test.Collections.Trees
{
    [TestFixture]
    public class TestTransform : TreeTestFixtureBase
    {
        [Test]
        public void TestIdentityEmpty()
        {
            // Arrange
            var tree = Tree<int>.Create(true);

            // Act
            var newTree = tree.Transform(node=>node);

            // Assert
            Display(tree);
            Assert.That(Value.Of(newTree).Is().EqualTo(tree), "Identical copy");
            Assert.That(Value.Of(newTree).Is().Not().ReferenceEqualTo(tree), "Not the same object");
        }

        [Test]
        public void TestIdentityFromRoot()
        {
            // Arrange
            var tree = Tree<int>.Create(true);
            var node1 = tree.Add(1);
            var node11 = node1.Add(11);
            var node12 = node1.Add(12);
            var node13 = node1.Add(13);
            var node121 = node12.Add(121);
            var node122 = node12.Add(122);

            // Act
            var newTree = tree.Transform(node=>node);

            // Assert
            Display(tree);
            Assert.That(Value.Of(newTree).Is().EqualTo(tree), "Identical copy");
            Assert.That(Value.Of(newTree).Is().Not().ReferenceEqualTo(tree), "Not the same object");
        }

        [Test]
        public void TestIdentityFromMid()
        {
            // Arrange
            var tree = Tree<int>.Create(true);
            var node1 = tree.Add(1);
            var node11 = node1.Add(11);
            var node12 = node1.Add(12);
            var node13 = node1.Add(13);
            var node121 = node12.Add(121);
            var node122 = node12.Add(122);

            // Act
            var newNode = node12.Transform(node=>node);

            // Assert
            Display(tree);
            Assert.That(Value.Of(newNode).Is().EqualTo(node12), "Identical copy");
            Assert.That(Value.Of(newNode).Is().Not().ReferenceEqualTo(node12), "Not the same object");
        }
    }
}