using System.Reflection;
using System.Xml.Linq;
using Markdown;

namespace XMLDoc2Markdown.Rendering.Sections;

internal sealed class OperatorSectionRenderer : MemberSectionRenderer<MethodInfo>
{
    private readonly MethodParametersRenderer parametersRenderer;

    protected override string SectionHeader => "Operators";
    protected override string ObsoleteDefaultMessage => "This member is obsolete.";

    internal OperatorSectionRenderer(
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
        this.parametersRenderer.WriteParameters(document, member, memberDocElement);
        this.parametersRenderer.WriteReturns(document, member, memberDocElement);
    }
}
