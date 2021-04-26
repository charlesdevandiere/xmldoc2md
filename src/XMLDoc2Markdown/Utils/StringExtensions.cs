namespace XMLDoc2Markdown.Utils
{
    internal static class StringExtensions
    {
        internal static string FormatChevrons(this string value)
        {
            return value.Replace("<", "&lt;").Replace(">", "&gt;");
        }
    }
}
