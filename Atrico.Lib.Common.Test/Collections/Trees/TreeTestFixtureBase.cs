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
                .Replace("+", Tree<string>.MidChildNode.ToString())
                .Replace("-", Tree<string>.Dash.ToString())
                .Replace("/", Tree<string>.FirstChildNode.ToString())
                .Replace("\\", Tree<string>.LastChildNode.ToString())
                .Replace("~", Tree<string>.SingleRoot.ToString())
                .Replace(">", Tree<string>.FirstOfDoubleRoot.ToString())
                .Replace("#", Tree<string>.MidRoot.ToString())
                .Replace("|", Tree<string>.VerticalLine.ToString())
                .Replace(" ", Tree<string>.Space.ToString());
        }

        protected static void Display<T>(Tree<T>.IModifiableNode tree)
        {
            var lines = tree.ToMultilineString().ToArray();
            foreach (var line in lines) Debug.WriteLine(line);
        }
    }
}