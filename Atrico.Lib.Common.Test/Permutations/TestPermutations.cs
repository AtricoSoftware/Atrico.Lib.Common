using System.Collections.Generic;
using System.Linq;
using Atrico.Lib.Assertions;
using Atrico.Lib.Assertions.Constraints;
using Atrico.Lib.Assertions.Elements;
using Atrico.Lib.Testing.TestAttributes.NUnit;

namespace Atrico.Lib.Common.Test.Permutations
{
    [TestFixture]
    public class TestPermutations
    {
        [Test]
        public void TestEmptySet()
        {
            // Arrange
            var set = new HashSet<char>();

            // Act
            var combinations = Common.Permutations.Permutations.GetPermutations(set, 2);

            // Assert
            Assert.That(Value.Of(combinations).Count().Is().EqualTo(0));
        }

        [Test]
        public void TestNonEmptySet()
        {
            // Arrange
            var set = new HashSet<char>(new[] {'a', 'b', 'c', 'd'});

            // Act
            var combinations = Common.Permutations.Permutations.GetPermutations(set, 2);

            // Assert
            var result = combinations.Select(c => string.Format("{0}{1}", c[0], c[1]));
            Assert.That(Value.Of(result).Is().EquivalentTo(new[] {"ab", "ac", "ad", "bc", "bd", "cd", "ba", "ca", "da", "cb", "db", "dc"}));
        }
    }
}
