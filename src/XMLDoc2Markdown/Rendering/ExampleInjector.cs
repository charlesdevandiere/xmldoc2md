using System.Reflection;
using Markdown;
using XMLDoc2Markdown.Utils;
using XMLDoc2Markdown.XmlDocId;

namespace XMLDoc2Markdown.Rendering;

internal sealed class ExampleInjector
{
    private readonly string? examplesDirectory;

    internal ExampleInjector(string? examplesDirectory)
    {
        this.examplesDirectory = examplesDirectory;
    }

    internal bool Inject(IMarkdownDocument document, MemberInfo memberInfo)
    {
        if (this.examplesDirectory is null)
        {
            return false;
        }

        string file = Path.Combine(this.examplesDirectory, $"{memberInfo.GetIdentifier()}.md");
        if (!File.Exists(file))
        {
            return false;
        }

        try
        {
            using StreamReader reader = new(file);
            document.Append(new MarkdownParagraph(reader.ReadToEnd()));
            return true;
        }
        catch (IOException e)
        {
            Logger.Warning(e.Message);
            return false;
        }
    }
}
