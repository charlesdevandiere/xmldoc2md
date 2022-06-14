namespace XMLDoc2Markdown.Utils;

internal static class VisibilityExtensions
{
    internal static string Print(this Visibility visibility)
    {
        return visibility switch
        {
            Visibility.Public => "public",
            Visibility.Internal => "internal",
            Visibility.Protected => "protected",
            Visibility.ProtectedInternal => "protected internal",
            Visibility.Private => "private",
            _ => string.Empty
        };
    }
}
