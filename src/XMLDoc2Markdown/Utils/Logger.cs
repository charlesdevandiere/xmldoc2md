using System;

namespace XMLDoc2Markdown.Utils
{
    internal class Logger
    {
        internal static void Info(string message)
        {
            Console.WriteLine(message);
        }

        internal static void Warning(string message)
        {
            ConsoleColor color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(message);
            Console.ForegroundColor = color;
        }

        internal static void Error(string message)
        {
            ConsoleColor color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(message);
            Console.ForegroundColor = color;
        }
    }
}
