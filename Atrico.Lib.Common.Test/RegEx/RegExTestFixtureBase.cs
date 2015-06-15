using System.Diagnostics;
using Atrico.Lib.Common.RegEx.Elements;
using Atrico.Lib.Testing;

namespace Atrico.Lib.Common.Test.RegEx
{
    public class RegExTestFixtureBase : TestFixtureBase
    {
        protected static void DisplayElement(RegExElement element)
        {
            // As tree
            var tree = element.ToTree();
            foreach (var line in tree.ToMultilineString())
            {
                Debug.WriteLine(line);
            }
            // Regex
            Debug.WriteLine(element);
        }
    }
}