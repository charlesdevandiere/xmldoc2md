using System.Reflection;
using System.Runtime.CompilerServices;

namespace XMLDoc2Markdown.Signatures;

/// <summary>
/// Per-member display metadata that augments a bare <see cref="Type"/> with the
/// nullable-reference annotation and named-tuple element names captured in the
/// metadata. Threaded through the signature builders so that <c>string?</c>,
/// <c>int?</c>, and <c>(int X, string Y)</c> render the way C# does.
/// </summary>
internal sealed class DisplayMeta
{
    private static readonly DisplayMeta EmptyInstance = new(null, (IReadOnlyList<string?>)[]);

    internal NullabilityInfo? Nullability { get; }

    /// <summary>
    /// Flat, depth-first tuple element names — shared across every recursive
    /// descent so that <c>(int X, (string A, string B) Y)</c> consumes the names
    /// in declaration order. Designed for single-threaded, single-pass rendering:
    /// each <see cref="For(System.Reflection.ParameterInfo, System.Reflection.NullabilityInfoContext)"/>
    /// (or sibling factory) builds a fresh queue, and <see cref="GenericArg(int)"/>
    /// shares it by reference so children consume the same stream.
    /// </summary>
    internal Queue<string?> TupleNames { get; }

    private DisplayMeta(NullabilityInfo? nullability, IReadOnlyList<string?> tupleNames)
    {
        this.Nullability = nullability;
        this.TupleNames = new Queue<string?>(tupleNames);
    }

    private DisplayMeta(NullabilityInfo? nullability, Queue<string?> sharedTupleNames)
    {
        this.Nullability = nullability;
        this.TupleNames = sharedTupleNames;
    }

    internal static DisplayMeta Empty => EmptyInstance;

    internal DisplayMeta GenericArg(int index)
    {
        NullabilityInfo? sub = this.Nullability != null
            && this.Nullability.GenericTypeArguments.Length > index
            ? this.Nullability.GenericTypeArguments[index]
            : null;
        return new DisplayMeta(sub, this.TupleNames);
    }

    internal DisplayMeta ForElement()
    {
        NullabilityInfo? sub = this.Nullability?.ElementType;
        return new DisplayMeta(sub, this.TupleNames);
    }

    internal static DisplayMeta For(ParameterInfo param, NullabilityInfoContext ctx)
        => new(TryNullability(param, ctx), GetTupleNames(param));

    internal static DisplayMeta For(PropertyInfo property, NullabilityInfoContext ctx)
        => new(TryNullability(property, ctx), GetTupleNames(property));

    internal static DisplayMeta For(FieldInfo field, NullabilityInfoContext ctx)
        => new(TryNullability(field, ctx), GetTupleNames(field));

    internal static DisplayMeta For(EventInfo @event, NullabilityInfoContext ctx)
        => new(TryNullability(@event, ctx), GetTupleNames(@event));

    /// <summary>Metadata for a method's return type.</summary>
    internal static DisplayMeta ForReturn(MethodInfo method, NullabilityInfoContext ctx)
        => For(method.ReturnParameter, ctx);

    private static NullabilityInfo? TryNullability(ParameterInfo p, NullabilityInfoContext ctx)
    {
        try { return ctx.Create(p); } catch { return null; }
    }

    private static NullabilityInfo? TryNullability(PropertyInfo p, NullabilityInfoContext ctx)
    {
        try { return ctx.Create(p); } catch { return null; }
    }

    private static NullabilityInfo? TryNullability(FieldInfo f, NullabilityInfoContext ctx)
    {
        try { return ctx.Create(f); } catch { return null; }
    }

    private static NullabilityInfo? TryNullability(EventInfo e, NullabilityInfoContext ctx)
    {
        try { return ctx.Create(e); } catch { return null; }
    }

    private static IReadOnlyList<string?> GetTupleNames(ICustomAttributeProvider holder)
    {
        TupleElementNamesAttribute? attr = holder
            .GetCustomAttributes(typeof(TupleElementNamesAttribute), inherit: false)
            .OfType<TupleElementNamesAttribute>()
            .FirstOrDefault();
        return attr?.TransformNames?.ToArray() ?? [];
    }
}
