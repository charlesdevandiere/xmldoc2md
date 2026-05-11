namespace XMLDoc2Markdown.Signatures.Modifiers;

internal static class StaticModifier
{
    internal static SignatureBuilder AppendIfStatic(this SignatureBuilder b, bool isStatic)
        => isStatic ? b.Append("static") : b;
}
