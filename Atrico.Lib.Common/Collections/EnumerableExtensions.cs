using System;
using System.Collections.Generic;

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
    }
}
