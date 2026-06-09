namespace XMLDoc2Markdown.Linking;

internal static class AnchorBuilder
{
    internal static string ToAnchorLink(this string value) => value
        .Replace("(", string.Empty)
        .Replace(")", string.Empty)
        .Replace("<", string.Empty)
        .Replace(">", string.Empty)
        .Replace("[", string.Empty)
        .Replace("]", string.Empty)
        .Replace(",", string.Empty)
        .Replace(' ', '-')
        .ToLower();
}
