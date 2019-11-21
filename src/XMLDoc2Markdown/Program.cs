using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
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

                var xmlDocumentation = new XmlDocumentation(src, namespaceMatch);

                if (!Directory.Exists(@out)) Directory.CreateDirectory(@out);

                var indexBuilder = new MarkdownBuilder();
                indexBuilder.Header(1, xmlDocumentation.AssemblyName);

                foreach (var g in xmlDocumentation.Types.GroupBy(x => x.Namespace).OrderBy(x => x.Key))
                {
                    string subDir = Path.Combine(@out, g.Key);
                    if (!Directory.Exists(subDir)) Directory.CreateDirectory(subDir);

                    indexBuilder.AppendLine();
                    indexBuilder.HeaderWithCode(2, g.Key);
                    indexBuilder.AppendLine();
                    foreach (var item in g.OrderBy(x => x.Name))
                    {
                        string typeName = item.BeautifyName.Replace("<", "{").Replace(">", "}").Replace(",", "").Replace(" ", "-");
                        var sb = new StringBuilder();

                        indexBuilder.ListLink(MarkdownBuilder.MarkdownCodeQuote(item.BeautifyName), g.Key + "/" + typeName);

                        sb.Append(item.ToString());

                        File.WriteAllText(Path.Combine(@out, g.Key, $"{typeName}.md"), sb.ToString());
                    }
                }

                File.WriteAllText(Path.Combine(@out, $"{indexPageName}.md"), indexBuilder.ToString());

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
