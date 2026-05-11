using System.Reflection;
using System.Xml.Linq;
using Markdown;
using XMLDoc2Markdown.Signatures;

namespace XMLDoc2Markdown.Rendering.Sections;

internal sealed class IndexerSectionRenderer : MemberSectionRenderer<PropertyInfo>
{
    protected override string SectionHeader => "Indexers";
    protected override string ObsoleteDefaultMessage => "This member is obsolete.";

    internal IndexerSectionRenderer(
        RenderingContext context,
        XmlDocToMarkdownConverter converter,
        ExampleInjector examples)
        : base(context, converter, examples) { }

    protected override void RenderBody(IMarkdownDocument document, PropertyInfo member, XElement? memberDocElement)
    {
        ParameterInfo[] @params = member.GetIndexParameters();
        if (@params.Length > 0)
        {
            document.AppendHeader("Parameters", 4);

            foreach (ParameterInfo param in @params)
            {
                MarkdownInlineElement typeName = this.Context.DocsLink(param.ParameterType);
                IEnumerable<XNode> nodes = memberDocElement?
                    .Elements("param")
                    .FirstOrDefault(e => e.Attribute("name")?.Value == param.Name)
                    ?.Nodes() ?? [];
                MarkdownParagraph paramDoc = this.Converter.ToMarkdownParagraph(nodes);

                document.AppendParagraph($"{new MarkdownInlineCode(param.Name ?? string.Empty)} {typeName}<br>{Environment.NewLine}{paramDoc}");
            }
        }

        document.AppendHeader("Property Value", 4);
        Type? returnType = member.GetReturnType();
        MarkdownInlineElement? typeNameLink = returnType is null ? null : this.Context.DocsLink(returnType);
        IEnumerable<XNode> valueNodes = memberDocElement?.Element("value")?.Nodes() ?? [];
        MarkdownParagraph valueDoc = this.Converter.ToMarkdownParagraph(valueNodes);

        document.AppendParagraph($"{typeNameLink}<br>{Environment.NewLine}{valueDoc}");
    }
}
