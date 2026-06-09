namespace XMLDoc2Markdown;

/// <summary>
/// Target documentation host. A platform is a preset that picks sensible
/// defaults for link rewriting (<c>--link-extension</c> / <c>--link-prefix</c>)
/// and front matter (<c>--front-matter</c>); each of those can still be
/// overridden explicitly on the command line.
/// </summary>
internal enum Platform
{
    /// <summary>Raw Markdown: keep the <c>.md</c> extension and <c>./</c> prefix, no front matter.</summary>
    Plain,

    /// <summary>GitHub Pages (Jekyll): strip <c>.md</c>, keep <c>./</c>, emit Jekyll front matter.</summary>
    GitHubPages,

    /// <summary>Jekyll (any host): strip <c>.md</c>, keep <c>./</c>, emit Jekyll front matter. Equivalent to <see cref="GitHubPages"/>.</summary>
    Jekyll,

    /// <summary>GitLab wiki: strip both <c>.md</c> and the <c>./</c> prefix, no front matter.</summary>
    GitlabWiki,

    /// <summary>Just the Docs (GitHub Pages theme): strip <c>.md</c>, keep <c>./</c>, emit Just the Docs front matter.</summary>
    JustTheDocs,

    /// <summary>Docusaurus: keep <c>.md</c> and <c>./</c>, emit Docusaurus front matter.</summary>
    Docusaurus
}
