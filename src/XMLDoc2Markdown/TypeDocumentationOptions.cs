namespace XMLDoc2Markdown;

internal class TypeDocumentationOptions
{
    public string ExamplesDirectory { get; set; }
    public bool GitHubPages { get; set; }
    public string BackButton { get; set; }
    public string LinkBackButton { get; set; }
    public bool HasBackButton { get; set; }
    public bool GitlabWiki { get; set; }
    public bool IncludePrivateMembers { get; set; }
    public bool ExcludeInternals { get; set; }
    public bool OnlyInternalMembers { get; set; }
    public bool FoundBackButtonTemplate { get; set; }
}
