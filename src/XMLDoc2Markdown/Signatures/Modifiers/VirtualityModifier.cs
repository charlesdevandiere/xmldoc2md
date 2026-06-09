using System.Reflection;

namespace XMLDoc2Markdown.Signatures.Modifiers;

/// <summary>
/// Emits <c>virtual</c> / <c>override</c> / <c>sealed override</c> / <c>new</c>
/// for methods, properties (via their accessors), indexers, and events.
/// <para>
/// <c>abstract</c> is owned by <see cref="AbstractModifier"/> and emitted before
/// this modifier; if a method is abstract, no virtuality token is added here.
/// </para>
/// </summary>
internal static class VirtualityModifier
{
    private const BindingFlags AllInstanceAndStatic =
        BindingFlags.Public | BindingFlags.NonPublic |
        BindingFlags.Instance | BindingFlags.Static;

    /// <summary>
    /// Append the virtuality token for a single method.
    /// </summary>
    internal static SignatureBuilder AppendVirtuality(this SignatureBuilder b, MethodInfo method)
    {
        if (method.IsAbstract)
        {
            return b;
        }

        string? token = ClassifyClassMethod(method) ?? ClassifyInterfaceMethod(method);
        return token == null ? b : b.Append(token);
    }

    /// <summary>
    /// Append the virtuality token for a property — picks the strongest token across
    /// get/set accessors.
    /// </summary>
    internal static SignatureBuilder AppendVirtuality(this SignatureBuilder b, PropertyInfo property)
        => AppendStrongest(b, property.GetMethod, property.SetMethod);

    internal static SignatureBuilder AppendVirtuality(this SignatureBuilder b, EventInfo @event)
        => AppendStrongest(b, @event.AddMethod, @event.RemoveMethod);

    private static SignatureBuilder AppendStrongest(SignatureBuilder b, MethodInfo? a, MethodInfo? c)
    {
        string? tokenA = a == null || a.IsAbstract ? null : ClassifyClassMethod(a) ?? ClassifyInterfaceMethod(a);
        string? tokenB = c == null || c.IsAbstract ? null : ClassifyClassMethod(c) ?? ClassifyInterfaceMethod(c);
        string? token = Strongest(tokenA, tokenB);
        return token == null ? b : b.Append(token);
    }

    private static string? Strongest(string? a, string? b)
    {
        if (a == null) return b;
        if (b == null) return a;
        return Rank(a) >= Rank(b) ? a : b;
    }

    private static int Rank(string token) => token switch
    {
        "sealed override" => 4,
        "override" => 3,
        "virtual" => 2,
        "new" => 1,
        _ => 0,
    };

    private static string? ClassifyClassMethod(MethodInfo method)
    {
        Type? declaring = method.DeclaringType;
        if (declaring == null || !(declaring.IsClass || declaring.IsValueType) || declaring.IsInterface)
        {
            return null;
        }

        if (method.IsVirtual)
        {
            MethodInfo baseDefinition = method.GetBaseDefinition();
            bool overrides = baseDefinition != method && baseDefinition.DeclaringType != declaring;
            if (overrides)
            {
                return method.IsFinal ? "sealed override" : "override";
            }
            // virtual is only meaningful on a class — structs cannot declare virtual instance methods
            if (declaring.IsClass && !method.IsFinal)
            {
                return "virtual";
            }
        }

        return DetectNewHiding(method, declaring) ? "new" : null;
    }

    private static string? ClassifyInterfaceMethod(MethodInfo method)
    {
        Type? declaring = method.DeclaringType;
        if (declaring == null || !declaring.IsInterface)
        {
            return null;
        }

        // On interfaces: abstract members get nothing extra (already filtered above).
        // Members with a body are default impls — emit `virtual`.
        if (!method.IsAbstract)
        {
            return "virtual";
        }
        return null;
    }

    private static bool DetectNewHiding(MethodInfo method, Type declaring)
    {
        if (method.IsConstructor)
        {
            return false;
        }

        Type? baseType = declaring.BaseType;
        Type[] paramTypes = method.GetParameters()
            .Select(p => p.ParameterType)
            .ToArray();

        while (baseType != null && baseType != typeof(object))
        {
            MethodInfo? candidate = baseType.GetMethod(
                method.Name,
                AllInstanceAndStatic,
                binder: null,
                types: paramTypes,
                modifiers: null);

            if (candidate != null && !candidate.IsPrivate)
            {
                // If the current method is part of the same virtual chain we'd have
                // returned `override` already — so reaching here means hiding.
                return true;
            }

            baseType = baseType.BaseType;
        }

        return false;
    }
}
