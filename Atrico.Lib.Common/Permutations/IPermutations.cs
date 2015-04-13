using System.Collections.Generic;

namespace Atrico.Lib.Common.Permutations
{
	public interface IPermutations<T> : IEnumerable<T[]>
	{
		// Number of permutations (or Combinations)
		ulong Count { get; }
	}
}
