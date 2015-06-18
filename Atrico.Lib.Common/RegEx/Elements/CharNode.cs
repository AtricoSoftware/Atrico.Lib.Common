using System;
using Atrico.Lib.Common.Collections;

namespace Atrico.Lib.Common.RegEx.Elements
{
    public abstract partial class RegExElement
    {
        public class CharNode : ComparableObject<CharNode>
        {
            public char Char { get; private set; }
            public bool IsTerminator { get; set; }

            public CharNode(char c)
            {
                Char = c;
                IsTerminator = false;
            }

            #region Comparable

            protected override int GetHashCodeImpl()
            {
                return Char.GetHashCode() ^ IsTerminator.GetHashCode();
            }

            protected override bool EqualsImpl(CharNode other)
            {
                return IsTerminator.Equals(other.IsTerminator) && Char.Equals(other.Char);
            }

            protected override int CompareToImpl(CharNode other)
            {
                return new IComparable[] { Char, IsTerminator }.SequenceCompare(new IComparable[] { other.Char, other.IsTerminator });
            }

            #endregion

            public override string ToString()
            {
                return string.Format("{0}{1}", Char, IsTerminator ? "($)" : "");
            }
        }
    }
}