using System.Collections.Generic;

namespace Atrico.Lib.Common.Permutations
{
	// Permutations and combinations
	public static partial class Permutations
	{
		// Get permutations
		public static IPermutations<T> GetPermutations<T>(ICollection<T> set)
		{
			return GetPermutations(set, (uint) set.Count);
		}

		public static IPermutations<T> GetPermutations<T>(IEnumerable<T> set, uint size)
		{
			// Create set of permutations
			return new PermutationSet<T>(set, size);
		}

		// Get combinations
		public static IPermutations<T> GetCombinations<T>(ICollection<T> set)
		{
			return GetCombinations(set, (uint) set.Count);
		}

		public static IPermutations<T> GetCombinations<T>(IEnumerable<T> set, uint size)
		{
			// Create set of combinations
			return new CombinationSet<T>(set, size);
		}
	}
}
