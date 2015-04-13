using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Atrico.Lib.Common.Permutations
{
	partial class Permutations
	{
		private abstract class PermutationSetBase<T> : IPermutations<T>
		{
			// Set of items
			private readonly T[] _items;
			// Number of items in Combination
			private readonly uint _size;

			protected PermutationSetBase(IEnumerable<T> set, uint size)
			{
				_items = set.ToArray();
				_size = size;
			}

			public IEnumerator<T[]> GetEnumerator()
			{
				return CreateEnumeratorFunction(_items, _size);
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}

			public ulong Count
			{
				get
				{
					if (_items.Length == 0 || _size > _items.Length)
					{
						return 0L;
					}
					return CountFunction(_items, _size);
				}
			}

			protected abstract Func<T[], uint, ulong> CountFunction { get; }
			protected abstract Func<T[], uint, IEnumerator<T[]>> CreateEnumeratorFunction { get; }
		}

		// Enumerator
		private abstract class PermutationationEnumeratorBase<T> : IEnumerator<T[]>
		{
			// Original group
			protected readonly T[] Group;
			// Size of Combination (r)
			protected readonly uint Size;
			// Current position of first item
			protected int CurrentIndex;
			// Remaining items
			protected PermutationationEnumeratorBase<T> Remaining;
			// Ctor
			protected PermutationationEnumeratorBase(T[] @group, uint size)
			{
				Group = @group;
				Size = size;
				// Setup first Combination
				Reset();
			}

			public abstract bool MoveNext();

			public void Reset()
			{
				CurrentIndex = -1;
			}

			public T[] Current
			{
				get
				{
					// Set first item
					var perm = new List<T> {Group[CurrentIndex]};
					// Get remaining
					if (null != Remaining)
					{
						perm.AddRange(Remaining.Current);
					}
					return perm.ToArray();
				}
			}

			object IEnumerator.Current
			{
				get { return Current; }
			}

			public void Dispose()
			{
				// Nothing to do
			}
		}
	}
}
