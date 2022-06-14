using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Markdown;

namespace XMLDoc2Markdown.Utils;

internal static class MemberInfoExtensions
{
    internal static string GetSignature(this MemberInfo memberInfo, bool full = false)
    {
        if (memberInfo is Type type)
        {
            return type.GetSignature(full);
        }
        else if (memberInfo is MethodBase methodBase)
        {
            return methodBase.GetSignature(full);
        }
        else if (memberInfo is PropertyInfo propertyInfo)
        {
            return propertyInfo.GetSignature(full);
        }
        else if (memberInfo is EventInfo eventInfo)
        {
            return eventInfo.GetSignature(full);
        }
        else if (memberInfo is FieldInfo fieldInfo)
        {
            return fieldInfo.GetSignature(full);
        }

        throw new NotImplementedException();
    }

    internal static string GetIdentifier(this MemberInfo memberInfo)
    {
        switch (memberInfo)
        {
            case Type type:
                return Regex.Replace(type.FullName, @"\[.*\]", string.Empty).Replace('+', '.');

            case PropertyInfo _:
            case FieldInfo _:
            case EventInfo _:
                return memberInfo.DeclaringType.GetIdentifier() + "." + memberInfo.Name;

            case MethodBase methodBase:
                Dictionary<string, int> typeGenericMap = new();
                Type[] typeGenericArguments = methodBase.DeclaringType.GetGenericArguments();
                for (int i = 0; i < typeGenericArguments.Length; i++)
                {
                    Type typeGeneric = typeGenericArguments[i];
                    typeGenericMap[typeGeneric.Name] = i;
                }

                Dictionary<string, int> methodGenericMap = new();
                if (methodBase is MethodInfo)
                {
                    Type[] methodGenericArguments = methodBase.GetGenericArguments();
                    for (int i = 0; i < methodGenericArguments.Length; i++)
                    {
                        Type methodGeneric = methodGenericArguments[i];
                        methodGenericMap[methodGeneric.Name] = i;
                    }
                }

                ParameterInfo[] parameterInfos = methodBase.GetParameters();

                string identifier = GetFormatedTypeName(methodBase.DeclaringType, false, typeGenericMap, methodGenericMap);
                identifier += ".";
                identifier += methodBase is ConstructorInfo ? "#ctor" : methodBase.Name;
                identifier += methodGenericMap.Count > 0 ? "``" + methodGenericMap.Count : string.Empty;
                identifier += parameterInfos.Length > 0
                    ? $@"({string.Join(
                        ",",
                        methodBase.GetParameters().Select(x => GetFormatedTypeName(x.ParameterType, true, typeGenericMap, methodGenericMap)))})"
                    : string.Empty;

                if (methodBase is MethodInfo methodInfo &&
                    (methodBase.Name == "op_Implicit" ||
                    methodBase.Name == "op_Explicit"))
                {
                    identifier += "~" + GetFormatedTypeName(methodInfo.ReturnType, true, typeGenericMap, methodGenericMap);
                }

                return identifier;

            default:
                throw new Exception($"{nameof(GetIdentifier)} encountered an unhandled member info: {memberInfo}");
        }
    }

    internal static string GetMSDocsUrl(this MemberInfo memberInfo, string msdocsBaseUrl = "https://docs.microsoft.com/en-us/dotnet/api")
    {
        RequiredArgument.NotNull(memberInfo, nameof(memberInfo));

        Type type = memberInfo.DeclaringType ?? throw new Exception($"Property {memberInfo.Name} has no declaring type.");

        if (type.Assembly != typeof(string).Assembly)
        {
            throw new InvalidOperationException($"{type.FullName} is not a mscorlib type.");
        }

        return $"{msdocsBaseUrl}/{type.GetDocsFileName()}.{memberInfo.Name.ToLower()}";
    }

    internal static string GetInternalDocsUrl(this MemberInfo memberInfo, bool noExtension = false, bool noPrefix = false)
    {
        RequiredArgument.NotNull(memberInfo, nameof(memberInfo));

        Type type = memberInfo.DeclaringType ?? throw new Exception($"Event {memberInfo.Name} has no declaring type.");

        string url = $"{type.GetDocsFileName()}";

        if (!noExtension)
        {
            url += ".md";
        }

        if (!noPrefix)
        {
            url = url.Insert(0, "./");
        }

        string anchor = memberInfo.Name.ToAnchorLink();

        return $"{url}#{anchor}";
    }

    internal static MarkdownInlineElement GetDocsLink(this MemberInfo memberInfo, Assembly assembly, string text = null, bool noExtension = false, bool noPrefix = false)
    {
        RequiredArgument.NotNull(memberInfo, nameof(memberInfo));
        RequiredArgument.NotNull(assembly, nameof(assembly));

        return memberInfo switch
        {
            Type type => type.GetDocsLink(assembly, text, noExtension, noPrefix),
            MethodBase method => method.GetDocsLink(assembly, text, noExtension, noPrefix),
            _ => getDocsLinkBase(memberInfo, assembly, text, noExtension, noPrefix),
        };

        static MarkdownInlineElement getDocsLinkBase(MemberInfo memberInfo, Assembly assembly, string text = null, bool noExtension = false, bool noPrefix = false)
        {
            Type declaringType = memberInfo.DeclaringType;

            if (declaringType is not null)
            {
                if (string.IsNullOrEmpty(text))
                {
                    text = $"{declaringType.GetDisplayName().FormatChevrons()}.{memberInfo.Name}";
                }

                if (declaringType.Assembly == typeof(string).Assembly)
                {
                    return new MarkdownLink(text, memberInfo.GetMSDocsUrl());
                }
                else if (declaringType.Assembly == assembly)
                {
                    return new MarkdownLink(text, memberInfo.GetInternalDocsUrl(noExtension, noPrefix));
                }

                return new MarkdownText(text);
            }

            return new MarkdownText(text ?? memberInfo.Name);
        }
    }

    private static string GetFormatedTypeName(
        Type type,
        bool isMethodParameter,
        Dictionary<string, int> typeGenericMap,
        Dictionary<string, int> methodGenericMap)
    {
        if (type.IsGenericParameter)
        {
            return methodGenericMap.TryGetValue(type.Name, out int methodIndex)
                ? "``" + methodIndex
                : "`" + typeGenericMap[type.Name];
        }
        else if (type.HasElementType)
        {
            string elementTypeString = GetFormatedTypeName(
                type.GetElementType(),
                isMethodParameter,
                typeGenericMap,
                methodGenericMap);

            switch (type)
            {
                case Type when type.IsPointer:
                    return elementTypeString + "*";

                case Type when type.IsByRef:
                    return elementTypeString + "@";

                case Type when type.IsArray:
                    int rank = type.GetArrayRank();
                    string arrayDimensionsString = rank > 1
                        ? $"[{string.Join(",", Enumerable.Repeat("0:", rank))}]"
                        : "[]";
                    return elementTypeString + arrayDimensionsString;

                default:
                    throw new Exception($"{nameof(GetFormatedTypeName)} encountered an unhandled element type: {type}");
            }
        }
        else
        {
            string name = type.IsNested
                ? GetFormatedTypeName(
                    type.DeclaringType,
                    isMethodParameter,
                    typeGenericMap,
                    methodGenericMap) + "."
                : type.Namespace + ".";

            name += isMethodParameter
                ? Regex.Replace(type.Name, @"`\d+", string.Empty)
                : type.Name;

            if (type.IsGenericType && isMethodParameter)
            {
                IEnumerable<string> genericArgs = type.GetGenericArguments()
                            .Select(argument => GetFormatedTypeName(argument, isMethodParameter, typeGenericMap, methodGenericMap));
                name += $"{{{string.Join(",", genericArgs)}}}";
            }

            return name;
        }
    }
}
