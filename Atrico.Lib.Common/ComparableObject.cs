using System;

namespace Atrico.Lib.Common
{
    /// <summary>
    /// Implements the fundamentals of comparison (and equality)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ComparableObject<T> : EquatableObject<T>, IComparable, IComparable<T> where T : ComparableObject<T>
    {
        public int CompareTo(object obj)
        {
            return CompareTo(obj as T);
        }

        public int CompareTo(T other)
        {
            // Null always smallest
            return ReferenceEquals(other, null) ? 1 : CompareToImpl(other);
        }

        public static bool operator <(ComparableObject<T> x, ComparableObject<T> y)
        {
            return x.CompareTo(y) < 0;
        }

        public static bool operator >(ComparableObject<T> x, ComparableObject<T> y)
        {
            return x.CompareTo(y) > 0;
        }

        public static bool operator <=(ComparableObject<T> x, ComparableObject<T> y)
        {
            return x.CompareTo(y) <= 0;
        }

        public static bool operator >=(ComparableObject<T> x, ComparableObject<T> y)
        {
            return x.CompareTo(y) >= 0;
        }

        protected abstract int CompareToImpl(T other);
    }
}