using System.Text.RegularExpressions;

namespace XMLDoc2Markdown.XmlDocId;

internal static partial class XmlDocTypeNameEncoder
{
    [GeneratedRegex(@"`\d+")]
    private static partial Regex GenericArityRegex();

    internal static string? Encode(
        Type? type,
        bool isMethodParameter,
        IReadOnlyDictionary<string, int> typeGenericMap,
        IReadOnlyDictionary<string, int> methodGenericMap)
    {
        if (type is null)
        {
            return null;
        }

        if (type.IsGenericParameter)
        {
            return methodGenericMap.TryGetValue(type.Name, out int methodIndex)
                ? "``" + methodIndex
                : "`" + typeGenericMap[type.Name];
        }

        if (type.HasElementType)
        {
            string? element = Encode(type.GetElementType(), isMethodParameter, typeGenericMap, methodGenericMap);

            if (type.IsPointer) return element + "*";
            if (type.IsByRef) return element + "@";
            if (type.IsArray)
            {
                int rank = type.GetArrayRank();
                string dims = rank > 1
                    ? $"[{string.Join(",", Enumerable.Repeat("0:", rank))}]"
                    : "[]";
                return element + dims;
            }

            throw new Exception($"{nameof(XmlDocTypeNameEncoder)}.{nameof(Encode)} encountered an unhandled element type: {type}");
        }

        string name = type.IsNested
            ? Encode(type.DeclaringType, isMethodParameter, typeGenericMap, methodGenericMap) + "."
            : type.Namespace + ".";

        name += isMethodParameter
            ? GenericArityRegex().Replace(type.Name, string.Empty)
            : type.Name;

        if (type.IsGenericType && isMethodParameter)
        {
            IEnumerable<string?> args = type.GetGenericArguments()
                .Select(a => Encode(a, isMethodParameter, typeGenericMap, methodGenericMap));
            name += $"{{{string.Join(",", args)}}}";
        }

        return name;
    }
}
