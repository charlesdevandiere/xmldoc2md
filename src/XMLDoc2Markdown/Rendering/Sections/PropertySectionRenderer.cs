using System.Reflection;
using System.Xml.Linq;
using Markdown;
using XMLDoc2Markdown.Signatures;

namespace XMLDoc2Markdown.Rendering.Sections;

internal sealed class PropertySectionRenderer : MemberSectionRenderer<PropertyInfo>
{
    protected override string SectionHeader => "Properties";
    protected override string ObsoleteDefaultMessage => "This member is obsolete.";

    internal PropertySectionRenderer(
        RenderingContext context,
        XmlDocToMarkdownConverter converter,
        ExampleInjector examples)
        : base(context, converter, examples) { }

    protected override void RenderBody(IMarkdownDocument document, PropertyInfo member, XElement? memberDocElement)
    {
        document.AppendHeader("Property Value", 4);

        Type? returnType = member.GetReturnType();
        MarkdownInlineElement? typeName = returnType is null ? null : this.Context.DocsLink(returnType);
        IEnumerable<XNode> nodes = memberDocElement?.Element("value")?.Nodes() ?? [];
        MarkdownParagraph valueDoc = this.Converter.ToMarkdownParagraph(nodes);

        document.AppendParagraph($"{typeName}<br>{Environment.NewLine}{valueDoc}");
    }
}
