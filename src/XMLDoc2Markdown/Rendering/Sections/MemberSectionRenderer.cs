using System.Reflection;
using System.Xml.Linq;
using Markdown;
using XMLDoc2Markdown.Signatures;
using XMLDoc2Markdown.Utils;
using XMLDoc2Markdown.XmlDocId;

namespace XMLDoc2Markdown.Rendering.Sections;

/// <summary>
/// Shared scaffolding for emitting a "## Methods / Properties / Events / Fields"
/// section: heading, then for each member: signature header, obsolete, summary,
/// signature block, kind-specific body (parameters, returns, etc.), exceptions,
/// remarks, example.
/// </summary>
internal abstract class MemberSectionRenderer<T> where T : MemberInfo
{
    protected RenderingContext Context { get; }
    protected XmlDocToMarkdownConverter Converter { get; }
    protected ExampleInjector Examples { get; }

    protected abstract string SectionHeader { get; }
    protected abstract string ObsoleteDefaultMessage { get; }

    protected MemberSectionRenderer(
        RenderingContext context,
        XmlDocToMarkdownConverter converter,
        ExampleInjector examples)
    {
        this.Context = context;
        this.Converter = converter;
        this.Examples = examples;
    }

    internal void Render(IMarkdownDocument document, IReadOnlyList<T> members)
    {
        if (members.Count == 0)
        {
            return;
        }

        document.AppendHeader(this.SectionHeader, 2);
        Logger.Info($"    {this.SectionHeader}");

        foreach (T member in members)
        {
            this.RenderMember(document, member);
        }
    }

    protected virtual void RenderMember(IMarkdownDocument document, T member)
    {
        document.AppendHeader(new MarkdownStrongEmphasis(member.GetSignature().FormatChevrons()), 3);

        XElement? memberDocElement = this.Context.Documentation.GetMember(member);

        ObsoleteRenderer.Write(document, member, this.ObsoleteDefaultMessage);
        this.WriteSummary(document, memberDocElement);
        document.AppendCode("csharp", member.GetSignature(full: true));

        this.RenderBody(document, member, memberDocElement);

        this.WriteExceptions(document, memberDocElement);
        this.WriteRemarks(document, memberDocElement);
        bool example = this.Examples.Inject(document, member);

        string log = $"      {member.GetIdentifier()}";
        if (memberDocElement is not null) log += " (documented)";
        if (example) log += " (example)";
        Logger.Info(log);
    }

    protected abstract void RenderBody(IMarkdownDocument document, T member, XElement? memberDocElement);

    protected void WriteSummary(IMarkdownDocument document, XElement? memberDocElement)
    {
        IEnumerable<XNode>? nodes = memberDocElement?.Element("summary")?.Nodes();
        if (nodes is not null)
        {
            document.Append(this.Converter.ToMarkdownParagraph(nodes));
        }
    }

    protected void WriteRemarks(IMarkdownDocument document, XElement? memberDocElement)
    {
        IEnumerable<XNode>? nodes = memberDocElement?.Element("remarks")?.Nodes();
        if (nodes is not null)
        {
            document.AppendParagraph(new MarkdownStrongEmphasis("Remarks:"));
            document.Append(this.Converter.ToMarkdownParagraph(nodes));
        }
    }

    private void WriteExceptions(IMarkdownDocument document, XElement? memberDocElement)
    {
        XElement[] exceptionDocs = memberDocElement?.Elements("exception").ToArray() ?? [];
        if (exceptionDocs.Length == 0)
        {
            return;
        }

        document.AppendHeader("Exceptions", 4);

        foreach (XElement exceptionDoc in exceptionDocs)
        {
            string? cref = exceptionDoc.Attribute("cref")?.Value;
            MarkdownInlineElement? exceptionLink = this.Converter.LinkFromCref(cref);
            MarkdownParagraph summary = this.Converter.ToMarkdownParagraph(exceptionDoc.Nodes());

            document.AppendParagraph(string.Join($"<br>{Environment.NewLine}", exceptionLink, summary));
        }
    }
}
