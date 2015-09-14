using System.Diagnostics;
using System.Linq;
using Atrico.Lib.Common.Collections.Tree;
using Atrico.Lib.Common.Collections.Tree.Implementation;
using Atrico.Lib.Testing;

namespace Atrico.Lib.Common.Tests.Collections.Trees
{
    public abstract class TreeTestFixtureBase : TestFixtureBase
    {
        protected static string ConvertAscii(string ascii)
        {
            return ascii
                .Replace("+", Node.MidChildNode.ToString())
                .Replace("-", Node.Dash.ToString())
                .Replace("/", Node.FirstChildNode.ToString())
                .Replace("\\", Node.LastChildNode.ToString())
                .Replace("~", Node.SingleRoot.ToString())
                .Replace(">", Node.FirstOfDoubleRoot.ToString())
                .Replace("#", Node.MidRoot.ToString())
                .Replace("|", Node.VerticalLine.ToString())
                .Replace(" ", Node.Space.ToString());
        }

        protected static void Display(ITreeNodeContainer tree)
        {
            var lines = tree.ToMultilineString().ToArray();
            foreach (var line in lines) Debug.WriteLine(line);
        }
        protected static void Display<T>(ITreeNodeContainer<T> tree)
        {
            var lines = tree.ToMultilineString().ToArray();
            foreach (var line in lines) Debug.WriteLine(line);
        }
    }
}