namespace XMLDoc2Markdown.Signatures.Modifiers;

internal static class ReturnTypeModifier
{
    internal static SignatureBuilder AppendReturnType(this SignatureBuilder b, Type? returnType)
        => AppendReturnType(b, returnType, DisplayMeta.Empty);

    internal static SignatureBuilder AppendReturnType(this SignatureBuilder b, Type? returnType, DisplayMeta meta)
    {
        if (returnType is null)
        {
            return b;
        }
        return b.Append(returnType.GetDisplayName(meta, simplifyName: true));
    }
}
