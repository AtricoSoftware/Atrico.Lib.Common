﻿using System;

namespace ConsoleApplication1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var nm = new NumberMatcher();
            nm.AddRange(123, 321);
            //foreach (var line in nm.Display())
            //{
            //    Console.WriteLine(line);
            //}
            var regex = nm.GetRegex();
            Console.WriteLine(regex);
        }
    }
}