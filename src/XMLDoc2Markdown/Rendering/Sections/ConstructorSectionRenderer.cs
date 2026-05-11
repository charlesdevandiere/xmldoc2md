using System.Reflection;
using System.Xml.Linq;
using Markdown;

namespace XMLDoc2Markdown.Rendering.Sections;

internal sealed class ConstructorSectionRenderer : MemberSectionRenderer<ConstructorInfo>
{
    private readonly MethodParametersRenderer parametersRenderer;

    protected override string SectionHeader => "Constructors";
    protected override string ObsoleteDefaultMessage => "This member is obsolete.";

    internal ConstructorSectionRenderer(
        RenderingContext context,
        XmlDocToMarkdownConverter converter,
        ExampleInjector examples,
        MethodParametersRenderer parametersRenderer)
        : base(context, converter, examples)
    {
        this.parametersRenderer = parametersRenderer;
    }

    protected override void RenderBody(IMarkdownDocument document, ConstructorInfo member, XElement? memberDocElement)
    {
        this.parametersRenderer.WriteTypeParameters(document, member, memberDocElement);
        this.parametersRenderer.WriteParameters(document, member, memberDocElement);
    }
}
