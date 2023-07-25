using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Markdown;

namespace XMLDoc2Markdown.Utils;

internal static class MethodBaseExtensions
{
    internal static Visibility GetVisibility(this MethodBase methodBase)
    {
        if (methodBase.IsPublic)
        {
            return Visibility.Public;
        }
        else if (methodBase.IsAssembly)
        {
            return Visibility.Internal;
        }
        else if (methodBase.IsFamily)
        {
            return Visibility.Protected;
        }
        else if (methodBase.IsFamilyOrAssembly)
        {
            return Visibility.ProtectedInternal;
        }
        else if (methodBase.IsPrivate)
        {
            return Visibility.Private;
        }
        else
        {
            return Visibility.None;
        }
    }

    internal static string GetSignature(this MethodBase methodBase, bool full = false)
    {
        List<string> signature = new();

        if (full)
        {
            if (methodBase.DeclaringType.IsClass)
            {
                signature.Add(methodBase.GetVisibility().Print());

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

        string displayName = methodBase.MemberType == MemberTypes.Constructor ? methodBase.DeclaringType.Name : methodBase.Name;
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
        ParameterInfo[] @params = methodBase.GetParameters();
        IEnumerable<string> paramsNames = @params
            .Select(p => $"{p.GetStringIsbyRef()}{p.GetStringIsParams()}{ RemoveSymbolRef(p.ParameterType.GetDisplayName(simplifyName: full)) }{ RemoveSymbolRef((full ? $" { p.Name}" : null))}");
        displayName += $"({string.Join(", ", paramsNames)})";
        signature.Add(displayName);

        return string.Join(' ', signature);
    }

    internal static string RemoveSymbolRef(string name)
    {
        if (name == null)
        {
            return name;
        }
        if (name.EndsWith("&"))
        {
            return name.Substring(0,name.Length-1);
        }
        return name;
    }

    internal static string GetStringIsbyRef(this ParameterInfo param)
    {
        if (param.ParameterType.IsByRef)
        {
            return "ref ";
        }
        return "";
    }

    internal static string GetStringIsParams(this ParameterInfo param)
    {
        if (param.IsDefined(typeof(ParamArrayAttribute), false))
        {
            return "params ";
        }
        return "";
    }

    internal static string GetMSDocsUrl(this MethodBase methodInfo, string msdocsBaseUrl = "https://docs.microsoft.com/en-us/dotnet/api")
    {
        RequiredArgument.NotNull(methodInfo, nameof(methodInfo));

        Type type = methodInfo.DeclaringType ?? throw new Exception($"Method {methodInfo.Name} has no declaring type.");

        if (type.Assembly != typeof(string).Assembly)
        {
            throw new InvalidOperationException($"{type.FullName} is not a mscorlib type.");
        }

        return $"{msdocsBaseUrl}/{type.GetDocsFileName()}.{methodInfo.Name.ToLower().Replace('`', '-')}";
    }

    internal static string GetInternalDocsUrl(this MethodBase methodInfo, bool noExtension = false, bool noPrefix = false)
    {
        RequiredArgument.NotNull(methodInfo, nameof(methodInfo));

        Type type = methodInfo.DeclaringType ?? throw new Exception($"Method {methodInfo.Name} has no declaring type.");

        string url = $"{type.GetDocsFileName()}";

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

    internal static MarkdownInlineElement GetDocsLink(this MethodBase methodInfo, Assembly assembly, string text = null, bool noExtension = false, bool noPrefix = false)
    {
        RequiredArgument.NotNull(methodInfo, nameof(methodInfo));
        RequiredArgument.NotNull(assembly, nameof(assembly));

        Type type = methodInfo.DeclaringType;

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
                return new MarkdownLink(text, methodInfo.GetInternalDocsUrl(noExtension, noPrefix));
            }

            return new MarkdownText(text);
        }

        return new MarkdownText(text ?? methodInfo.Name);
    }
}
