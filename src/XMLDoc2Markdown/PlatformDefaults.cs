namespace XMLDoc2Markdown;

/// <summary>
/// Resolved link-rewriting and front-matter settings for a documentation run.
/// </summary>
/// <param name="NoExtension">Strip the <c>.md</c> extension from generated links.</param>
/// <param name="NoPrefix">Strip the <c>./</c> relative prefix from generated links.</param>
/// <param name="FrontMatter">Front matter preset to emit at the top of every page.</param>
internal readonly record struct PlatformSettings(bool NoExtension, bool NoPrefix, FrontMatterPreset FrontMatter);

/// <summary>
/// Turns a <see cref="Platform"/> preset plus optional per-knob overrides into
/// the concrete <see cref="PlatformSettings"/> the renderer consumes. The
/// platform supplies the defaults; any override that is non-<c>null</c> wins.
/// </summary>
internal static class PlatformDefaults
{
    /// <summary>The link/front-matter defaults a platform implies before overrides.</summary>
    internal static PlatformSettings For(Platform platform) => platform switch
    {
        // strip .md, keep ./, Jekyll front matter
        Platform.GitHubPages => new(NoExtension: true, NoPrefix: false, FrontMatter: FrontMatterPreset.Jekyll),
        Platform.Jekyll => new(NoExtension: true, NoPrefix: false, FrontMatter: FrontMatterPreset.Jekyll),
        // strip .md and ./, no front matter
        Platform.GitlabWiki => new(NoExtension: true, NoPrefix: true, FrontMatter: FrontMatterPreset.None),
        // strip .md, keep ./, Just the Docs front matter
        Platform.JustTheDocs => new(NoExtension: true, NoPrefix: false, FrontMatter: FrontMatterPreset.JustTheDocs),
        // keep .md and ./, Docusaurus front matter
        Platform.Docusaurus => new(NoExtension: false, NoPrefix: false, FrontMatter: FrontMatterPreset.Docusaurus),
        // Plain: raw Markdown, no rewriting, no front matter
        _ => new(NoExtension: false, NoPrefix: false, FrontMatter: FrontMatterPreset.None)
    };

    /// <summary>
    /// Resolves the effective settings: start from the platform preset, then let
    /// each explicit override (non-<c>null</c>) take precedence.
    /// </summary>
    internal static PlatformSettings Resolve(
        Platform platform,
        bool? noExtension,
        bool? noPrefix,
        FrontMatterPreset? frontMatter)
    {
        PlatformSettings defaults = For(platform);
        return new(
            NoExtension: noExtension ?? defaults.NoExtension,
            NoPrefix: noPrefix ?? defaults.NoPrefix,
            FrontMatter: frontMatter ?? defaults.FrontMatter);
    }
}
