using System;
using System.Text;

namespace Atrico.Lib.Common.Console
{
    public static class ConsoleUtils
    {
        public static string ReadPassword()
        {
            var sb = new StringBuilder();
            while (true)
            {
                var cki = System.Console.ReadKey(true);
                if (cki.Key == ConsoleKey.Enter)
                {
                    System.Console.WriteLine();
                    break;
                }

                if (cki.Key == ConsoleKey.Backspace)
                {
                    if (sb.Length > 0)
                    {
                        System.Console.Write("\b\0\b");
                        sb.Length--;
                    }

                    continue;
                }

                System.Console.Write('*');
                sb.Append(cki.KeyChar);
            }

            return sb.ToString();
        }
    }
}
