using XMLDoc2Markdown.Members;

namespace XMLDoc2Markdown;

internal class TypeDocumentationOptions
{
    internal Accessibility MemberAccessibilityLevel { get; set; } = Accessibility.Public;
    internal bool BackButton { get; set; }
    internal string? ExamplesDirectory { get; set; }
    internal bool NoExtension { get; set; }
    internal bool NoPrefix { get; set; }
    internal DocumentationStructure Structure { get; set; }
    internal FrontMatterPreset FrontMatter { get; set; }

    /// <summary>
    /// Extra front matter key/value pairs merged on top of the preset on every
    /// page. Values are emitted verbatim (the caller is responsible for valid YAML),
    /// and a custom key overrides a preset key of the same name.
    /// </summary>
    internal IReadOnlyList<KeyValuePair<string, string>> FrontMatterFields { get; set; } = [];
}
