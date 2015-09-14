using System.Collections.Generic;
using Atrico.Lib.Assertions;
using Atrico.Lib.Assertions.Constraints;
using Atrico.Lib.Assertions.Elements;
using Atrico.Lib.Common.Collections;
using Atrico.Lib.Testing.TestAttributes.NUnit;

namespace Atrico.Lib.Common.Tests.Collections
{
    [TestFixture]
    public class TestForeachExtension
    {
        [Test]
        public void TestEachElement()
        {
            // Arrange
            var list = new[] {1, 2, 3};
            var result = new List<int>();

            // Act
            list.ForEach(item => result.Add(item * 2));

            // Assert
            Assert.That(Value.Of(result).Is().EqualTo(new object[] {2, 4, 6}));
        }

        [Test]
        public void TestEmptyList()
        {
            // Arrange
            var list = new int[] {};

            // Act
            list.ForEach(item => Assert.Fail("Action called"));
        }
    }
}
