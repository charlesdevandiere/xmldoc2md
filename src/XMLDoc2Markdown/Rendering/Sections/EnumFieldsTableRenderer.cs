using System.Reflection;
using System.Text;
using System.Xml.Linq;
using Markdown;

namespace XMLDoc2Markdown.Rendering.Sections;

internal sealed class EnumFieldsTableRenderer
{
    private readonly RenderingContext context;
    private readonly XmlDocToMarkdownConverter converter;

    internal EnumFieldsTableRenderer(RenderingContext context, XmlDocToMarkdownConverter converter)
    {
        this.context = context;
        this.converter = converter;
    }

    internal void Render(IMarkdownDocument document, IReadOnlyList<FieldInfo> fields)
    {
        if (fields.Count == 0)
        {
            return;
        }

        document.AppendHeader("Fields", 2);

        MarkdownTableHeader header = new(
            new MarkdownTableHeaderCell("Name"),
            new MarkdownTableHeaderCell("Value", MarkdownTableTextAlignment.Right),
            new MarkdownTableHeaderCell("Description"));

        MarkdownTable table = new(header, fields.Count);

        foreach (FieldInfo field in fields)
        {
            IEnumerable<XNode> nodes = this.context.Documentation.GetMember(field)?.Element("summary")?.Nodes() ?? [];
            MarkdownParagraph summary = this.converter.ToMarkdownParagraph(nodes);
            string formatted = EscapePipes(summary.ToString());

            string value = ((Enum)Enum.Parse(this.context.Type, field.Name)).ToString("D");
            table.AddRow(new MarkdownTableRow(field.Name, value, formatted));
        }

        document.Append(table);
    }

    private static string EscapePipes(string input)
    {
        input = input.Replace("\r\n", "\n");
        StringBuilder sb = new(input.Length);

        foreach (string line in input.Split('\n'))
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                sb.Append(line.Replace("|", "&#124;"));
            }
        }

        return sb.ToString();
    }
}
