﻿using System;
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
            //nm.AddRange(9, 11);
            //nm.AddRange(1, 31);
            //nm.AddRange(123, 321);
            nm.AddRange(10000, 10001); // TODO - doesn't work with others
            //nm.AddRange(123, 199);
            //nm.AddRange(100, 399);
            Display("Tree", nm.ToMultilineString());
            var regex = nm.GetRegex();
            Console.WriteLine(regex);
        }
    }
}