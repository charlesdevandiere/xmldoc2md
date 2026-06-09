namespace XMLDoc2Markdown.Signatures.Modifiers;

internal static class AbstractModifier
{
    /// <summary>
    /// Emits <c>abstract</c> when warranted. Plain instance interface members
    /// are implicitly abstract, so the keyword is suppressed for them; the
    /// <c>static abstract</c> interface form keeps the keyword.
    /// </summary>
    internal static SignatureBuilder AppendIfAbstract(
        this SignatureBuilder b,
        bool isAbstract,
        bool isInterface,
        bool isStatic)
        => isAbstract && (!isInterface || isStatic) ? b.Append("abstract") : b;
}
