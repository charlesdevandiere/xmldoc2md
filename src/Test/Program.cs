using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Markdown;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var document = new MarkdownDocument();
            document.AppendHeader("StringBuilder", 1);

            Constructor(document);
            Console.WriteLine(document);

            string path = Path.Combine(Directory.GetCurrentDirectory(), "document.md");
            using var outputFile = new StreamWriter(path);

            outputFile.Write(document);
        }

    }
}
