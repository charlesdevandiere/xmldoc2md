namespace XMLDoc2Markdown.Utils;

internal static class StringExtensions
{
    internal static string FormatChevrons(this string value)
    {
        return value.Replace("<", "&lt;").Replace(">", "&gt;");
    }

    internal static string ToAnchorLink(this string value)
    {
        return value
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
}
