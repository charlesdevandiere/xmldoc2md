using System.Reflection;
using Markdown;

namespace XMLDoc2Markdown.Utils;

internal static class MethodBaseExtensions
{
    internal static Accessibility GetAccessibility(this MethodBase methodBase)
    {
        if (methodBase.IsPublic)
        {
            return Accessibility.Public;
        }
        else if (methodBase.IsAssembly)
        {
            return Accessibility.Internal;
        }
        else if (methodBase.IsFamily)
        {
            return Accessibility.Protected;
        }
        else if (methodBase.IsFamilyOrAssembly)
        {
            return Accessibility.ProtectedInternal;
        }
        else if (methodBase.IsPrivate)
        {
            return Accessibility.Private;
        }
        else
        {
            return Accessibility.None;
        }
    }

    internal static string GetSignature(this MethodBase methodBase, bool full = false)
    {
        List<string> signature = new();

        if (full)
        {
            if (methodBase.DeclaringType?.IsClass ?? false)
            {
                signature.Add(methodBase.GetAccessibility().Print());

                if (methodBase.IsStatic)
                {
                    signature.Add("static");
                }

                if (methodBase.IsAbstract)
                {
                    signature.Add("abstract");
                }
            }

            if (methodBase is MethodInfo methodInfo)
            {
                signature.Add(methodInfo.ReturnType.GetDisplayName(simplifyName: true));
            }
        }

        string displayName = methodBase.MemberType == MemberTypes.Constructor && methodBase.DeclaringType != null
            ? methodBase.DeclaringType.Name
            : methodBase.Name;
        int genericCharIndex = displayName.IndexOf('`');
        if (genericCharIndex > -1)
        {
            displayName = displayName[..genericCharIndex];
        }
        if (methodBase is MethodInfo methodInfo1)
        {
            Type[] genericArguments = methodInfo1.GetGenericArguments();
            if (genericArguments.Length > 0)
            {
                displayName += $"<{string.Join(", ", genericArguments.Select(a => a.Name))}>";
            }
        }

        displayName += $"({string.Join(", ", methodBase.GetParameters().Select(p => p.GetDisplayLabel(full)))})";
        signature.Add(displayName);

        return string.Join(' ', signature);
    }

    internal static string GetMSDocsUrl(this MethodBase methodInfo, string msdocsBaseUrl = "https://docs.microsoft.com/en-us/dotnet/api")
    {
        ArgumentNullException.ThrowIfNull(methodInfo);

        Type type = methodInfo.DeclaringType ?? throw new Exception($"Method {methodInfo.Name} has no declaring type.");

        if (type.Assembly != typeof(string).Assembly)
        {
            throw new InvalidOperationException($"{type.FullName} is not a mscorlib type.");
        }

        return $"{msdocsBaseUrl}/{type.GetDocsFileName(DocumentationStructure.Flat)}.{methodInfo.Name.ToLower().Replace('`', '-')}";
    }

    internal static string GetInternalDocsUrl(this MethodBase methodInfo, DocumentationStructure structure, bool noExtension = false, bool noPrefix = false)
    {
        ArgumentNullException.ThrowIfNull(methodInfo);

        Type type = methodInfo.DeclaringType ?? throw new Exception($"Method {methodInfo.Name} has no declaring type.");

        string url = $"{type.GetDocsFileName(structure)}";

        if (!noExtension)
        {
            url += ".md";
        }

        if (!noPrefix)
        {
            url = url.Insert(0, "./");
        }

        string anchor = methodInfo.GetSignature().ToAnchorLink();

        return $"{url}#{anchor}";
    }

    internal static MarkdownInlineElement GetDocsLink(this MethodBase methodInfo, Assembly assembly, DocumentationStructure structure, string? text = null, bool noExtension = false, bool noPrefix = false)
    {
        ArgumentNullException.ThrowIfNull(methodInfo);
        ArgumentNullException.ThrowIfNull(assembly);

        Type? type = methodInfo.DeclaringType;

        if (type is not null)
        {
            if (string.IsNullOrEmpty(text))
            {
                text = $"{type.GetDisplayName().FormatChevrons()}.{methodInfo.GetSignature().FormatChevrons()}";
            }

            if (type.Assembly == typeof(string).Assembly)
            {
                return new MarkdownLink(text, methodInfo.GetMSDocsUrl());
            }
            else if (type.Assembly == assembly)
            {
                return new MarkdownLink(text, methodInfo.GetInternalDocsUrl(structure, noExtension, noPrefix));
            }

            return new MarkdownText(text);
        }

        return new MarkdownText(text ?? methodInfo.Name);
    }
}
