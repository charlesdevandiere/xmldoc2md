using System.Text;
using XMLDoc2Markdown.Linking;
using XMLDoc2Markdown.Signatures;

namespace XMLDoc2Markdown.Rendering;

/// <summary>
/// Emits the YAML front matter block (between <c>---</c> fences) that the
/// targeted documentation system expects.
/// <para>
/// Each preset contributes a set of computed keys (title / parent / id, derived
/// from the type tree the tool already knows); <see cref="TypeDocumentationOptions.FrontMatterFields"/>
/// are then merged on top so a regeneration never clobbers the user's own keys.
/// Returns <see cref="string.Empty"/> when the preset is <see cref="FrontMatterPreset.None"/>.
/// </para>
/// </summary>
internal static class FrontMatterRenderer
{
    /// <summary>Front matter for a single type's page.</summary>
    internal static string ForType(Type type, TypeDocumentationOptions options)
    {
        ArgumentNullException.ThrowIfNull(type);
        ArgumentNullException.ThrowIfNull(options);

        if (options.FrontMatter == FrontMatterPreset.None)
        {
            return string.Empty;
        }

        string title = type.GetDisplayName();
        List<KeyValuePair<string, string>> entries = [];

        switch (options.FrontMatter)
        {
            case FrontMatterPreset.Jekyll:
                entries.Add(new("layout", Quote("default")));
                entries.Add(new("title", Quote(title)));
                break;
            case FrontMatterPreset.JustTheDocs:
                entries.Add(new("title", Quote(title)));
                if (!string.IsNullOrEmpty(type.Namespace))
                {
                    entries.Add(new("parent", Quote(type.Namespace)));
                }
                break;
            case FrontMatterPreset.Docusaurus:
                entries.Add(new("id", Quote(type.GetDocsFileName(options.Structure))));
                entries.Add(new("title", Quote(title)));
                entries.Add(new("sidebar_label", Quote(title)));
                break;
        }

        return Build(entries, options.FrontMatterFields);
    }

    /// <summary>Front matter for the namespace index page.</summary>
    internal static string ForIndex(string assemblyName, string indexPageName, TypeDocumentationOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        if (options.FrontMatter == FrontMatterPreset.None)
        {
            return string.Empty;
        }

        List<KeyValuePair<string, string>> entries = [];

        switch (options.FrontMatter)
        {
            case FrontMatterPreset.Jekyll:
                entries.Add(new("layout", Quote("default")));
                entries.Add(new("title", Quote(assemblyName)));
                break;
            case FrontMatterPreset.JustTheDocs:
                entries.Add(new("title", Quote(assemblyName)));
                entries.Add(new("has_children", "true"));
                entries.Add(new("nav_order", "1"));
                break;
            case FrontMatterPreset.Docusaurus:
                entries.Add(new("id", Quote(indexPageName)));
                entries.Add(new("title", Quote(assemblyName)));
                break;
        }

        return Build(entries, options.FrontMatterFields);
    }

    private static string Build(
        List<KeyValuePair<string, string>> presetEntries,
        IReadOnlyList<KeyValuePair<string, string>> customFields)
    {
        // Preserve insertion order; custom fields override a preset key in place.
        List<string> order = [];
        Dictionary<string, string> byKey = new(StringComparer.Ordinal);

        foreach ((string key, string value) in presetEntries.Concat(customFields))
        {
            if (!byKey.ContainsKey(key))
            {
                order.Add(key);
            }

            byKey[key] = value;
        }

        StringBuilder sb = new();
        sb.Append("---\n");
        foreach (string key in order)
        {
            sb.Append(key).Append(": ").Append(byKey[key]).Append('\n');
        }

        sb.Append("---\n\n");
        return sb.ToString();
    }

    private static readonly char[] yamlSpecials =
        [':', '#', '<', '>', '{', '}', '[', ']', ',', '&', '*', '!', '|', '\'', '"', '%', '@', '`'];

    // Double-quote a scalar when leaving it bare could break the YAML parse
    // (generics like MyClass<T>, tuple names, leading/trailing space, empties).
    private static string Quote(string value)
    {
        bool needsQuote = value.Length == 0
            || value != value.Trim()
            || value.IndexOfAny(yamlSpecials) >= 0;

        return needsQuote
            ? "\"" + value.Replace("\\", "\\\\").Replace("\"", "\\\"") + "\""
            : value;
    }
}
