using Atrico.Lib.Common.Collections.Tree;

namespace Atrico.Lib.Common.RegEx.Elements
{
    public abstract partial class RegExElement
    {
        private sealed class RegExTerminator : RegExElement
        {
            #region Comparison

            protected override int GetHashCodeImpl()
            {
                return typeof (RegExTerminator).GetHashCode();
            }

            protected override bool EqualsImpl(RegExElement other)
            {
                return other is RegExTerminator;
            }

            protected override int CompareToImpl(RegExElement other)
            {
                return other is RegExTerminator ? 0 : 1;
            }

            #endregion

            protected override void AddNodeToTree(Tree<string>.IModifiableNode root)
            {
                root.Add("$");
            }
        }
    }
}