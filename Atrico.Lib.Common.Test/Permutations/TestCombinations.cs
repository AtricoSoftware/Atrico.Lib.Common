using System.Collections.Generic;
using System.Linq;
using Atrico.Lib.Assertions;
using Atrico.Lib.Testing.NUnitAttributes;

namespace Atrico.Lib.Common.Test.Permutations
{
	[TestFixture]
	public class TestCombinations
	{
		[Test]
		public void TestEmptySet()
		{
			// Arrange
			var set = new HashSet<char>();

			// Act
			var combinations = Common.Permutations.Permutations.GetCombinations(set, 2);

			// Assert
			Assert.That(combinations, AsCollection.Count.Is.EqualTo(0));
		}

		[Test]
		public void TestNonEmptySet()
		{
			// Arrange
			var set = new HashSet<char>(new[] {'a', 'b', 'c', 'd'});

			// Act
			var combinations = Common.Permutations.Permutations.GetCombinations(set, 2);

			// Assert
			Assert.That(combinations.Select(c => string.Format("{0}{1}", c[0], c[1])), AsCollection.Is.EquivalentTo(new[] {"ab", "ac", "ad", "bc", "bd", "cd"}));
		}
	}
}
