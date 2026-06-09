using System.Reflection;
using System.Xml.Linq;
using Markdown;

namespace XMLDoc2Markdown.Rendering.Sections;

internal sealed class EventSectionRenderer : MemberSectionRenderer<EventInfo>
{
    protected override string SectionHeader => "Events";
    protected override string ObsoleteDefaultMessage => "This member is obsolete.";

    internal EventSectionRenderer(
        RenderingContext context,
        XmlDocToMarkdownConverter converter,
        ExampleInjector examples)
        : base(context, converter, examples) { }

    protected override void RenderBody(IMarkdownDocument document, EventInfo member, XElement? memberDocElement)
    {
        // No event-specific sub-sections today.
    }
}
