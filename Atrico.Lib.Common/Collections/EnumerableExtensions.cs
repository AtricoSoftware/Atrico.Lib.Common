using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Atrico.Lib.Common.Collections
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach (var item in list) action(item);
        }

        public static void ForEach<T>(this IEnumerable list, Action<object> action)
        {
            foreach (var item in list) action(item);
        }

        public static IEnumerable<IGrouping<TKey, TSource>> PartitionBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var result = new List<IGrouping<TKey, TSource>>();
            Grouping<TKey, TSource> current = null;
            foreach (var item in source)
            {
                var key = keySelector(item);
                if (current == null || !current.Key.Equals(key))
                {
                    current = new Grouping<TKey, TSource>(key);
                    result.Add(current);
                }
                current.Add(item);
            }
            return result;
        }

        public static int SequenceCompare<T>(this IEnumerable<T> list1, IEnumerable<T> list2) where T : IComparable
        {
            var enum1 = list1.GetEnumerator();
            var enum2 = list2.GetEnumerator();
            do
            {
                var more1 = enum1.MoveNext();
                var more2 = enum2.MoveNext();
                // both empty, must be equal
                if (!more1 && !more2) return 0;
                // 1 is shorter (1 lt 2)
                if (!more1) return -1;
                // 2 is shorter (1 gt 2)
                if (!more2) return 1;
                // Compare this element
                var comp = enum1.Current.CompareTo(enum2.Current);
                // Return if not equal, otherwise next element
                if (comp != 0) return comp;
            } while (true);
        }

        private class Grouping<TKey, TSource> : IGrouping<TKey, TSource>
        {
            private readonly IList<TSource> _items = new List<TSource>();

            public Grouping(TKey key)
            {
                Key = key;
            }

            public TKey Key { get; private set; }

            public void Add(TSource item)
            {
                _items.Add(item);
            }

            public IEnumerator<TSource> GetEnumerator()
            {
                return _items.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IEnumerable) _items).GetEnumerator();
            }
        }
    }
}
