using System.Reflection;
using Markdown;
using XMLDoc2Markdown.Signatures;
using XMLDoc2Markdown.Utils;

namespace XMLDoc2Markdown.Linking;

/// <summary>
/// Single point of decision for "where does a link to <em>member</em> point?".
/// Replaces the previous triplet of <c>GetDocsLink</c> / <c>GetMSDocsUrl</c> /
/// <c>GetInternalDocsUrl</c> spread across TypeExtensions, MethodBaseExtensions
/// and MemberInfoExtensions.
/// </summary>
internal static class DocLinkFactory
{
    private const string DefaultMsDocsBaseUrl = "https://docs.microsoft.com/en-us/dotnet/api";

    internal static string GetMSDocsUrl(this Type type, string msdocsBaseUrl = DefaultMsDocsBaseUrl)
    {
        ArgumentNullException.ThrowIfNull(type);
        EnsureMscorlib(type);
        return $"{msdocsBaseUrl}/{type.GetDocsFileName(DocumentationStructure.Flat)}";
    }

    internal static string GetMSDocsUrl(this MemberInfo memberInfo, string msdocsBaseUrl = DefaultMsDocsBaseUrl)
    {
        ArgumentNullException.ThrowIfNull(memberInfo);

        Type type = memberInfo.DeclaringType ?? throw new Exception($"Member {memberInfo.Name} has no declaring type.");
        EnsureMscorlib(type);

        string slug = memberInfo switch
        {
            MethodBase => memberInfo.Name.ToLower().Replace('`', '-'),
            _ => memberInfo.Name.ToLower()
        };
        return $"{msdocsBaseUrl}/{type.GetDocsFileName(DocumentationStructure.Flat)}.{slug}";
    }

    internal static string GetInternalDocsUrl(this Type type, DocumentationStructure structure, bool noExtension = false, bool noPrefix = false)
    {
        ArgumentNullException.ThrowIfNull(type);
        return BuildPagePath(type, structure, noExtension, noPrefix);
    }

    internal static string GetInternalDocsUrl(this MemberInfo memberInfo, DocumentationStructure structure, bool noExtension = false, bool noPrefix = false)
    {
        ArgumentNullException.ThrowIfNull(memberInfo);

        Type type = memberInfo.DeclaringType ?? throw new Exception($"Member {memberInfo.Name} has no declaring type.");
        string page = BuildPagePath(type, structure, noExtension, noPrefix);

        string anchor = memberInfo is MethodBase methodBase
            ? methodBase.GetSignature().ToAnchorLink()
            : memberInfo.Name.ToAnchorLink();

        return $"{page}#{anchor}";
    }

    internal static MarkdownInlineElement GetDocsLink(
        this Type type,
        Assembly assembly,
        DocumentationStructure structure,
        string? text = null,
        bool noExtension = false,
        bool noPrefix = false)
    {
        ArgumentNullException.ThrowIfNull(type);
        ArgumentNullException.ThrowIfNull(assembly);

        text = string.IsNullOrEmpty(text) ? type.GetDisplayName().FormatChevrons() : text;

        if (!string.IsNullOrEmpty(type.FullName))
        {
            if (type.Assembly == typeof(string).Assembly)
            {
                return new MarkdownLink(text, type.GetMSDocsUrl());
            }
            if (type.Assembly == assembly)
            {
                return new MarkdownLink(text, type.GetInternalDocsUrl(structure, noExtension, noPrefix));
            }
        }
        return new MarkdownText(text);
    }

    internal static MarkdownInlineElement GetDocsLink(
        this MemberInfo memberInfo,
        Assembly assembly,
        DocumentationStructure structure,
        string? text = null,
        bool noExtension = false,
        bool noPrefix = false)
    {
        ArgumentNullException.ThrowIfNull(memberInfo);
        ArgumentNullException.ThrowIfNull(assembly);

        return memberInfo switch
        {
            Type type => type.GetDocsLink(assembly, structure, text, noExtension, noPrefix),
            MethodBase method => BuildMethodLink(method, assembly, structure, text, noExtension, noPrefix),
            _ => BuildSimpleMemberLink(memberInfo, assembly, structure, text, noExtension, noPrefix)
        };
    }

    private static MarkdownInlineElement BuildMethodLink(
        MethodBase method, Assembly assembly, DocumentationStructure structure,
        string? text, bool noExtension, bool noPrefix)
    {
        Type? declaringType = method.DeclaringType;
        if (declaringType is null)
        {
            return new MarkdownText(text ?? method.Name);
        }

        text = string.IsNullOrEmpty(text)
            ? $"{declaringType.GetDisplayName().FormatChevrons()}.{method.GetSignature().FormatChevrons()}"
            : text;

        if (declaringType.Assembly == typeof(string).Assembly)
        {
            return new MarkdownLink(text, method.GetMSDocsUrl());
        }
        if (declaringType.Assembly == assembly)
        {
            return new MarkdownLink(text, method.GetInternalDocsUrl(structure, noExtension, noPrefix));
        }
        return new MarkdownText(text);
    }

    private static MarkdownInlineElement BuildSimpleMemberLink(
        MemberInfo memberInfo, Assembly assembly, DocumentationStructure structure,
        string? text, bool noExtension, bool noPrefix)
    {
        Type? declaringType = memberInfo.DeclaringType;
        if (declaringType is null)
        {
            return new MarkdownText(text ?? memberInfo.Name);
        }

        text = string.IsNullOrEmpty(text)
            ? $"{declaringType.GetDisplayName().FormatChevrons()}.{memberInfo.Name}"
            : text;

        if (declaringType.Assembly == typeof(string).Assembly)
        {
            return new MarkdownLink(text, memberInfo.GetMSDocsUrl());
        }
        if (declaringType.Assembly == assembly)
        {
            return new MarkdownLink(text, memberInfo.GetInternalDocsUrl(structure, noExtension, noPrefix));
        }
        return new MarkdownText(text);
    }

    private static string BuildPagePath(Type type, DocumentationStructure structure, bool noExtension, bool noPrefix)
    {
        string url = type.GetDocsFileName(structure);
        if (!noExtension) url += ".md";
        if (!noPrefix) url = "./" + url;
        return url;
    }

    private static void EnsureMscorlib(Type type)
    {
        if (type.Assembly != typeof(string).Assembly)
        {
            throw new InvalidOperationException($"{type.FullName} is not a mscorlib type.");
        }
    }
}
