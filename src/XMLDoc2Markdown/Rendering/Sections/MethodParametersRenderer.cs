using System.Reflection;
using System.Xml.Linq;
using Markdown;

namespace XMLDoc2Markdown.Rendering.Sections;

/// <summary>
/// Renders the "#### Type Parameters", "#### Parameters", "#### Returns" sub-sections.
/// Shared by methods and constructors (returns is method-only).
/// </summary>
internal sealed class MethodParametersRenderer
{
    private readonly RenderingContext context;
    private readonly XmlDocToMarkdownConverter converter;

    internal MethodParametersRenderer(RenderingContext context, XmlDocToMarkdownConverter converter)
    {
        this.context = context;
        this.converter = converter;
    }

    internal void WriteTypeParameters(IMarkdownDocument document, MemberInfo memberInfo, XElement? memberDocElement)
    {
        Type[] typeParams = memberInfo switch
        {
            TypeInfo typeInfo => typeInfo.GenericTypeParameters,
            MethodInfo methodInfo => methodInfo.GetGenericArguments(),
            _ => []
        };

        if (typeParams.Length == 0)
        {
            return;
        }

        document.AppendHeader("Type Parameters", 4);

        foreach (Type typeParam in typeParams)
        {
            MarkdownInlineElement typeName = this.context.DocsLink(typeParam);
            IEnumerable<XNode> nodes = memberDocElement?
                .Elements("typeparam")
                .FirstOrDefault(e => e.Attribute("name")?.Value == typeParam.Name)
                ?.Nodes() ?? [];
            MarkdownParagraph paragraph = this.converter.ToMarkdownParagraph(nodes);

            document.AppendParagraph(string.Join($"<br>{Environment.NewLine}", new MarkdownInlineCode(typeName), paragraph));
        }
    }

    internal void WriteParameters(IMarkdownDocument document, MethodBase methodBase, XElement? memberDocElement)
    {
        ParameterInfo[] @params = methodBase.GetParameters();
        if (@params.Length == 0)
        {
            return;
        }

        document.AppendHeader("Parameters", 4);

        foreach (ParameterInfo param in @params)
        {
            MarkdownInlineElement typeName = this.context.DocsLink(param.ParameterType);
            IEnumerable<XNode> nodes = memberDocElement?
                .Elements("param")
                .FirstOrDefault(e => e.Attribute("name")?.Value == param.Name)
                ?.Nodes() ?? [];
            MarkdownParagraph paramDoc = this.converter.ToMarkdownParagraph(nodes);

            document.AppendParagraph($"{new MarkdownInlineCode(param.Name ?? string.Empty)} {typeName}<br>{Environment.NewLine}{paramDoc}");
        }
    }

    internal void WriteReturns(IMarkdownDocument document, MethodInfo methodInfo, XElement? memberDocElement)
    {
        if (methodInfo.ReturnType == typeof(void))
        {
            return;
        }

        document.AppendHeader("Returns", 4);

        MarkdownInlineElement typeName = this.context.DocsLink(methodInfo.ReturnType);
        IEnumerable<XNode> nodes = memberDocElement?.Element("returns")?.Nodes() ?? [];
        MarkdownParagraph paragraph = this.converter.ToMarkdownParagraph(nodes);

        document.AppendParagraph($"{typeName}<br>{Environment.NewLine}{paragraph}");
    }
}
