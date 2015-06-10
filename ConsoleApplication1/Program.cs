using System;
using System.Collections.Generic;

namespace ConsoleApplication1
{
    internal class Program
    {
        private static void Display(string title, IEnumerable<string> lines)
        {
            Console.WriteLine(title);
            Console.WriteLine(new string('~', title.Length));
            foreach (var line in lines) Console.WriteLine(line);
            Console.WriteLine();
        }

        private static void Main(string[] args)
        {
            var nm = new NumberMatcher();
            nm.AddRange(9, 11);
            Display("Tree", nm.ToMultilineString());
            //nm.AddRange(123, 321);
            //nm.AddRange(100, 399);
            //var regex = nm.GetRegex();
            //Console.WriteLine(regex);
        }
    }
}