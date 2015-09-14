using System.Linq;
using Atrico.Lib.Assertions;
using Atrico.Lib.Assertions.Constraints;
using Atrico.Lib.Assertions.Elements;
using Atrico.Lib.Common.Collections;
using Atrico.Lib.Testing.TestAttributes.NUnit;

namespace Atrico.Lib.Common.Tests.Collections
{
    [TestFixture]
    public class TestPartitionByExtension
    {
        [Test]
        public void TestPartitioning()
        {
            // Arrange
            var list = new[]
            {
                1,
                2,
                3, 5,
                6, 8,
                9,
                10
            };

            // Act
            var result = list.PartitionBy(item => (item % 2) == 0).ToArray(); // IsEven

            // Assert
            Assert.That(Value.Of(result).Count().Is().EqualTo(6), "Number of entries");
            Assert.That(Value.Of(result[0].Key).Is().False(), "0 = Odd");
            Assert.That(Value.Of(result[0]).Is().EqualTo(new[] {1}), "0 Items");
            Assert.That(Value.Of(result[1].Key).Is().True(), "1 = Even");
            Assert.That(Value.Of(result[1]).Is().EqualTo(new[] {2}), "1 Items");
            Assert.That(Value.Of(result[2].Key).Is().False(), "2 = Odd");
            Assert.That(Value.Of(result[2]).Is().EqualTo(new[] {3, 5}), "2 Items");
            Assert.That(Value.Of(result[3].Key).Is().True(), "3 = Even");
            Assert.That(Value.Of(result[3]).Is().EqualTo(new[] {6, 8}), "3 Items");
            Assert.That(Value.Of(result[4].Key).Is().False(), "4 = Odd");
            Assert.That(Value.Of(result[4]).Is().EqualTo(new[] {9}), "4 Items");
            Assert.That(Value.Of(result[5].Key).Is().True(), "5 = Even");
            Assert.That(Value.Of(result[5]).Is().EqualTo(new[] {10}), "5 Items");
        }

        [Test]
        public void TestEmptyList()
        {
            // Arrange
            var list = new int[] {};

            // Act
            var result = list.PartitionBy(item => Assert.Fail("Action called"));

            // Assert
            Assert.That(Value.Of(result).Count().Is().EqualTo(0));
        }
    }
}
