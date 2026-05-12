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
    private const string DefaultMsDocsBaseUrl = "https://learn.microsoft.com/en-us/dotnet/api";

    internal static bool LooksLikeMicrosoftType(string? @namespace)
    {
        if (string.IsNullOrEmpty(@namespace))
        {
            return false;
        }

        return @namespace is "System" or "Microsoft" or "Windows"
            || @namespace.StartsWith("System.", StringComparison.Ordinal)
            || @namespace.StartsWith("Microsoft.", StringComparison.Ordinal)
            || @namespace.StartsWith("Windows.", StringComparison.Ordinal);
    }

    private static bool IsMicrosoftFamily(Type type) =>
        type.Assembly == typeof(string).Assembly || LooksLikeMicrosoftType(type.Namespace);

    internal static string GetMSDocsUrl(this Type type, string msdocsBaseUrl = DefaultMsDocsBaseUrl)
    {
        ArgumentNullException.ThrowIfNull(type);
        EnsureMicrosoftFamily(type);
        return $"{msdocsBaseUrl}/{type.GetDocsFileName(DocumentationStructure.Flat)}";
    }

    internal static string GetMSDocsUrl(this MemberInfo memberInfo, string msdocsBaseUrl = DefaultMsDocsBaseUrl)
    {
        ArgumentNullException.ThrowIfNull(memberInfo);

        Type type = memberInfo.DeclaringType ?? throw new InvalidOperationException($"Member {memberInfo.Name} has no declaring type.");
        EnsureMicrosoftFamily(type);

        string slug = memberInfo switch
        {
            MethodBase => memberInfo.Name.ToLower().Replace('`', '-'),
            _ => memberInfo.Name.ToLower()
        };
        return $"{msdocsBaseUrl}/{type.GetDocsFileName(DocumentationStructure.Flat)}.{slug}";
    }

    // Returns null when the cref is malformed or its namespace is not Microsoft-owned.
    internal static string? MsDocsUrlFromCref(string? cref, string msdocsBaseUrl = DefaultMsDocsBaseUrl)
    {
        if (cref is null || cref.Length <= 2 || cref[1] != ':') return null;

        string body = cref[2..];
        int paren = body.IndexOf('(');
        if (paren > -1) body = body[..paren];

        // For T: the body is the type FQN; for M:/P:/F:/E: the body is type-FQN + "." + member.
        // We need the type's namespace either way.
        string typeFqn = cref[0] == 'T' ? body : DropMemberSegment(body);
        int lastDot = typeFqn.LastIndexOf('.');
        if (lastDot < 0) return null;

        string @namespace = typeFqn[..lastDot];
        if (!LooksLikeMicrosoftType(@namespace)) return null;

        string slug = body.ToLowerInvariant().Replace('`', '-');
        return $"{msdocsBaseUrl}/{slug}";
    }

    private static string DropMemberSegment(string body)
    {
        int lastDot = body.LastIndexOf('.');
        return lastDot < 0 ? body : body[..lastDot];
    }

    internal static string GetInternalDocsUrl(this Type type, DocumentationStructure structure, bool noExtension = false, bool noPrefix = false)
    {
        ArgumentNullException.ThrowIfNull(type);
        return BuildPagePath(type, structure, noExtension, noPrefix);
    }

    internal static string GetInternalDocsUrl(this MemberInfo memberInfo, DocumentationStructure structure, bool noExtension = false, bool noPrefix = false)
    {
        ArgumentNullException.ThrowIfNull(memberInfo);

        Type type = memberInfo.DeclaringType ?? throw new InvalidOperationException($"Member {memberInfo.Name} has no declaring type.");
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
            if (type.Assembly == assembly)
            {
                return new MarkdownLink(text, type.GetInternalDocsUrl(structure, noExtension, noPrefix));
            }
            if (IsMicrosoftFamily(type))
            {
                return new MarkdownLink(text, type.GetMSDocsUrl());
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

        if (declaringType.Assembly == assembly)
        {
            return new MarkdownLink(text, method.GetInternalDocsUrl(structure, noExtension, noPrefix));
        }
        if (IsMicrosoftFamily(declaringType))
        {
            return new MarkdownLink(text, method.GetMSDocsUrl());
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

        if (declaringType.Assembly == assembly)
        {
            return new MarkdownLink(text, memberInfo.GetInternalDocsUrl(structure, noExtension, noPrefix));
        }
        if (IsMicrosoftFamily(declaringType))
        {
            return new MarkdownLink(text, memberInfo.GetMSDocsUrl());
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

    private static void EnsureMicrosoftFamily(Type type)
    {
        if (!IsMicrosoftFamily(type))
        {
            throw new InvalidOperationException($"{type.FullName} is not a Microsoft-family type.");
        }
    }
}
