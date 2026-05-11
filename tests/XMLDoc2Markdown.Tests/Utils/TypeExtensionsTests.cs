using MyClassLib;
using MyClassLib.SubNamespace;
using XMLDoc2Markdown.Linking;
using XMLDoc2Markdown.Members;
using XMLDoc2Markdown.Signatures;

namespace XMLDoc2Markdown.Tests.Utils;

[Collection(nameof(SampleAssemblyCollection))]
public class TypeExtensionsTests
{
    [Theory]
    [InlineData(typeof(void), "void")]
    [InlineData(typeof(object), "object")]
    [InlineData(typeof(bool), "bool")]
    [InlineData(typeof(int), "int")]
    [InlineData(typeof(long), "long")]
    [InlineData(typeof(double), "double")]
    [InlineData(typeof(string), "string")]
    [InlineData(typeof(char), "char")]
    public void GetSimplifiedName_returns_csharp_keyword(Type type, string expected)
    {
        Assert.Equal(expected, type.GetSimplifiedName());
    }

    [Fact]
    public void GetSimplifiedName_falls_back_to_type_name_for_non_primitives()
    {
        Assert.Equal(nameof(MyClass), typeof(MyClass).GetSimplifiedName());
    }

    [Fact]
    public void GetAccessibility_distinguishes_public_from_internal()
    {
        Assert.Equal(Accessibility.Public, typeof(MyClass).GetAccessibility());
        // nested non-public types report Internal here (only IsPublic is checked at the top-level)
    }

    [Fact]
    public void GetDisplayName_for_open_generic_includes_type_parameter()
    {
        Assert.Equal("GenericClass<T>", typeof(GenericClass<>).GetDisplayName());
    }

    [Fact]
    public void GetDisplayName_for_closed_generic_uses_argument_names()
    {
        Assert.Equal("GenericClass<Int32>", typeof(GenericClass<int>).GetDisplayName());
        Assert.Equal("GenericClass<int>", typeof(GenericClass<int>).GetDisplayName(simplifyName: true));
    }

    [Fact]
    public void GetSignature_class_full_includes_keyword_and_inheritance()
    {
        string signature = typeof(MyClass).GetSignature(full: true);

        Assert.StartsWith("public class MyClass", signature);
        Assert.Contains("IMyInterface", signature);
    }

    [Fact]
    public void GetSignature_abstract_class_emits_abstract_keyword()
    {
        Assert.Contains("abstract class MyAbstractClass", typeof(MyAbstractClass).GetSignature(full: true));
    }

    [Fact]
    public void GetSignature_static_class_emits_static_keyword()
    {
        Assert.Contains("static class MyStaticClass", typeof(MyStaticClass).GetSignature(full: true));
    }

    [Fact]
    public void GetSignature_interface_emits_interface_keyword()
    {
        Assert.StartsWith("public interface IMyInterface", typeof(IMyInterface).GetSignature(full: true));
    }

    [Fact]
    public void GetSignature_enum_emits_enum_keyword()
    {
        Assert.StartsWith("public enum MyEnum", typeof(MyEnum).GetSignature(full: true));
    }

    [Fact]
    public void GetSignature_short_form_omits_access_and_kind_modifiers_but_keeps_inheritance()
    {
        // Short form drops `public class` but still appends `: BaseType, Interfaces`.
        Assert.Equal("MyClass : IMyInterface", typeof(MyClass).GetSignature(full: false));
        Assert.Equal("GenericClass<T>", typeof(GenericClass<>).GetSignature(full: false));
    }

    [Theory]
    [InlineData((int)DocumentationStructure.Flat, "myclasslib.myclass")]
    [InlineData((int)DocumentationStructure.Tree, "myclasslib/myclass")]
    public void GetDocsFileName_respects_structure(int structure, string expected)
    {
        Assert.Equal(expected, typeof(MyClass).GetDocsFileName((DocumentationStructure)structure));
    }

    [Fact]
    public void GetDocsFileName_replaces_generic_arity_backtick_with_dash()
    {
        Assert.Equal(
            "myclasslib.subnamespace.genericclass-1",
            typeof(GenericClass<>).GetDocsFileName(DocumentationStructure.Flat));
    }

    [Fact]
    public void GetInternalDocsUrl_default_has_dot_slash_prefix_and_md_extension()
    {
        string url = typeof(MyClass).GetInternalDocsUrl(DocumentationStructure.Flat);
        Assert.Equal("./myclasslib.myclass.md", url);
    }

    [Fact]
    public void GetInternalDocsUrl_no_extension_for_github_pages()
    {
        string url = typeof(MyClass).GetInternalDocsUrl(DocumentationStructure.Flat, noExtension: true);
        Assert.Equal("./myclasslib.myclass", url);
    }

    [Fact]
    public void GetInternalDocsUrl_no_prefix_for_gitlab_wiki()
    {
        string url = typeof(MyClass).GetInternalDocsUrl(
            DocumentationStructure.Flat,
            noExtension: true,
            noPrefix: true);
        Assert.Equal("myclasslib.myclass", url);
    }

    [Fact]
    public void GetMSDocsUrl_for_mscorlib_type()
    {
        string url = typeof(string).GetMSDocsUrl();
        Assert.StartsWith("https://docs.microsoft.com/en-us/dotnet/api/", url);
        Assert.EndsWith("system.string", url);
    }

    [Fact]
    public void GetMSDocsUrl_throws_for_non_mscorlib_type()
    {
        Assert.Throws<InvalidOperationException>(() => typeof(MyClass).GetMSDocsUrl());
    }

    [Fact]
    public void GetInheritanceHierarchy_walks_base_types()
    {
        Type[] hierarchy = typeof(MyClass).GetInheritanceHierarchy().ToArray();
        Assert.Equal(typeof(MyClass), hierarchy[0]);
        Assert.Equal(typeof(object), hierarchy[^1]);
    }

    [Fact]
    public void GetSignature_readonly_struct_emits_readonly_keyword()
    {
        Assert.Contains("readonly struct MyReadonlyStruct", typeof(MyReadonlyStruct).GetSignature(full: true));
    }

    [Fact]
    public void GetSignature_ref_struct_emits_ref_keyword()
    {
        Assert.Contains("ref struct MyRefStruct", typeof(MyRefStruct).GetSignature(full: true));
    }

    [Fact]
    public void GetSignature_byte_enum_includes_underlying_type()
    {
        Assert.Equal("public enum MyByteEnum : byte", typeof(MyByteEnum).GetSignature(full: true));
    }

    [Fact]
    public void GetSignature_int_enum_omits_underlying_type()
    {
        Assert.DoesNotContain(":", typeof(MyEnum).GetSignature(full: true));
    }
}
