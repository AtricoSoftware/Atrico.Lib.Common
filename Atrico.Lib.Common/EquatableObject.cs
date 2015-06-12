using System;

namespace Atrico.Lib.Common
{
    /// <summary>
    /// Implements the fundamentals of equality
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class EquatableObject<T> : IEquatable<T>
        where T : EquatableObject<T>
    {
        public override bool Equals(object obj)
        {
            return Equals(obj as T);
        }

        public override int GetHashCode()
        {
            return GetHashCodeImpl();
        }

        public virtual bool Equals(T other)
        {
            return !ReferenceEquals(other, null) && GetType() == other.GetType() && EqualsImpl(other);
        }

        protected abstract int GetHashCodeImpl();
        protected abstract bool EqualsImpl(T other);
    }
}