using System.Reflection;
using System.Xml.Linq;
using Markdown;

namespace XMLDoc2Markdown.Rendering.Sections;

internal sealed class FieldSectionRenderer : MemberSectionRenderer<FieldInfo>
{
    protected override string SectionHeader => "Fields";
    protected override string ObsoleteDefaultMessage => "This member is obsolete.";

    internal FieldSectionRenderer(
        RenderingContext context,
        XmlDocToMarkdownConverter converter,
        ExampleInjector examples)
        : base(context, converter, examples) { }

    protected override void RenderBody(IMarkdownDocument document, FieldInfo member, XElement? memberDocElement)
    {
        // Fields have no body sub-sections beyond the inherited summary/signature.
    }
}
