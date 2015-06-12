using System.Collections.Generic;
using System.Linq;

namespace Atrico.Lib.Common
{
    /// <summary>
    ///     Comparer to compare lists by each item
    /// </summary>
    /// <typeparam name="T">Type of list item</typeparam>
    public class ListSequenceEqualityComparer<T> : IEqualityComparer<IEnumerable<T>>
    {
        public bool Equals(IEnumerable<T> x, IEnumerable<T> y)
        {
            return x.SequenceEqual(y);
        }

        public int GetHashCode(IEnumerable<T> obj)
        {
            var objA = obj as T[] ?? obj.ToArray();
            return objA.Any() ? objA.First().GetHashCode() : 0;
        }
    }
}