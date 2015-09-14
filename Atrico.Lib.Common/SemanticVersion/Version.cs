using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atrico.Lib.Common.SemanticVersion
{
    /// <summary>
    /// Represents a semantic version
    /// </summary>
    public class Version : IEquatable<Version>, IComparable<Version>, IComparable
    {
        private readonly int[] _values;

        /// <summary>
        /// Create a version from a list of integers (with at least 1)
        /// </summary>
        /// <param name="value">First value (mandatory)</param>
        /// <param name="values">Successive values (optional)</param>
        /// <returns>New Version</returns>
        public static Version From(int value, params int[] values)
        {
            return new Version(new[] {value}.Concat(values));
        }

        /// <summary>
        /// Create a version from a string in the form a.b.c....
        /// </summary>
        /// <param name="text">String representing version</param>
        /// <returns>New Version</returns>
        public static Version From(string text)
        {
            if (ReferenceEquals(text, null))
            {
                throw new ArgumentException("Cannot be null", "text");
            }
            var parts = text.Split('.').Select(int.Parse);
            return new Version(parts);
        }

        public static Version From(System.Version version)
        {
            return From(version.Major, version.Minor, version.Build, version.Revision);
        }

        public int[] GetValues(int min = 0, int max = int.MaxValue)
        {
            if (min > max)
            {
                throw new ArgumentException("min must be smaller than max");
            }
            var values = new List<int>(_values);
            if (values.Count < min)
            {
                values.AddRange(Enumerable.Repeat(0, min - values.Count));
            }
            if (values.Count > max)
            {
                values.RemoveRange(max, values.Count - max);
            }
            return values.ToArray();
        }

        private Version(IEnumerable<int> values)
        {
            _values = values.Reverse().SkipWhile(i => i == 0).Reverse().ToArray();
        }

        #region Equality

        public override bool Equals(object obj)
        {
            return Equals(obj as Version);
        }

        public bool Equals(Version other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            if (_values.Length != other._values.Length)
            {
                return false;
            }
            return !_values.Where((t, i) => t != other._values[i]).Any();
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return _values.Aggregate((hash, val) => (hash * 397) ^ val.GetHashCode());
            }
        }

        #endregion

        #region Comparison

        public int CompareTo(object obj)
        {
            return CompareTo(obj as Version);
        }

        public int CompareTo(Version other)
        {
            if (ReferenceEquals(other, null))
            {
                return 1; // null smaller
            }
            var min = Math.Max(_values.Length, other._values.Length);
            return String.Compare(ToString(min), other.ToString(min), StringComparison.Ordinal);
        }

        #endregion

        public override string ToString()
        {
            return ToString(0);
        }

        public string ToString(int min, int max = int.MaxValue)
        {
            return GetValues(min, max).Aggregate(new StringBuilder(), (current, val) => current.AppendFormat("{0}.", val)).ToString().TrimEnd('.');
        }
    }
}