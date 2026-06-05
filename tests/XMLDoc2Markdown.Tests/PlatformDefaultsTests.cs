namespace XMLDoc2Markdown.Tests;

public class PlatformDefaultsTests
{
    [Fact]
    public void For_returns_the_preset_for_each_platform()
    {
        // platform => (NoExtension, NoPrefix, FrontMatter)
        Assert.Equal(new(false, false, FrontMatterPreset.None), PlatformDefaults.For(Platform.Plain));
        Assert.Equal(new(true, false, FrontMatterPreset.Jekyll), PlatformDefaults.For(Platform.GitHubPages));
        Assert.Equal(new(true, false, FrontMatterPreset.Jekyll), PlatformDefaults.For(Platform.Jekyll));
        Assert.Equal(new(true, true, FrontMatterPreset.None), PlatformDefaults.For(Platform.GitlabWiki));
        Assert.Equal(new(true, false, FrontMatterPreset.JustTheDocs), PlatformDefaults.For(Platform.JustTheDocs));
        Assert.Equal(new(false, false, FrontMatterPreset.Docusaurus), PlatformDefaults.For(Platform.Docusaurus));
    }

    [Fact]
    public void GitHubPages_and_Jekyll_resolve_identically()
    {
        Assert.Equal(PlatformDefaults.For(Platform.GitHubPages), PlatformDefaults.For(Platform.Jekyll));
    }

    [Fact]
    public void Resolve_keeps_platform_defaults_when_no_override_is_given()
    {
        PlatformSettings settings = PlatformDefaults.Resolve(Platform.GitHubPages, null, null, null);

        Assert.True(settings.NoExtension);
        Assert.False(settings.NoPrefix);
        Assert.Equal(FrontMatterPreset.Jekyll, settings.FrontMatter);
    }

    [Fact]
    public void Resolve_lets_each_explicit_override_win()
    {
        // GitHub Pages would strip the extension and emit Jekyll front matter;
        // every knob is overridden here.
        PlatformSettings settings = PlatformDefaults.Resolve(
            Platform.GitHubPages,
            noExtension: false,
            noPrefix: true,
            frontMatter: FrontMatterPreset.None);

        Assert.False(settings.NoExtension);
        Assert.True(settings.NoPrefix);
        Assert.Equal(FrontMatterPreset.None, settings.FrontMatter);
    }

    [Fact]
    public void Resolve_overrides_independently()
    {
        // Only the front matter is overridden; link defaults stay from the platform.
        PlatformSettings settings = PlatformDefaults.Resolve(
            Platform.Docusaurus,
            noExtension: null,
            noPrefix: null,
            frontMatter: FrontMatterPreset.None);

        Assert.False(settings.NoExtension);
        Assert.False(settings.NoPrefix);
        Assert.Equal(FrontMatterPreset.None, settings.FrontMatter);
    }
}
