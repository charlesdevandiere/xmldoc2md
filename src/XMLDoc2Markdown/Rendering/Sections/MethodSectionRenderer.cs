using System.Reflection;
using System.Xml.Linq;
using Markdown;

namespace XMLDoc2Markdown.Rendering.Sections;

internal sealed class MethodSectionRenderer : MemberSectionRenderer<MethodInfo>
{
    private readonly MethodParametersRenderer parametersRenderer;

    protected override string SectionHeader => "Methods";
    protected override string ObsoleteDefaultMessage => "This member is obsolete.";

    internal MethodSectionRenderer(
        RenderingContext context,
        XmlDocToMarkdownConverter converter,
        ExampleInjector examples,
        MethodParametersRenderer parametersRenderer)
        : base(context, converter, examples)
    {
        this.parametersRenderer = parametersRenderer;
    }

    protected override void RenderBody(IMarkdownDocument document, MethodInfo member, XElement? memberDocElement)
    {
        this.parametersRenderer.WriteTypeParameters(document, member, memberDocElement);
        this.parametersRenderer.WriteParameters(document, member, memberDocElement);
        this.parametersRenderer.WriteReturns(document, member, memberDocElement);
    }
}
