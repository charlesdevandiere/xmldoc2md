using System.Globalization;
using System.Reflection;
using System.Text;

namespace XMLDoc2Markdown.Signatures.Modifiers;

/// <summary>
/// Renders the <c>(...)</c> parameter list of a method or constructor.
/// Each parameter is prefixed with its passing modifier (<c>ref</c>, <c>in</c>,
/// <c>out</c>, <c>params</c>) and, when <paramref name="full"/> is true, suffixed
/// with its default value (<c>= literal</c>).
/// </summary>
internal static class ParameterListModifier
{
    internal static SignatureBuilder AppendParameterList(this SignatureBuilder b, MethodBase methodBase, bool full)
        => AppendParameterList(b, methodBase, null, full);

    internal static SignatureBuilder AppendParameterList(this SignatureBuilder b, MethodBase methodBase, NullabilityInfoContext? nullCtx, bool full)
    {
        ParameterInfo[] @params = methodBase.GetParameters();
        IEnumerable<string> formatted = @params.Select(p => FormatParameter(p, nullCtx, full));
        return b.AppendJoined($"({string.Join(", ", formatted)})");
    }

    internal static string FormatParameter(ParameterInfo param, NullabilityInfoContext? nullCtx, bool full)
    {
        StringBuilder sb = new();

        string? passingModifier = GetPassingModifier(param);
        if (passingModifier != null)
        {
            sb.Append(passingModifier).Append(' ');
        }

        Type type = param.ParameterType.IsByRef
            ? param.ParameterType.GetElementType()!
            : param.ParameterType;

        DisplayMeta meta = BuildMeta(param, nullCtx);
        sb.Append(type.GetDisplayName(meta, simplifyName: full));

        if (full)
        {
            sb.Append(' ').Append(param.Name);

            if (param.HasDefaultValue)
            {
                sb.Append(" = ").Append(FormatDefault(param.DefaultValue, param.ParameterType));
            }
        }

        return sb.ToString();
    }

    /// <summary>
    /// Returns the passing-mode keyword for a parameter (<c>ref</c>, <c>in</c>,
    /// <c>out</c>, <c>params</c>), or <see langword="null"/> if the parameter has
    /// no passing modifier.
    /// </summary>
    internal static string? GetPassingModifier(ParameterInfo param)
    {
        if (param.IsDefined(typeof(ParamArrayAttribute), inherit: false))
        {
            return "params";
        }
        if (param.IsOut)
        {
            return "out";
        }
        if (param.ParameterType.IsByRef)
        {
            return param.IsIn ? "in" : "ref";
        }
        return null;
    }

    private static DisplayMeta BuildMeta(ParameterInfo param, NullabilityInfoContext? nullCtx)
    {
        if (nullCtx == null)
        {
            return DisplayMeta.Empty;
        }
        DisplayMeta meta = DisplayMeta.For(param, nullCtx);
        // The compiler annotates the ByRef wrapper; the underlying type's nullability
        // sits one level in for ref/in/out parameters.
        return param.ParameterType.IsByRef ? meta.ForElement() : meta;
    }

    private static string FormatDefault(object? value, Type parameterType)
    {
        if (value == null)
        {
            return "null";
        }

        Type underlying = parameterType.IsByRef
            ? parameterType.GetElementType()!
            : parameterType;
        Type nullableUnwrapped = Nullable.GetUnderlyingType(underlying) ?? underlying;

        if (nullableUnwrapped.IsEnum)
        {
            string? name = Enum.GetName(nullableUnwrapped, value);
            return name != null
                ? $"{nullableUnwrapped.GetDisplayName(simplifyName: false)}.{name}"
                : Convert.ToString(value, CultureInfo.InvariantCulture) ?? "default";
        }

        return value switch
        {
            string s => $"\"{s}\"",
            char c => $"'{c}'",
            bool b => b ? "true" : "false",
            float f => f.ToString("R", CultureInfo.InvariantCulture) + "f",
            double d => d.ToString("R", CultureInfo.InvariantCulture) + "d",
            decimal m => m.ToString(CultureInfo.InvariantCulture) + "m",
            IFormattable f => f.ToString(null, CultureInfo.InvariantCulture),
            _ => value.ToString() ?? "default",
        };
    }
}
