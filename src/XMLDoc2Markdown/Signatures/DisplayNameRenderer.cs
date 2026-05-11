using System.Reflection;

namespace XMLDoc2Markdown.Signatures;

internal static class DisplayNameRenderer
{
    private static readonly IReadOnlyDictionary<Type, string> SimplifiedTypeNames = new Dictionary<Type, string>
    {
        { typeof(void), "void" },
        { typeof(object), "object" },
        { typeof(bool), "bool" },
        { typeof(sbyte), "sbyte" },
        { typeof(byte), "byte" },
        { typeof(short), "short" },
        { typeof(ushort), "ushort" },
        { typeof(int), "int" },
        { typeof(uint), "uint" },
        { typeof(long), "long" },
        { typeof(ulong), "ulong" },
        { typeof(float), "float" },
        { typeof(double), "double" },
        { typeof(decimal), "decimal" },
        { typeof(char), "char" },
        { typeof(string), "string" },
    };

    internal static string GetSimplifiedName(this Type type)
        => SimplifiedTypeNames.TryGetValue(type, out string? name) ? name : type.Name;

    internal static string GetDisplayName(this Type type, bool simplifyName = false)
    {
        string name = simplifyName ? type.GetSimplifiedName() : type.Name;

        TypeInfo typeInfo = type.GetTypeInfo();
        Type[] genericParams = typeInfo.GenericTypeArguments.Length > 0
            ? typeInfo.GenericTypeArguments
            : typeInfo.GenericTypeParameters;

        if (genericParams.Length > 0)
        {
            int tickIdx = name.IndexOf('`');
            if (tickIdx > -1)
            {
                name = name[..tickIdx];
            }
            name += $"<{string.Join(", ", genericParams.Select(t => t.GetDisplayName(simplifyName)))}>";
        }

        return name;
    }
}
