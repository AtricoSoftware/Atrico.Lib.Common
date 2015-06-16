using System.Collections.Generic;
using System.Diagnostics;
using Atrico.Lib.Common.RegEx;
using Atrico.Lib.Common.RegEx.Elements;

namespace Atrico.Lib.Common.Test.ConsoleApp
{
    internal class Program
    {
        private static void DisplayElement(string title, RegExElement element)
        {
            System.Console.WriteLine(title);
            // As tree
            var tree = element.ToTree();
            foreach (var line in tree.ToMultilineString())
            {
                System.Console.WriteLine(line);
            }
            // Regex
            System.Console.WriteLine(element);
        }

        private static void Main(string[] args)
        {
            var nm = new RegExHelpers.NumberMatcher();
            //nm.AddRange(9, 11);
            //nm.AddRange(1, 31);
            nm.AddRange(123, 321);
            nm.AddRange(10000, 10001); // TODO - doesn't work with others
            //nm.AddRange(123, 199);
            //nm.AddRange(100, 399);
            DisplayElement("Tree", nm.Element);
        }
    }
}