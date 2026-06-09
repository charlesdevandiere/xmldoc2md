using System.Reflection;

namespace XMLDoc2Markdown.Signatures.Modifiers;

/// <summary>
/// Emits the <c>where T : …</c> clauses that follow a generic type or method
/// signature. One <c>where</c> clause per parameter that carries any constraint.
/// </summary>
internal static class GenericConstraintsModifier
{
    private const string IsUnmanagedAttributeFullName =
        "System.Runtime.CompilerServices.IsUnmanagedAttribute";

    internal static SignatureBuilder AppendGenericConstraints(this SignatureBuilder b, Type type)
        => AppendConstraints(b, type.IsGenericTypeDefinition ? type.GetGenericArguments() : []);

    internal static SignatureBuilder AppendGenericConstraints(this SignatureBuilder b, MethodBase method)
        => AppendConstraints(b, method.IsGenericMethodDefinition ? method.GetGenericArguments() : []);

    private static SignatureBuilder AppendConstraints(SignatureBuilder b, Type[] parameters)
    {
        foreach (Type param in parameters)
        {
            string? clause = BuildClause(param);
            if (clause != null)
            {
                b.Append(clause);
            }
        }
        return b;
    }

    private static string? BuildClause(Type param)
    {
        List<string> parts = [];

        GenericParameterAttributes attrs = param.GenericParameterAttributes;
        bool refConstraint = (attrs & GenericParameterAttributes.ReferenceTypeConstraint) != 0;
        bool valConstraint = (attrs & GenericParameterAttributes.NotNullableValueTypeConstraint) != 0;
        bool defaultCtor = (attrs & GenericParameterAttributes.DefaultConstructorConstraint) != 0;

        if (valConstraint)
        {
            parts.Add(IsUnmanaged(param) ? "unmanaged" : "struct");
        }
        else if (refConstraint)
        {
            parts.Add("class");
        }

        foreach (Type constraint in param.GetGenericParameterConstraints())
        {
            if (valConstraint && constraint == typeof(ValueType))
            {
                continue;
            }
            parts.Add(constraint.GetDisplayName());
        }

        if (defaultCtor && !valConstraint)
        {
            parts.Add("new()");
        }

        return parts.Count == 0
            ? null
            : $"where {param.Name} : {string.Join(", ", parts)}";
    }

    private static bool IsUnmanaged(Type param) => param.HasAttribute(IsUnmanagedAttributeFullName);
}
