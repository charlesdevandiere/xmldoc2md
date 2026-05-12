using System.Reflection;
using System.Xml.Linq;
using Markdown;
using XMLDoc2Markdown.Signatures;
using XMLDoc2Markdown.Signatures.Modifiers;

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
            Type linkType = param.ParameterType.IsByRef
                ? param.ParameterType.GetElementType()!
                : param.ParameterType;
            DisplayMeta meta = this.BuildParameterMeta(param);
            string typeDisplay = this.RenderTypeForProse(linkType, meta);

            string? passing = ParameterListModifier.GetPassingModifier(param);
            string passingPrefix = passing == null ? string.Empty : $"`{passing}` ";

            IEnumerable<XNode> nodes = memberDocElement?
                .Elements("param")
                .FirstOrDefault(e => e.Attribute("name")?.Value == param.Name)
                ?.Nodes() ?? [];
            MarkdownParagraph paramDoc = this.converter.ToMarkdownParagraph(nodes);

            document.AppendParagraph($"{passingPrefix}{new MarkdownInlineCode(param.Name ?? string.Empty)} {typeDisplay}<br>{Environment.NewLine}{paramDoc}");
        }
    }

    internal void WriteReturns(IMarkdownDocument document, MethodInfo methodInfo, XElement? memberDocElement)
    {
        if (methodInfo.ReturnType == typeof(void))
        {
            return;
        }

        document.AppendHeader("Returns", 4);

        DisplayMeta returnMeta = DisplayMeta.ForReturn(methodInfo, this.context.Nullability);
        string typeDisplay = this.RenderTypeForProse(methodInfo.ReturnType, returnMeta);
        IEnumerable<XNode> nodes = memberDocElement?.Element("returns")?.Nodes() ?? [];
        MarkdownParagraph paragraph = this.converter.ToMarkdownParagraph(nodes);

        document.AppendParagraph($"{typeDisplay}<br>{Environment.NewLine}{paragraph}");
    }

    private DisplayMeta BuildParameterMeta(ParameterInfo param)
    {
        DisplayMeta meta = DisplayMeta.For(param, this.context.Nullability);
        return param.ParameterType.IsByRef ? meta.ForElement() : meta;
    }

    /// <summary>
    /// Renders a type for inline prose. Tuples are emitted as inline code
    /// (no link — the named tuple syntax does not point at a single docs page).
    /// Reference types receive a trailing <c>?</c> when nullable. All other
    /// types fall through to the existing <see cref="RenderingContext.DocsLink(Type, string?)"/>
    /// path so cross-references continue to work.
    /// </summary>
    private string RenderTypeForProse(Type type, DisplayMeta meta)
    {
        if (type.IsValueTuple())
        {
            return new MarkdownInlineCode(type.GetDisplayName(meta, simplifyName: false)).ToString();
        }

        MarkdownInlineElement link = this.context.DocsLink(type);
        bool isNullableReference = !type.IsValueType
            && !type.IsGenericParameter
            && meta.Nullability is { ReadState: NullabilityState.Nullable };
        return isNullableReference ? $"{link}?" : (link.ToString() ?? string.Empty);
    }
}
