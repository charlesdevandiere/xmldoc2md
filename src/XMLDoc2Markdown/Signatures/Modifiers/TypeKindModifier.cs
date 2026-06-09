namespace XMLDoc2Markdown.Signatures.Modifiers;

/// <summary>
/// Emits the class/struct/interface/enum keyword. For class, also emits
/// static / abstract / sealed in front. For struct, prepends readonly / ref.
/// Records are detected and emitted as <c>record class</c> / <c>record struct</c>.
/// </summary>
internal static class TypeKindModifier
{
    private const string IsReadOnlyAttributeFullName =
        "System.Runtime.CompilerServices.IsReadOnlyAttribute";

    internal static SignatureBuilder AppendTypeKind(this SignatureBuilder b, Type type)
    {
        if (type.IsClass)
        {
            AppendClassKind(b, type);
        }
        else if (type.IsInterface)
        {
            b.Append("interface");
        }
        else if (type.IsEnum)
        {
            b.Append("enum");
        }
        else if (type.IsValueType)
        {
            AppendStructKind(b, type);
        }

        return b;
    }

    private static void AppendClassKind(SignatureBuilder b, Type type)
    {
        if (type.IsAbstract && type.IsSealed)
        {
            b.Append("static");
        }
        else if (type.IsAbstract)
        {
            b.Append("abstract");
        }
        else if (type.IsSealed)
        {
            b.Append("sealed");
        }
        if (RecordDetection.IsRecordClass(type))
        {
            b.Append("record");
        }
        b.Append("class");
    }

    private static void AppendStructKind(SignatureBuilder b, Type type)
    {
        if (type.HasAttribute(IsReadOnlyAttributeFullName))
        {
            b.Append("readonly");
        }
        if (type.IsByRefLike)
        {
            b.Append("ref");
        }
        if (RecordDetection.IsRecordStruct(type))
        {
            b.Append("record");
        }
        b.Append("struct");
    }
}
