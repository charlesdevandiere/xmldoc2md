namespace XMLDoc2Markdown
{
    public static class StringExtensions
    {
        public static string FormatChevrons(this string value)
        {
            return value.Replace("<", "&lt;").Replace(">", "&gt;");
        }
    }
}
