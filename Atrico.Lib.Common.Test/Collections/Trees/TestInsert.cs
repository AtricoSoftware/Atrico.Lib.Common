﻿using System.Linq;
using Atrico.Lib.Assertions;
using Atrico.Lib.Assertions.Constraints;
using Atrico.Lib.Assertions.Elements;
using Atrico.Lib.Common.Collections.Tree;
using Atrico.Lib.Testing.TestAttributes.NUnit;

namespace Atrico.Lib.Common.Tests.Collections.Trees
{
    [TestFixture]
    public class TestInsert : TreeTestFixtureBase
    {
        [Test]
        public void TestMultipleWithinTree()
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
            node12.Insert(0);

            // Assert
            Display(tree);
            var nodes = tree.GetNodes().ToArray();
            Assert.That(Value.Of(nodes).Count().Is().EqualTo(7), "Node count");
            Assert.That(Value.Of(nodes[0]).Is().EqualTo(new[] {1, 11}), "Node 11");
            Assert.That(Value.Of(nodes[1]).Is().EqualTo(new[] {1, 0, 12, 121}), "Node 121");
            Assert.That(Value.Of(nodes[2]).Is().EqualTo(new[] {1, 0, 12, 122}), "Node 122");
            Assert.That(Value.Of(nodes[3]).Is().EqualTo(new[] {1, 0, 12}), "Node 12");
            Assert.That(Value.Of(nodes[4]).Is().EqualTo(new[] {1, 0}), "Node 10");
            Assert.That(Value.Of(nodes[5]).Is().EqualTo(new[] {1, 13}), "Node 13");
            Assert.That(Value.Of(nodes[6]).Is().EqualTo(new[] {1}), "Node 1");
        }
    }

    [TestFixture]
    public class TestInsertT : TreeTestFixtureBase
    {
        [Test]
        public void TestMultipleWithinTree()
        {
            // Arrange
            var tree = Tree.Create<int>(true);
            var node1 = tree.Add(1);
            var node11 = node1.Add(11);
            var node12 = node1.Add(12);
            var node13 = node1.Add(13);
            var node121 = node12.Add(121);
            var node122 = node12.Add(122);

            // Act
            node12.Insert(0);

            // Assert
            Display(tree);
            var nodes = tree.GetNodes().ToArray();
            Assert.That(Value.Of(nodes).Count().Is().EqualTo(7), "Node count");
            Assert.That(Value.Of(nodes[0]).Is().EqualTo(new[] {1, 11}), "Node 11");
            Assert.That(Value.Of(nodes[1]).Is().EqualTo(new[] {1, 0, 12, 121}), "Node 121");
            Assert.That(Value.Of(nodes[2]).Is().EqualTo(new[] {1, 0, 12, 122}), "Node 122");
            Assert.That(Value.Of(nodes[3]).Is().EqualTo(new[] {1, 0, 12}), "Node 12");
            Assert.That(Value.Of(nodes[4]).Is().EqualTo(new[] {1, 0}), "Node 10");
            Assert.That(Value.Of(nodes[5]).Is().EqualTo(new[] {1, 13}), "Node 13");
            Assert.That(Value.Of(nodes[6]).Is().EqualTo(new[] {1}), "Node 1");
        }
    }
}
