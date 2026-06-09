using Markdown;
using XMLDoc2Markdown.Linking;

namespace XMLDoc2Markdown.Rendering;

internal static class BackButtonRenderer
{
    internal enum Position { Top, Bottom }

    internal static void Write(IMarkdownDocument document, Type type, DocumentationStructure structure, Position position)
    {
        if (position == Position.Bottom)
        {
            document.AppendHorizontalRule();
        }

        int depth = type.GetDocsFileName(structure).Count(f => f == '/');
        string route = depth > 0
            ? string.Join('/', Enumerable.Repeat("..", depth))
            : ".";
        route += "/";

        document.AppendParagraph(new MarkdownLink(new MarkdownInlineCode("< Back"), route));

        if (position == Position.Top)
        {
            document.AppendHorizontalRule();
        }
    }
}
