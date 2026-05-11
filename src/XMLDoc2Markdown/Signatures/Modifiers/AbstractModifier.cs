namespace XMLDoc2Markdown.Signatures.Modifiers;

internal static class AbstractModifier
{
    internal static SignatureBuilder AppendIfAbstract(this SignatureBuilder b, bool isAbstract)
        => isAbstract ? b.Append("abstract") : b;
}
