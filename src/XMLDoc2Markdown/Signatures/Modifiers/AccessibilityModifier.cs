using XMLDoc2Markdown.Members;

namespace XMLDoc2Markdown.Signatures.Modifiers;

internal static class AccessibilityModifier
{
    internal static SignatureBuilder AppendAccessibility(this SignatureBuilder b, Accessibility accessibility)
        => b.Append(accessibility.Print());
}
