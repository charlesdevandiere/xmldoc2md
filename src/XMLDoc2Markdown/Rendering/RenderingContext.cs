using System.Reflection;
using System.Xml.Linq;
using Markdown;
using XMLDoc2Markdown.Linking;

namespace XMLDoc2Markdown.Rendering;

/// <summary>
/// Bundles every piece of state a section renderer needs. Instead of every
/// helper accepting (assembly, options, structure, noExtension, noPrefix),
/// they accept one context and read what they need from it.
/// </summary>
internal sealed class RenderingContext
{
    private CrefResolver? crefResolver;
    private InheritDocResolver? inheritDocResolver;

    internal Assembly Assembly { get; }
    internal Type Type { get; }
    internal XmlDocumentation Documentation { get; }
    internal TypeDocumentationOptions Options { get; }
    internal NullabilityInfoContext Nullability { get; } = new();

    internal RenderingContext(
        Assembly assembly,
        Type type,
        XmlDocumentation documentation,
        TypeDocumentationOptions options)
    {
        this.Assembly = assembly;
        this.Type = type;
        this.Documentation = documentation;
        this.Options = options;
    }

    internal bool NoExtension => this.Options.GitHubPages || this.Options.GitlabWiki;
    internal bool NoPrefix => this.Options.GitlabWiki;
    internal DocumentationStructure Structure => this.Options.Structure;

    internal CrefResolver CrefResolver => this.crefResolver ??= new CrefResolver(this.Assembly);

    internal InheritDocResolver InheritDocResolver =>
        this.inheritDocResolver ??= new InheritDocResolver(this.Documentation, this.CrefResolver);

    /// <summary>
    /// Returns the XML doc element for a member with <c>&lt;inheritdoc/&gt;</c>
    /// tags expanded.
    /// </summary>
    internal XElement? GetMemberDoc(MemberInfo member)
    {
        XElement? raw = this.Documentation.GetMember(member);
        return raw is null ? null : this.InheritDocResolver.Resolve(member, raw);
    }

    internal MarkdownInlineElement DocsLink(Type type, string? text = null)
        => type.GetDocsLink(this.Assembly, this.Structure, text, this.NoExtension, this.NoPrefix);

    internal MarkdownInlineElement DocsLink(MemberInfo member, string? text = null)
        => member.GetDocsLink(this.Assembly, this.Structure, text, this.NoExtension, this.NoPrefix);
}
