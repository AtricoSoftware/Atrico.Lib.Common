using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Atrico.Lib.Common.Collections
{
    public static class CollectionComparer
    {
        public static CollectionDifferences Differences(IEnumerable actual, IEnumerable expected)
        {
            if (ReferenceEquals(actual, null) || ReferenceEquals(expected, null))
            {
                return new CollectionDifferencesError();
            }
            if (actual is string && expected is string)
            {
                return new CollectionDifferencesError();
            }

            var actualA = actual.Cast<object>().ToArray();
            var expectedA = expected.Cast<object>().ToArray();

            // Check for equal
            if (actualA.Length == expectedA.Length && !actualA.Where((t, i) => !t.Equals(expectedA[i])).Any())
            {
                return new CollectionDifferencesEqual();
            }

            var extra = new List<object>(actualA);
            foreach (var item in expectedA)
            {
                extra.Remove(item);
            }
            var missing = new List<object>(expectedA);
            foreach (var item in actualA)
            {
                missing.Remove(item);
            }
            if (missing.Any() || extra.Any())
            {
                return new CollectionDifferencesWrongItems(actualA.Length - expectedA.Length, missing, extra);
            }

            // One out of order
            var outOfOrder = new HashSet<object>();
            for (uint count = 1; count <= (actualA.Length / 2); ++count)
            {
                foreach (var items in Permutations.Permutations.GetCombinations(expectedA, count))
                {
                    var actual2 = actualA.Except(items);
                    var expected2 = expectedA.Except(items).ToArray();
                    if (!actual2.Where((t, i) => !t.Equals(expected2[i])).Any())
                    {
                        outOfOrder.UnionWith(items);
                    }
                }
                if (outOfOrder.Any())
                {
                    return new CollectionDifferencesWrongOrder(outOfOrder);
                }
            }
            return new CollectionDifferencesError();
        }
    }
}
