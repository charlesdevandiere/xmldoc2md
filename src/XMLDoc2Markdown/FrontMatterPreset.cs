namespace XMLDoc2Markdown;

/// <summary>
/// Documentation system whose YAML front matter the tool should emit at the top
/// of every generated page. <see cref="None"/> (the default) emits nothing.
/// </summary>
internal enum FrontMatterPreset
{
    None,
    Jekyll,
    JustTheDocs,
    Docusaurus
}
