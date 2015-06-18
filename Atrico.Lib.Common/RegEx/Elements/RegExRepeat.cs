using System;
using System.Linq;
using System.Text;
using Atrico.Lib.Common.Collections.Tree;

namespace Atrico.Lib.Common.RegEx.Elements
{
    public abstract partial class RegExElement
    {
        private class RegExRepeat : RegExElement
        {
            private readonly RegExElement _element;
            private readonly int _repeats;

            public static RegExElement Create(RegExElement element, int repeats)
            {
                return repeats < 1 ? null : repeats == 1 ? element : new RegExRepeat(element, repeats);
            }

            private RegExRepeat(RegExElement element, int repeats)
            {
                _element = element;
                _repeats = repeats;
            }

            #region Comparison

            protected override int GetHashCodeImpl()
            {
                return _repeats.GetHashCode();
            }

            protected override bool EqualsImpl(RegExElement obj)
            {
                var other = obj as RegExRepeat;
                return !ReferenceEquals(other, null) && _repeats.Equals(other._repeats) && _element.Equals(other._element);
            }

            protected override int CompareToImpl(RegExElement obj)
            {
                var other = obj as RegExRepeat;
                if (ReferenceEquals(other, null)) return 1;
                var comp = _repeats.CompareTo(other._repeats);
                return comp != 0 ? comp : _element.CompareTo(other._element);
            }

            #endregion

            protected override void AddNodeToTree(Tree<string>.IModifiableNode root)
            {
                var thisNode = root.Add(string.Format("{0}x", _repeats));
                _element.AddNodeToTree(thisNode);
            }

            public override string ToString()
            {
                // Ensure resulting regex is smaller than non repeat alternative
                var len = _element.ToString().Length;
                return (len * _repeats) < len + 3 ? Enumerable.Repeat(_element.ToString(), _repeats).Aggregate(new StringBuilder(), (current, next)=>current.Append(next)).ToString() : string.Format("{0}{{{1}}}", _element, _repeats);
            }
        }
    }
}