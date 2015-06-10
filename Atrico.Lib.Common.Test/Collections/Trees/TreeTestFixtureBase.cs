using System.Diagnostics;
using System.Linq;
using Atrico.Lib.Common.Collections.Tree;
using Atrico.Lib.Testing;

namespace Atrico.Lib.Common.Test.Collections.Trees
{
    public abstract class TreeTestFixtureBase : TestFixtureBase
    {
        protected static void Display<T>(Tree<T>.INode tree)
        {
            var lines = tree.ToMultilineString().ToArray();
            foreach (var line in lines) Debug.WriteLine(line);
        }
    }
}