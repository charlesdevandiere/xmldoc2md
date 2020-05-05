using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Markdown;
using Microsoft.Extensions.CommandLineUtils;

namespace XMLDoc2Markdown
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = new CommandLineApplication();
            app.Name = "xmldoc2md";

            app.VersionOption("-v|--version", () =>
            {
                return string.Format(
                    "Version {0}",
                    Assembly.GetEntryAssembly()
                        .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                        .InformationalVersion
                        .ToString());
            });
            app.HelpOption("-?|-h|--help");

            CommandArgument srcArg = app.Argument("src", "DLL source path");
            CommandArgument outArg = app.Argument("out", "Output directory");

            CommandOption namespaceMatchOption = app.Option(
                "--namespace-match <regex>",
                "Regex pattern to select namespaces",
                CommandOptionType.SingleValue);

            CommandOption indexPageNameOption = app.Option(
                "--index-page-name <regex>",
                "Name of the index page (default: \"index\")",
                CommandOptionType.SingleValue);

            app.OnExecute(() =>
            {
                string src = srcArg.Value;
                string @out = outArg.Value;
                string namespaceMatch = namespaceMatchOption.Value();
                string indexPageName = indexPageNameOption.HasValue() ? indexPageNameOption.Value() : "index";

                if (!Directory.Exists(@out))
                {
                    Directory.CreateDirectory(@out);
                }

                var assembly = Assembly.LoadFrom(src);
                var documentation = new XmlDocumentation(src);

                IMarkdownDocument indexPage = new MarkdownDocument().AppendHeader(assembly.GetName().Name, 1);

                foreach (IGrouping<string, Type> g in assembly.GetTypes().GroupBy(x => x.Namespace).OrderBy(x => x.Key))
                {
                    string subDir = Path.Combine(@out, g.Key);
                    if (!Directory.Exists(subDir))
                    {
                        Directory.CreateDirectory(subDir);
                    }

                    indexPage.AppendHeader(new MarkdownInlineCode(g.Key), 2);

                    var list = new MarkdownList();
                    foreach (Type type in g.OrderBy(x => x.Name))
                    {
                        string beautifyName = Beautifier.BeautifyType(type);
                        string typeName = beautifyName.Replace("<", "{").Replace(">", "}").Replace(",", "").Replace(" ", "-");

                        list.AddItem(new MarkdownLink(new MarkdownInlineCode(beautifyName), g.Key + "/" + typeName));

                        File.WriteAllText(Path.Combine(@out, g.Key, $"{typeName}.md"), new TypeDocumentation(type, documentation).ToString());
                    }

                    indexPage.Append(list);
                }

                File.WriteAllText(Path.Combine(@out, $"{indexPageName}.md"), indexPage.ToString());

                return 0;
            });

            try
            {
                app.Execute(args);
            }
            catch (CommandParsingException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to execute application: {0}", ex.Message);
            }
        }
    }
}
