using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Atrico.Lib.Common.Collections
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach (var item in list)
            {
                action(item);
            }
            return list;
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
