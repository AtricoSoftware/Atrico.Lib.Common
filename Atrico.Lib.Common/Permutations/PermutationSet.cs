using System;
using System.Collections.Generic;

namespace Atrico.Lib.Common.Permutations
{
	// Permutations
	partial class Permutations
	{
		// Permutation set
		private class PermutationSet<T> : PermutationSetBase<T>
		{
			// Ctor
			public PermutationSet(IEnumerable<T> set, uint size)
				: base(set, size)
			{
			}

			protected override Func<T[], uint, ulong> CountFunction
			{
				get { return (items, size) => Factorials.Divide((uint) items.Length, ((uint) items.Length - size)); }
			}

			protected override Func<T[], uint, IEnumerator<T[]>> CreateEnumeratorFunction
			{
				get { return (items, size) => new PermutationEnumerator<T>(items, size); }
			}
		}

		// Permutation set enumerator
		private class PermutationEnumerator<T> : PermutationationEnumeratorBase<T>
		{
			public PermutationEnumerator(T[] @group, uint size)
				: base(@group, size)
			{
			}

			// Move to next permutation
			public override bool MoveNext()
			{
				// First permutation?
				var bFound = (-1 == CurrentIndex);
				if (bFound)
				{
					// Check for impossible set
					bFound = (Group.GetLength(0) > 0 && Group.GetLength(0) >= Size);
					if (bFound)
					{
						// Setup first perm
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
							CreateRemaining();
						}
					}
				}
				return bFound;
			}

			// Create remaining group
			private void CreateRemaining()
			{
				if (1 < Size)
				{
					// Strip current item
					var remaining = new T[Group.Length - 1];
					var dest = 0;
					for (var src = 0; src < Group.Length; ++src)
					{
						if (src != CurrentIndex)
						{
							remaining[dest++] = Group[src];
						}
					}
					// Create perm object
					Remaining = new PermutationEnumerator<T>(remaining, Size - 1);
					// Always move to next
					Remaining.MoveNext();
				}
			}
		}
	}
}
