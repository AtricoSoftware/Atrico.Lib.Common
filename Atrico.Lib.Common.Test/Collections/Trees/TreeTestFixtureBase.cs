using System.Diagnostics;
using System.Linq;
using Atrico.Lib.Common.Collections.Tree;
using Atrico.Lib.Testing;

namespace Atrico.Lib.Common.Test.Collections.Trees
{
    public abstract class TreeTestFixtureBase : TestFixtureBase
    {
        protected static string ConvertAscii(string ascii)
        {
            return ascii
                .Replace("+", Tree.MidChildNode.ToString())
                .Replace("-", Tree.Dash.ToString())
                .Replace("/", Tree.FirstChildNode.ToString())
                .Replace("\\", Tree.LastChildNode.ToString())
                .Replace("~", Tree.SingleRoot.ToString())
                .Replace(">", Tree.FirstOfDoubleRoot.ToString())
                .Replace("#", Tree.MidRoot.ToString())
                .Replace("|", Tree.VerticalLine.ToString())
                .Replace(" ", Tree.Space.ToString());
        }

        protected static void Display(ITreeNode tree)
        {
            var lines = tree.ToMultilineString().ToArray();
            foreach (var line in lines) Debug.WriteLine(line);
        }
    }
}