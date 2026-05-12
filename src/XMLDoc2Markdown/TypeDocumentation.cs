using System.Reflection;
using System.Xml.Linq;
using Markdown;
using XMLDoc2Markdown.Rendering;
using XMLDoc2Markdown.Rendering.Sections;
using XMLDoc2Markdown.Signatures;
using XMLDoc2Markdown.Utils;

namespace XMLDoc2Markdown;

/// <summary>
/// Top-level orchestrator. Wires the rendering components together and produces
/// the Markdown for a single .NET type. All real work is delegated:
/// <list type="bullet">
///   <item><description><see cref="Signatures"/> builds C# signatures.</description></item>
///   <item><description><see cref="XmlDocId"/> builds XML doc identifiers.</description></item>
///   <item><description><see cref="Linking"/> builds intra-doc / MSDN links.</description></item>
///   <item><description><see cref="Rendering"/> renders sections and converts XML doc nodes.</description></item>
/// </list>
/// </summary>
internal sealed class TypeDocumentation
{
    private readonly RenderingContext context;
    private readonly MarkdownDocument document = new();

    internal TypeDocumentation(Assembly assembly, Type type, XmlDocumentation documentation, TypeDocumentationOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(assembly);
        ArgumentNullException.ThrowIfNull(type);
        ArgumentNullException.ThrowIfNull(documentation);

        this.context = new RenderingContext(assembly, type, documentation, options ?? new TypeDocumentationOptions());
    }

    public override string ToString()
    {
        Type type = this.context.Type;
        TypeDocumentationOptions options = this.context.Options;

        XmlDocToMarkdownConverter converter = new(this.context, this.context.CrefResolver);
        ExampleInjector examples = new(options.ExamplesDirectory);
        MemberDiscovery discovery = new(type, options.MemberAccessibilityLevel);
        MethodParametersRenderer parameters = new(this.context, converter);

        if (options.BackButton)
        {
            BackButtonRenderer.Write(this.document, type, options.Structure, BackButtonRenderer.Position.Top);
        }

        this.document.AppendHeader(type.GetDisplayName().FormatChevrons(), 1);

        if (type.Namespace != null)
        {
            this.document.AppendParagraph($"Namespace: {type.Namespace}");
        }

        XElement? typeDocElement = this.context.GetMemberDoc(type);
        if (typeDocElement != null)
        {
            Logger.Info("    (documented)");
        }

        ObsoleteRenderer.Write(this.document, type, "This type is obsolete.");
        this.WriteSummary(converter, typeDocElement);
        this.document.AppendCode("csharp", type.GetSignature(full: true));
        parameters.WriteTypeParameters(this.document, type, typeDocElement);
        InheritanceRenderer.Write(this.document, this.context);
        this.WriteRemarks(converter, typeDocElement);

        if (type.IsEnum)
        {
            new EnumFieldsTableRenderer(this.context, converter).Render(this.document, discovery.GetEnumFields());
        }
        else
        {
            new FieldSectionRenderer(this.context, converter, examples).Render(this.document, discovery.GetFields());
        }

        new PropertySectionRenderer(this.context, converter, examples).Render(this.document, discovery.GetProperties());
        new IndexerSectionRenderer(this.context, converter, examples).Render(this.document, discovery.GetIndexers());
        new ConstructorSectionRenderer(this.context, converter, examples, parameters).Render(this.document, discovery.GetConstructors());
        new MethodSectionRenderer(this.context, converter, examples, parameters).Render(this.document, discovery.GetMethods());
        new OperatorSectionRenderer(this.context, converter, examples, parameters).Render(this.document, discovery.GetOperators());
        new EventSectionRenderer(this.context, converter, examples).Render(this.document, discovery.GetEvents());

        if (examples.Inject(this.document, type))
        {
            Logger.Info("    (example)");
        }

        if (options.BackButton)
        {
            BackButtonRenderer.Write(this.document, type, options.Structure, BackButtonRenderer.Position.Bottom);
        }

        return this.document.ToString();
    }

    private void WriteSummary(XmlDocToMarkdownConverter converter, XElement? typeDocElement)
    {
        IEnumerable<XNode>? nodes = typeDocElement?.Element("summary")?.Nodes();
        if (nodes is not null)
        {
            this.document.Append(converter.ToMarkdownParagraph(nodes));
        }
    }

    private void WriteRemarks(XmlDocToMarkdownConverter converter, XElement? typeDocElement)
    {
        IEnumerable<XNode>? nodes = typeDocElement?.Element("remarks")?.Nodes();
        if (nodes is not null)
        {
            this.document.AppendParagraph(new MarkdownStrongEmphasis("Remarks:"));
            this.document.Append(converter.ToMarkdownParagraph(nodes));
        }
    }
}
