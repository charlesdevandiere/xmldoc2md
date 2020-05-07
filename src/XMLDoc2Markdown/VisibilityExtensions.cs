namespace XMLDoc2Markdown
{
    public static class VisibilityExtensions
    {
        public static string Print(this Visibility visibility)
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
}
