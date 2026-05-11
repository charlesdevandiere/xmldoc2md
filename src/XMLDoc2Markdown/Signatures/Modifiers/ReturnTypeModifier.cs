namespace XMLDoc2Markdown.Signatures.Modifiers;

internal static class ReturnTypeModifier
{
    internal static SignatureBuilder AppendReturnType(this SignatureBuilder b, Type? returnType)
    {
        if (returnType is null)
        {
            return b;
        }
        return b.Append(returnType.GetDisplayName(simplifyName: true));
    }
}
