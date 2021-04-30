using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Markdown;
using Microsoft.Extensions.CommandLineUtils;
using XMLDoc2Markdown.Utils;

namespace XMLDoc2Markdown
{
    class Program
    {
        static int Main(string[] args)
        {
            var app = new CommandLineApplication
            {
                Name = "xmldoc2md"
            };

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

            CommandOption indexPageNameOption = app.Option(
                "--index-page-name",
                "Name of the index page (default: \"index\")",
                CommandOptionType.SingleValue);

            CommandOption examplesPathOption = app.Option(
                "--examples-path",
                "Path to the code examples to insert in the documentation",
                CommandOptionType.SingleValue);

            CommandOption gitHubPagesOption = app.Option(
                "--github-pages",
                "Remove '.md' extension from links for GitHub Pages",
                CommandOptionType.NoValue);

            app.OnExecute(() =>
            {
                string src = srcArg.Value;
                string @out = outArg.Value;
                string indexPageName = indexPageNameOption.Value() ?? "index";
                string examplesPath = examplesPathOption.Value();
                bool githubPages = gitHubPagesOption.HasValue();

                int succeeded = 0;
                int failed = 0;

                if (!Directory.Exists(@out))
                {
                    Directory.CreateDirectory(@out);
                }

                var assembly = Assembly.LoadFrom(src);
                string assemblyName = assembly.GetName().Name;
                var documentation = new XmlDocumentation(src);

                Logger.Info($"Generation started: Assembly: {assemblyName}");

                IMarkdownDocument indexPage = new MarkdownDocument().AppendHeader(assemblyName, 1);

                IEnumerable<Type> types = assembly.GetTypes().Where(type => type.IsPublic);
                IEnumerable<IGrouping<string, Type>> typesByNamespace = types.GroupBy(type => type.Namespace).OrderBy(g => g.Key);
                foreach (IGrouping<string, Type> namespaceTypes in typesByNamespace)
                {
                    indexPage.AppendHeader(namespaceTypes.Key, 2);

                    foreach (Type type in namespaceTypes.OrderBy(x => x.Name))
                    {
                        // exclude delegates
                        if (typeof(Delegate).IsAssignableFrom(type))
                        {
                            continue;
                        }

                        string fileName = type.GetDocsFileName();
                        Logger.Info($"  {fileName}.md");

                        indexPage.AppendParagraph(type.GetDocsLink(assembly, noExtension: githubPages));

                        try
                        {
                            File.WriteAllText(
                                Path.Combine(@out, $"{fileName}.md"),
                                new TypeDocumentation(assembly, type, documentation, examplesPath, githubPages).ToString()
                            );
                            succeeded++;
                        }
                        catch (Exception exception)
                        {
                            Logger.Error(exception.Message);
                            failed++;
                        }
                    }
                }

                File.WriteAllText(Path.Combine(@out, $"{indexPageName}.md"), indexPage.ToString());

                Logger.Info($"Generation: {succeeded} succeeded, {failed} failed");

                return 0;
            });

            try
            {
                return app.Execute(args);
            }
            catch (CommandParsingException ex)
            {
                Logger.Error(ex.Message);
            }
            catch (Exception ex)
            {
                Logger.Error("Unable to generate documentation:");
                Logger.Error(ex.Message);
            }

            return 1;
        }
    }
}
