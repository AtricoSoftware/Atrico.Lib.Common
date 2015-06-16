using System.Collections.Generic;
using Atrico.Lib.Common.Collections.Tree;

namespace Atrico.Lib.Common.RegEx.Elements
{
    public abstract partial class RegExElement
    {
        private class RegExSequence : RegExComposite
        {
            public static RegExElement Create(IEnumerable<RegExElement> elements)
            {
                return Create(elements, CreateImpl);
            }

            public override RegExElement Simplify()
            {
                // TODO
                return this;
            }


            private static RegExSequence CreateImpl(IEnumerable<RegExElement> elements)
            {
                // Check for sequences
                //var groups = nonNull.PartitionBy(el => el);
                return new RegExSequence(elements);
            }

            private RegExSequence(IEnumerable<RegExElement> elements)
                : base(elements)
            {
            }

            public override string Separator
            {
                get { return ""; }
            }

            public override string StartGroup
            {
                get { return ""; }
            }

            public override string EndGroup
            {
                get { return ""; }
            }
            protected override void AddNodeToTree(Tree<string>.IModifiableNode root)
            {
                var thisNode = root.Add("AND");
                base.AddNodeToTree(thisNode);
            }
        }
    }
}