using System.Reflection;
using System.Text;

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
        => GetDisplayName(type, DisplayMeta.Empty, simplifyName);

    internal static string GetDisplayName(this Type type, DisplayMeta meta, bool simplifyName)
    {
        // Nullable<T> → T? (independent of the NullabilityInfo, which is irrelevant for value types)
        Type? underlying = Nullable.GetUnderlyingType(type);
        if (underlying != null)
        {
            DisplayMeta inner = meta.GenericArg(0);
            return GetDisplayName(underlying, inner, simplifyName) + "?";
        }

        // ValueTuple → (name? type, ...) — pulls names from the shared queue
        if (type.IsValueTuple())
        {
            return RenderTuple(type, meta, simplifyName);
        }

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
            string args = string.Join(", ", genericParams.Select((t, i) => t.GetDisplayName(meta.GenericArg(i), simplifyName)));
            name += $"<{args}>";
        }

        // Nullable reference type suffix.
        // Skip generic parameters: NullabilityInfoContext reports an unconstrained
        // `T` as `Nullable`, but C# source never spells that as `T?` unless the
        // parameter has a `class` / `struct` / `default` constraint, which is
        // metadata we cannot reliably reconstruct here.
        if (!type.IsValueType
            && !type.IsGenericParameter
            && meta.Nullability is { ReadState: NullabilityState.Nullable })
        {
            name += "?";
        }

        return name;
    }

    internal static bool IsValueTuple(this Type type)
    {
        if (!type.IsGenericType || type.IsGenericTypeDefinition)
        {
            return false;
        }
        Type def = type.GetGenericTypeDefinition();
        return def.FullName is not null && def.FullName.StartsWith("System.ValueTuple`", StringComparison.Ordinal);
    }

    private static string RenderTuple(Type tupleType, DisplayMeta meta, bool simplifyName)
    {
        StringBuilder sb = new("(");
        Type[] args = tupleType.GenericTypeArguments;
        for (int i = 0; i < args.Length; i++)
        {
            if (i > 0)
            {
                sb.Append(", ");
            }
            // The 8th slot of a long ValueTuple is itself a ValueTuple — flatten it.
            Type arg = args[i];
            if (i == 7 && IsValueTuple(arg))
            {
                sb.Length -= 2; // drop the leading ", "
                sb.Append(", ").Append(RenderTupleInner(arg, meta.GenericArg(i), simplifyName));
                continue;
            }

            DisplayMeta inner = meta.GenericArg(i);
            string typeName = GetDisplayName(arg, inner, simplifyName);
            string? elementName = meta.TupleNames.Count > 0 ? meta.TupleNames.Dequeue() : null;

            sb.Append(typeName);
            if (!string.IsNullOrEmpty(elementName))
            {
                sb.Append(' ').Append(elementName);
            }
        }
        sb.Append(')');
        return sb.ToString();
    }

    private static string RenderTupleInner(Type tupleType, DisplayMeta meta, bool simplifyName)
    {
        // Render an inner tuple WITHOUT surrounding parens — used to flatten 8-tuples
        string outer = RenderTuple(tupleType, meta, simplifyName);
        return outer.Length >= 2 ? outer[1..^1] : outer;
    }
}
