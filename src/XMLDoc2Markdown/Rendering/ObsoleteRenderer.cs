using System.Reflection;
using Markdown;

namespace XMLDoc2Markdown.Rendering;

internal static class ObsoleteRenderer
{
    internal static void Write(IMarkdownDocument document, MemberInfo member, string defaultMessage)
    {
        ObsoleteAttribute? attribute = member.GetCustomAttributes<ObsoleteAttribute>().FirstOrDefault();
        if (attribute is null)
        {
            return;
        }

        document.AppendHeader("Caution", 4);
        document.AppendParagraph(string.IsNullOrEmpty(attribute.Message) ? defaultMessage : attribute.Message);
        document.AppendHorizontalRule();
    }
}
