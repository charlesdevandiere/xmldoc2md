using System.CommandLine;
using System.Reflection;
using Markdown;
using XMLDoc2Markdown;
using XMLDoc2Markdown.Utils;

Argument<string> srcArgument = new("src")
{
    Description = "DLL source path"
};

Option<string> outputOption = new("--output", "-o")
{
    Description = "Output directory"
};

Option<string> indexPageNameOption = new("--index-page-name")
{
    Description = "Name of the index page",
    DefaultValueFactory = _ => "index"
};

Option<string> examplesPathOption = new("--examples-path")
{
    Description = "Path to the code examples to insert in the documentation"
};

Option<bool> gitHubPagesOption = new("--github-pages")
{
    Description = "Remove '.md' extension from links for GitHub Pages"
};

Option<bool> gitlabWikiOption = new("--gitlab-wiki")
{
    Description = "Remove '.md' extension and './' prefix from links for gitlab wikis"
};

Option<bool> backButtonOption = new("--back-button")
{
    Description = "Add a back button on each page"
};

Option<string> memberAccessibilityLevelOption = new("--member-accessibility-level")
{
    Description = "Minimum accessibility level of members to be documented.",
    DefaultValueFactory = _ => "protected"
};
memberAccessibilityLevelOption.CompletionSources.Add("public", "protected", "internal", "private");

Option<string> structureOption = new("--structure")
{
    Description = "Documentation structure.",
    DefaultValueFactory = _ => "flat"
};
structureOption.CompletionSources.Add("flat", "tree");

RootCommand rootCommand = new(description: "Tool to generate markdown from C# XML documentation.")
{
    srcArgument,
    outputOption,
    indexPageNameOption,
    examplesPathOption,
    gitHubPagesOption,
    gitlabWikiOption,
    backButtonOption,
    memberAccessibilityLevelOption,
    structureOption
};

rootCommand.SetAction(parseResult =>
{
    try
    {
        string src = parseResult.GetValue(srcArgument)!;
        string @out = parseResult.GetValue(outputOption) ?? ".";
        string indexPageName = parseResult.GetValue(indexPageNameOption)!;
        TypeDocumentationOptions options = new()
        {
            ExamplesDirectory = parseResult.GetValue(examplesPathOption),
            GitHubPages = parseResult.GetValue(gitHubPagesOption),
            GitlabWiki = parseResult.GetValue(gitlabWikiOption),
            BackButton = parseResult.GetValue(backButtonOption),
            MemberAccessibilityLevel = parseResult.GetValue(memberAccessibilityLevelOption) switch
            {
                "private" => Accessibility.Private,
                "internal" => Accessibility.Internal,
                "protected" => Accessibility.Protected,
                _ => Accessibility.Public,
            },
            Structure = parseResult.GetValue(structureOption) switch
            {
                "tree" => DocumentationStructure.Tree,
                _ => DocumentationStructure.Flat,
            }
        };
        int succeeded = 0;
        int failed = 0;

        Assembly assembly = new AssemblyLoadContext(src)
            .LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(src)));

        string assemblyName = assembly.GetName().Name ?? Path.GetFileNameWithoutExtension(src);
        XmlDocumentation documentation = new(src);
        Logger.Info($"Generation started: Assembly: {assemblyName}");

        IMarkdownDocument indexPage = new MarkdownDocument().AppendHeader(assemblyName, 1);

        IEnumerable<Type> types = assembly.GetTypes()
            .Where(type => type.IsPublic && !typeof(Delegate).IsAssignableFrom(type));
        IEnumerable<IGrouping<string?, Type>> typesByNamespace = types.GroupBy(type => type.Namespace).OrderBy(g => g.Key);
        foreach (IGrouping<string?, Type> namespaceTypes in typesByNamespace)
        {
            indexPage.AppendHeader(namespaceTypes.Key ?? "No namespace", 2);

            foreach (Type type in namespaceTypes.OrderBy(x => x.Name))
            {
                string fileName = type.GetDocsFileName(options.Structure);
                Logger.Info($"  {fileName}.md");

                indexPage.AppendParagraph(type.GetDocsLink(assembly, options.Structure, noExtension: options.GitHubPages));

                try
                {
                    string filePath = Path.Combine(@out, $"{fileName}.md");
                    string? directory = Path.GetDirectoryName(filePath);

                    if (directory != null)
                    {
                        Directory.CreateDirectory(directory);
                    }

                    File.WriteAllText(
                        filePath,
                        new TypeDocumentation(assembly, type, documentation, options).ToString()
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
    }
    catch (Exception ex)
    {
        Logger.Error("Unable to generate documentation:");
        Logger.Error(ex.Message);
        return 1;
    }
});

return rootCommand.Parse(args).Invoke();
