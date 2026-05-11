namespace XMLDoc2Markdown.Signatures.Modifiers;

/// <summary>
/// Emits the class/struct/interface/enum keyword. For class, also emits
/// static / abstract / sealed in front. Extend here for record / readonly / ref.
/// </summary>
internal static class TypeKindModifier
{
    internal static SignatureBuilder AppendTypeKind(this SignatureBuilder b, Type type)
    {
        if (type.IsClass)
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
            b.Append("class");
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
            b.Append("struct");
        }

        return b;
    }
}
