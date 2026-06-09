using XMLDoc2Markdown.Members;

namespace XMLDoc2Markdown.Signatures.Modifiers;

internal static class AccessibilityModifier
{
    internal static SignatureBuilder AppendAccessibility(this SignatureBuilder b, Accessibility accessibility)
        => b.Append(accessibility.Print());

    /// <summary>
    /// Suppresses the keyword when the declaring scope is an interface (members
    /// are implicitly <c>public</c> there).
    /// </summary>
    internal static SignatureBuilder AppendAccessibilityUnlessInterface(
        this SignatureBuilder b,
        Accessibility accessibility,
        bool isInterface)
        => isInterface ? b : b.AppendAccessibility(accessibility);
}
