using XMLDoc2Markdown.Utils;

namespace XMLDoc2Markdown;

internal class TypeDocumentationOptions
{
    internal Accessibility MemberAccessibilityLevel { get; set; } = Accessibility.Public;
    internal bool BackButton { get; set; }
    internal string? ExamplesDirectory { get; set; }
    internal bool GitHubPages { get; set; }
    internal bool GitlabWiki { get; set; }
    internal DocumentationStructure Structure { get; set; }
}
