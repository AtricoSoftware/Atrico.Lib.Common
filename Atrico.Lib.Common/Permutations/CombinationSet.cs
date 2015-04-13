using System;
using System.Collections.Generic;

namespace Atrico.Lib.Common.Permutations
{
	// Combinations
	partial class Permutations
	{
		// Combination set
		private class CombinationSet<T> : PermutationSetBase<T>
		{
			// Ctor
			public CombinationSet(IEnumerable<T> set, uint size)
				: base(set, size)
			{
			}

			protected override Func<T[], uint, ulong> CountFunction
			{
				get { return (items, size) => Factorials.Divide((uint) items.Length, ((uint) items.Length - size)) / Factorials.Calculate(size); }
			}

			protected override Func<T[], uint, IEnumerator<T[]>> CreateEnumeratorFunction
			{
				get { return (items, size) => new CombinationEnumerator<T>(items, size); }
			}
		}

		// Combination set enumerator
		private class CombinationEnumerator<T> : PermutationationEnumeratorBase<T>
		{
			public CombinationEnumerator(T[] @group, uint size)
				: base(@group, size)
			{
			}

			// Move to next Combination
			public override bool MoveNext()
			{
				// First Combination?
				var bFound = (-1 == CurrentIndex);
				if (bFound)
				{
					// Check for impossible set
					bFound = (Group.GetLength(0) > 0 && Group.GetLength(0) >= Size);
					if (bFound)
					{
						// Setup first com
						CurrentIndex = 0;
						CreateRemaining();
					}
				}
				else
				{
					// Move remaining
					bFound = (null != Remaining);
					if (bFound)
					{
						bFound = Remaining.MoveNext();
					}
					if (!bFound)
					{
						// Update this one
						bFound = (Group.Length > ++CurrentIndex);
						if (bFound)
						{
							bFound = CreateRemaining();
						}
					}
				}
				return bFound;
			}

			// Create remaining group
			private bool CreateRemaining()
			{
				var bRetVal = (1 >= Size);
				if (!bRetVal && Group.Length > (CurrentIndex + 1))
				{
					// Strip current item
					var remaining = new T[Group.Length - CurrentIndex - 1];
					var dest = 0;
					for (var src = CurrentIndex + 1; src < Group.Length; ++src)
					{
						remaining[dest++] = Group[src];
					}
					// Create perm object
					Remaining = new CombinationEnumerator<T>(remaining, Size - 1);
					// Always move to next
					bRetVal = Remaining.MoveNext();
				}
				return bRetVal;
			}
		}
	}
}
