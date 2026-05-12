using System.Reflection;
using System.Text.RegularExpressions;

namespace XMLDoc2Markdown.XmlDocId;

internal static partial class XmlDocIdBuilder
{
    [GeneratedRegex(@"\[.*\]")]
    private static partial Regex ArrayBracketsRegex();

    internal static string GetIdentifier(this MemberInfo memberInfo) => memberInfo switch
    {
        Type type => BuildTypeId(type),
        PropertyInfo or FieldInfo or EventInfo => BuildMemberId(memberInfo),
        MethodBase methodBase => BuildMethodId(methodBase),
        _ => throw new NotSupportedException($"{nameof(GetIdentifier)} encountered an unhandled member info: {memberInfo}")
    };

    private static string BuildTypeId(Type type)
        => ArrayBracketsRegex().Replace(type.FullName ?? type.Name, string.Empty).Replace('+', '.');

    private static string BuildMemberId(MemberInfo memberInfo)
        => memberInfo.DeclaringType != null
            ? memberInfo.DeclaringType.GetIdentifier() + "." + memberInfo.Name
            : memberInfo.Name;

    private static string BuildMethodId(MethodBase methodBase)
    {
        Dictionary<string, int> typeGenericMap = [];
        Type[]? typeGenericArgs = methodBase.DeclaringType?.GetGenericArguments();
        for (int i = 0; i < typeGenericArgs?.Length; i++)
        {
            typeGenericMap[typeGenericArgs[i].Name] = i;
        }

        Dictionary<string, int> methodGenericMap = [];
        if (methodBase is MethodInfo)
        {
            Type[] methodGenericArgs = methodBase.GetGenericArguments();
            for (int i = 0; i < methodGenericArgs.Length; i++)
            {
                methodGenericMap[methodGenericArgs[i].Name] = i;
            }
        }

        string id = string.Empty;
        if (methodBase.DeclaringType != null)
        {
            id += XmlDocTypeNameEncoder.Encode(methodBase.DeclaringType, false, typeGenericMap, methodGenericMap);
            id += ".";
        }

        id += methodBase is ConstructorInfo ? "#ctor" : methodBase.Name;
        id += methodGenericMap.Count > 0 ? "``" + methodGenericMap.Count : string.Empty;

        ParameterInfo[] @params = methodBase.GetParameters();
        if (@params.Length > 0)
        {
            string paramList = string.Join(
                ",",
                @params.Select(p => XmlDocTypeNameEncoder.Encode(p.ParameterType, true, typeGenericMap, methodGenericMap)));
            id += $"({paramList})";
        }

        if (methodBase is MethodInfo methodInfo &&
            (methodBase.Name == "op_Implicit" || methodBase.Name == "op_Explicit"))
        {
            id += "~" + XmlDocTypeNameEncoder.Encode(methodInfo.ReturnType, true, typeGenericMap, methodGenericMap);
        }

        return id;
    }
}
