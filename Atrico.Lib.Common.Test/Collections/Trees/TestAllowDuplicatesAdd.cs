using Atrico.Lib.Assertions;
using Atrico.Lib.Assertions.Constraints;
using Atrico.Lib.Assertions.Elements;
using Atrico.Lib.Common.Collections.Tree;
using Atrico.Lib.Testing.NUnitAttributes;

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
            var tree = Tree<string>.Create(true);

            // Assert
            Display(tree);
            Assert.That(Value.Of(tree.Parent).Is().Null(), "Tree parent");
            Assert.That(Value.Of(tree.Children).Count().Is().EqualTo(0), "Tree children count");
        }

        [Test]
        public void TestAddSingle()
        {
            // Arrange
            var tree = Tree<string>.Create(true);

            // Act
            var node = tree.Add("node");

            // Assert
            Display(tree);
            Assert.That(Value.Of(tree.Parent).Is().Null(), "Tree parent");
            Assert.That(Value.Of(tree.Children).Count().Is().EqualTo(1), "Tree children count");
            Assert.That(Value.Of(tree.Children).Is().EqualTo(new[] {node}), "Tree children");
            Assert.That(Value.Of(node.Parent).Is().ReferenceEqualTo(tree), "Node parent");
            Assert.That(Value.Of(node.Children).Count().Is().EqualTo(0), "Node children count");
        }
    }
}