using System.Text.RegularExpressions;
using System.Xml.Linq;
using Markdown;

namespace XMLDoc2Markdown.Rendering;

/// <summary>
/// Converts XML-doc inline elements (<c>&lt;summary&gt;</c>, <c>&lt;remarks&gt;</c>,
/// <c>&lt;see&gt;</c>, <c>&lt;code&gt;</c>, <c>&lt;list&gt;</c>, etc.) to Markdown.
/// All cref resolution is delegated to <see cref="CrefResolver"/>.
/// </summary>
internal sealed partial class XmlDocToMarkdownConverter
{
    [GeneratedRegex("[ ]{2,}")]
    private static partial Regex CollapseSpacesRegex();

    private readonly CrefResolver crefResolver;
    private readonly RenderingContext context;

    internal XmlDocToMarkdownConverter(RenderingContext context, CrefResolver crefResolver)
    {
        this.context = context;
        this.crefResolver = crefResolver;
    }

    internal MarkdownParagraph ToMarkdownParagraph(IEnumerable<XNode>? nodes)
    {
        if (nodes is null)
        {
            return new MarkdownParagraph(string.Empty);
        }

        List<IMarkdownBlockElement> blocks = [];
        MarkdownText? paragraph = null;

        foreach (XNode node in nodes)
        {
            object? element = this.NodeToMarkdown(node);
            if (element is null) continue;

            switch (element)
            {
                case MarkdownInlineElement inline:
                    if (paragraph is null) paragraph = new MarkdownText(inline);
                    else paragraph.Append(inline);
                    break;

                case IMarkdownBlockElement block:
                    if (paragraph is not null)
                    {
                        blocks.Add(new MarkdownParagraph(paragraph));
                        paragraph = null;
                    }
                    blocks.Add(block);
                    break;
            }
        }

        if (paragraph is not null)
        {
            blocks.Add(new MarkdownParagraph(paragraph));
        }

        return new MarkdownParagraph(string.Join(Environment.NewLine, blocks));
    }

    internal MarkdownInlineElement? LinkFromCref(string? cref, string? text = null)
    {
        string? effectiveText = text;

        if (string.IsNullOrEmpty(effectiveText))
        {
            effectiveText = CrefResolver.FormatDisplayName(cref);
        }

        if (this.crefResolver.TryResolve(cref, out System.Reflection.MemberInfo? memberInfo) && memberInfo is not null)
        {
            return this.context.DocsLink(memberInfo, effectiveText);
        }

        if (!string.IsNullOrEmpty(effectiveText))
        {
            return new MarkdownText(effectiveText);
        }

        // Fall back to the cref id itself. Strip the "T:" / "M:" / "!:" prefix and wrap in inline code
        // so backticks (XML doc generic-arity markers like ``1) survive markdown rendering.
        string fallback = cref is not null && cref.Length > 2 && cref[1] == ':'
            ? cref[2..]
            : (cref ?? string.Empty);

        return string.IsNullOrEmpty(fallback)
            ? new MarkdownText(string.Empty)
            : new MarkdownInlineCode(fallback);
    }

    private object? NodeToMarkdown(XNode node) => node switch
    {
        XText text => new MarkdownText(CollapseSpacesRegex().Replace(text.ToString(), " ")),
        XElement element => this.ElementToMarkdown(element),
        _ => null
    };

    private object? ElementToMarkdown(XElement element) => element.Name.ToString() switch
    {
        "see" => this.LinkFromCref(element.Attribute("cref")?.Value ?? element.Attribute("href")?.Value, element.Value),
        "seealso" => this.LinkFromCref(element.Attribute("cref")?.Value, element.Value),
        "c" => new MarkdownInlineCode(element.Value),
        "br" => new MarkdownText("<br>"),
        "para" => this.ToMarkdownParagraph(element.Nodes()),
        "example" => this.ToMarkdownParagraph(element.Nodes()),
        "code" => new MarkdownCode("csharp", FormatCodeBlock(element.Value)),
        "list" => this.ElementToMarkdownList(element),
        "paramref" => new MarkdownInlineCode(element.Attribute("name")?.Value ?? string.Empty),
        _ => new MarkdownText(element.Value)
    };

    private MarkdownList ElementToMarkdownList(XElement element)
    {
        MarkdownList list = element.Attribute("type")?.Value switch
        {
            "number" => new MarkdownOrderedList(),
            _ => new MarkdownList()
        };

        foreach (XElement item in element.Elements("item"))
        {
            MarkdownText listItem = new(string.Empty);

            MarkdownText? term = this.CollectInlineChildren(item.Element("term")?.Nodes());
            if (term is not null)
            {
                listItem.Append(new MarkdownStrongEmphasis(term));
            }

            MarkdownText? description = this.CollectInlineChildren(item.Element("description")?.Nodes());
            if (description is not null)
            {
                listItem.Append(" - ");
                listItem.Append(description);
            }

            list.AddItem(listItem);
        }

        return list;
    }

    private MarkdownText? CollectInlineChildren(IEnumerable<XNode>? nodes)
    {
        if (nodes is null)
        {
            return null;
        }

        MarkdownText? accumulator = null;
        foreach (XNode node in nodes)
        {
            if (this.NodeToMarkdown(node) is MarkdownInlineElement inline)
            {
                if (accumulator is null) accumulator = new MarkdownText(inline);
                else accumulator.Append(inline);
            }
        }
        return accumulator;
    }

    private static string FormatCodeBlock(string code)
    {
        if (string.IsNullOrEmpty(code))
        {
            return code;
        }

        code = code.TrimStart('\n');
        code = code.TrimEnd('\n', ' ');

        int indent = GetIndent(code);

        IEnumerable<string> lines = code
            .Split('\n')
            .Select(line => ReindentLine(line, indent));

        return string.Join(Environment.NewLine, lines);

        static int GetIndent(string value)
        {
            int indent = 0;
            foreach (char ch in value)
            {
                if (ch == ' ') indent++;
                else break;
            }
            return indent;
        }

        static string ReindentLine(string line, int indent)
        {
            int i;
            for (i = 0; i < indent && i < line.Length; i++)
            {
                if (line[i] != ' ') break;
            }
            return line[i..];
        }
    }
}
