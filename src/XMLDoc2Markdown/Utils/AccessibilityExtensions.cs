namespace XMLDoc2Markdown.Utils;

internal static class AccessibilityExtensions
{
    internal static string Print(this Accessibility accessibility)
    {
        return accessibility switch
        {
            Accessibility.Public => "public",
            Accessibility.Internal => "internal",
            Accessibility.Protected => "protected",
            Accessibility.ProtectedInternal => "protected internal",
            Accessibility.Private => "private",
            _ => string.Empty
        };
    }
}
