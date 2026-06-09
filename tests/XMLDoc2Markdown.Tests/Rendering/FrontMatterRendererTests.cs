using MyClassLib;
using XMLDoc2Markdown.Rendering;

namespace XMLDoc2Markdown.Tests.Rendering;

[Collection(nameof(SampleAssemblyCollection))]
public class FrontMatterRendererTests
{
    [Fact]
    public void None_preset_emits_nothing()
    {
        TypeDocumentationOptions options = new() { FrontMatter = FrontMatterPreset.None };

        Assert.Equal(string.Empty, FrontMatterRenderer.ForType(typeof(MyClass), options));
        Assert.Equal(string.Empty, FrontMatterRenderer.ForIndex("MyClassLib", "index", options));
    }

    [Fact]
    public void Jekyll_type_emits_layout_and_title()
    {
        TypeDocumentationOptions options = new() { FrontMatter = FrontMatterPreset.Jekyll };

        string result = FrontMatterRenderer.ForType(typeof(MyClass), options);

        Assert.Equal("---\nlayout: default\ntitle: MyClass\n---\n\n", result);
    }

    [Fact]
    public void Jekyll_index_uses_assembly_name_as_title()
    {
        TypeDocumentationOptions options = new() { FrontMatter = FrontMatterPreset.Jekyll };

        string result = FrontMatterRenderer.ForIndex("MyClassLib", "index", options);

        Assert.Equal("---\nlayout: default\ntitle: MyClassLib\n---\n\n", result);
    }

    [Fact]
    public void JustTheDocs_type_emits_title_and_namespace_parent()
    {
        TypeDocumentationOptions options = new() { FrontMatter = FrontMatterPreset.JustTheDocs };

        string result = FrontMatterRenderer.ForType(typeof(MyClass), options);

        Assert.Equal("---\ntitle: MyClass\nparent: MyClassLib\n---\n\n", result);
    }

    [Fact]
    public void JustTheDocs_index_marks_parent_with_children()
    {
        TypeDocumentationOptions options = new() { FrontMatter = FrontMatterPreset.JustTheDocs };

        string result = FrontMatterRenderer.ForIndex("MyClassLib", "index", options);

        Assert.Equal("---\ntitle: MyClassLib\nhas_children: true\nnav_order: 1\n---\n\n", result);
    }

    [Fact]
    public void Docusaurus_type_emits_id_title_and_sidebar_label()
    {
        TypeDocumentationOptions options = new() { FrontMatter = FrontMatterPreset.Docusaurus };

        string result = FrontMatterRenderer.ForType(typeof(MyClass), options);

        Assert.Contains("id: myclasslib.myclass\n", result);
        Assert.Contains("title: MyClass\n", result);
        Assert.Contains("sidebar_label: MyClass\n", result);
    }

    [Fact]
    public void Generic_type_title_is_quoted()
    {
        TypeDocumentationOptions options = new() { FrontMatter = FrontMatterPreset.Jekyll };

        string result = FrontMatterRenderer.ForType(typeof(INumeric<>), options);

        // The '<' / '>' in INumeric<T> would break a bare YAML scalar.
        Assert.Contains("title: \"INumeric<T>\"", result);
    }

    [Fact]
    public void Custom_fields_are_merged_and_override_preset_keys()
    {
        TypeDocumentationOptions options = new()
        {
            FrontMatter = FrontMatterPreset.Jekyll,
            FrontMatterFields =
            [
                new("nav_order", "5"),
                new("title", "Overridden"),
            ]
        };

        string result = FrontMatterRenderer.ForType(typeof(MyClass), options);

        Assert.Equal("---\nlayout: default\ntitle: Overridden\nnav_order: 5\n---\n\n", result);
    }
}
