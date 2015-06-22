using System.Globalization;
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
            private readonly int _min;
            private readonly int? _max;

            public static RegExElement Create(RegExElement element, int min, int? max)
            {
                if (min < 1 || max < 1 || max < min) return null;
                if (min == 1 && (max ?? 0) == 1) return element;
                return new RegExRepeat(element, min, max);
            }

            private RegExRepeat(RegExElement element, int min, int? max)
            {
                _element = element;
                _min = min;
                _max = max;
            }

            #region Comparison

            protected override int GetHashCodeImpl()
            {
                return _min.GetHashCode() ^ _max.GetHashCode();
            }

            protected override bool EqualsImpl(RegExElement obj)
            {
                var other = obj as RegExRepeat;
                return !ReferenceEquals(other, null) && _min.Equals(other._min) && _max.Equals(other._max) && _element.Equals(other._element);
            }

            protected override int CompareToImpl(RegExElement obj)
            {
                var other = obj as RegExRepeat;
                if (ReferenceEquals(other, null)) return 1;
                var comp = _min.CompareTo(other._min);
                if (comp != 0) return comp;
                 comp = (_max ?? 0).CompareTo((other._max ??0));
               return comp != 0 ? comp : _element.CompareTo(other._element);
            }

            #endregion

            protected override void AddNodeToTree(ITreeNodeContainer root)
            {
                var thisNode = root.Add(string.Format("{0}x", CreateRepeatPart()));
                _element.AddNodeToTree(thisNode);
            }

            public override string ToString()
            {
                // Ensure resulting regex is smaller than non repeat alternative TODO - use min & max
                var len = _element.ToString().Length;
                var repeatPart = CreateRepeatPart();
                var canSimplify = _min == (_max ?? 0) && (len * _min) < len + repeatPart.Length;
                return canSimplify ? Enumerable.Repeat(_element.ToString(), _min).Aggregate(new StringBuilder(), (current, next) => current.Append(next)).ToString() : string.Format("{0}{1}", _element, repeatPart) ;
            }

            private string CreateRepeatPart()
            {
                var text = new StringBuilder();
                text.AppendFormat("{{{0}", _min);
                if (!_max.HasValue || _min != _max.Value)
                {
                    text.AppendFormat(",{0}", _max.HasValue ? _max.Value.ToString(CultureInfo.InvariantCulture) : "");
                }
                text.Append('}');
                return text.ToString();
            }
        }
    }

}