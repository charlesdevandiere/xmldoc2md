using MyClassLib;
using XMLDoc2Markdown;
using XMLDoc2Markdown.Linking;

namespace XMLDoc2Markdown.Tests.Linking;

public class DocsFileNameProviderTests
{
    [Fact]
    public void Flat_uses_dotted_identifier()
    {
        Assert.Equal("myclasslib.myclass", typeof(MyClass).GetDocsFileName(DocumentationStructure.Flat));
    }

    [Fact]
    public void Tree_replaces_dots_with_slashes()
    {
        Assert.Equal("myclasslib/myclass", typeof(MyClass).GetDocsFileName(DocumentationStructure.Tree));
    }

    [Fact]
    public void Flat_nested_type_keeps_dots()
    {
        Assert.Equal(
            "myclasslib.myclass.nested",
            typeof(MyClass.Nested).GetDocsFileName(DocumentationStructure.Flat));
    }

    [Fact]
    public void Tree_nested_type_gets_its_own_subdirectory()
    {
        Assert.Equal(
            "myclasslib/myclass/nested",
            typeof(MyClass.Nested).GetDocsFileName(DocumentationStructure.Tree));
    }
}
