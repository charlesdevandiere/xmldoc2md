using System.Reflection;
using Markdown;
using XMLDoc2Markdown.Signatures;

namespace XMLDoc2Markdown.Rendering;

internal static class InheritanceRenderer
{
    internal static void Write(IMarkdownDocument document, RenderingContext context)
    {
        Type type = context.Type;
        List<string> lines = [];

        if (type.BaseType != null)
        {
            IEnumerable<MarkdownInlineElement> hierarchy = type.GetInheritanceHierarchy()
                .Reverse()
                .Select(t => context.DocsLink(t));
            lines.Add($"Inheritance {string.Join(" → ", hierarchy)}");
        }

        Type[] interfaces = type.GetInterfaces();
        if (interfaces.Length > 0)
        {
            IEnumerable<MarkdownInlineElement> implements = interfaces.Select(i => context.DocsLink(i));
            lines.Add($"Implements {string.Join(", ", implements)}");
        }

        IEnumerable<Attribute> attributes = type.GetCustomAttributes();
        if (attributes.Any())
        {
            IEnumerable<MarkdownInlineElement> links = attributes.Select(a => context.DocsLink(a.GetType()));
            lines.Add($"Attributes {string.Join(", ", links)}");
        }

        if (lines.Count > 0)
        {
            document.AppendParagraph(string.Join($"<br>{Environment.NewLine}", lines));
        }
    }

}
