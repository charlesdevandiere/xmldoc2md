using MyClassLib;
using MyClassLib.SubNamespace;
using XMLDoc2Markdown.Members;

namespace XMLDoc2Markdown.Tests;

[Collection(nameof(SampleAssemblyCollection))]
public class TypeDocumentationTests
{
    private readonly SampleAssemblyFixture fixture;

    public TypeDocumentationTests(SampleAssemblyFixture fixture)
    {
        this.fixture = fixture;
    }

    private string Render(Type type, TypeDocumentationOptions? options = null)
    {
        return new TypeDocumentation(this.fixture.Assembly, type, this.fixture.Documentation, options).ToString();
    }

    [Fact]
    public void Renders_header_with_display_name_and_namespace()
    {
        string md = this.Render(typeof(MyClass));

        Assert.Contains("# MyClass", md);
        Assert.Contains("Namespace: MyClassLib", md);
    }

    [Fact]
    public void Renders_summary_and_remarks_from_xml()
    {
        string md = this.Render(typeof(MyClass));

        Assert.Contains("My class.", md);
        Assert.Contains("**Remarks:**", md);
        Assert.Contains("A remark.", md);
    }

    [Fact]
    public void Renders_full_signature_in_csharp_code_block()
    {
        string md = this.Render(typeof(MyClass));

        Assert.Contains("```csharp", md);
        Assert.Contains("public class MyClass", md);
    }

    [Fact]
    public void Renders_inheritance_and_implements()
    {
        string md = this.Render(typeof(MyClass));

        Assert.Contains("Inheritance", md);
        Assert.Contains("Implements", md);
        Assert.Contains("IMyInterface", md);
    }

    [Fact]
    public void Renders_section_headers_for_each_member_kind()
    {
        string md = this.Render(typeof(MyClass));

        Assert.Contains("## Properties", md);
        Assert.Contains("## Constructors", md);
        Assert.Contains("## Methods", md);
        Assert.Contains("## Events", md);
        Assert.Contains("## Fields", md);
    }

    [Fact]
    public void Renders_method_parameters_returns_and_exceptions()
    {
        string md = this.Render(typeof(MyClass));

        Assert.Contains("#### Parameters", md);
        Assert.Contains("`firstParam`", md);
        Assert.Contains("#### Returns", md);
        Assert.Contains("#### Exceptions", md);
    }

    [Fact]
    public void Renders_property_value_section()
    {
        string md = this.Render(typeof(MyClass));
        Assert.Contains("#### Property Value", md);
    }

    [Fact]
    public void Renders_type_parameters_for_generic_method()
    {
        string md = this.Render(typeof(MyClass));
        Assert.Contains("#### Type Parameters", md);
    }

    [Fact]
    public void Renders_type_parameters_for_generic_class()
    {
        string md = this.Render(typeof(GenericClass<>));
        Assert.Contains("#### Type Parameters", md);
        Assert.Contains("`T`", md);
    }

    [Fact]
    public void Renders_obsolete_caution_with_message()
    {
#pragma warning disable CS0618
        string md = this.Render(typeof(MyObsoleteClass));
#pragma warning restore CS0618

        Assert.Contains("#### Caution", md);
        Assert.Contains("Deprecated, use MyClass instead.", md);
    }

    [Fact]
    public void Renders_obsolete_member_default_message_when_no_attribute_message()
    {
#pragma warning disable CS0618
        string md = this.Render(typeof(MyObsoleteClass));
#pragma warning restore CS0618
        // Members on MyObsoleteClass have [Obsolete] without message — should fall back to default.
        Assert.Contains("This member is obsolete.", md);
    }

    [Fact]
    public void Renders_enum_fields_as_table()
    {
        string md = this.Render(typeof(MyEnum));

        Assert.Contains("## Fields", md);
        Assert.Contains("| Name", md);
        Assert.Contains("Default", md);
        Assert.Contains("First", md);
        Assert.Contains("Second", md);
        // Values
        Assert.Contains("| 0 ", md);
        Assert.Contains("| 1 ", md);
        Assert.Contains("| 2 ", md);
    }

    [Fact]
    public void Renders_flags_enum()
    {
        string md = this.Render(typeof(MyFlag));

        Assert.Contains("## Fields", md);
        Assert.Contains("Third", md);
    }

    [Fact]
    public void Renders_seealso_xml_doc_as_link()
    {
        // MyClass remarks include `<see cref="MyClassLib.MyClass.MyClass(string, int)" />`
        string md = this.Render(typeof(MyClass));
        Assert.Contains("See also", md);
        Assert.Contains("[MyClass.MyClass(String, Int32)]", md);
        Assert.Contains("./myclasslib.myclass.md#myclassstring-int32", md);
    }

    [Fact]
    public void Renders_code_block_from_xml_code_element()
    {
        string md = this.Render(typeof(MyClass));
        // The remark contains a <code> block with `var foo = new MyClass("foo", 1);`
        Assert.Contains("var foo = new MyClass(\"foo\", 1);", md);
    }

    [Fact]
    public void Renders_list_from_xml_list_element()
    {
        string md = this.Render(typeof(MyClass));
        // <list type="bullet"> with items "item 1" and "item 2"
        Assert.Contains("item 1", md);
        Assert.Contains("item 2", md);
    }

    [Fact]
    public void Member_accessibility_filter_excludes_private_members_by_default()
    {
        TypeDocumentationOptions options = new() { MemberAccessibilityLevel = Accessibility.Public };
        string md = this.Render(typeof(MyClass), options);

        Assert.DoesNotContain("PrivateDoGeneric", md);
        Assert.DoesNotContain("myPrivateField", md);
    }

    [Fact]
    public void Member_accessibility_filter_includes_private_when_lowered()
    {
        TypeDocumentationOptions options = new() { MemberAccessibilityLevel = Accessibility.Private };
        string md = this.Render(typeof(MyClass), options);

        Assert.Contains("PrivateDoGeneric", md);
        Assert.Contains("myPrivateField", md);
    }

    [Fact]
    public void Protected_filter_includes_protected_excludes_private_internal()
    {
        TypeDocumentationOptions options = new() { MemberAccessibilityLevel = Accessibility.Protected };
        string md = this.Render(typeof(MyClass), options);

        Assert.Contains("ProtectedDo", md);
        Assert.DoesNotContain("PrivateDo(", md);
        Assert.DoesNotContain("InternalDo(", md);
    }

    [Fact]
    public void NoExtension_strips_md_extension_from_internal_links()
    {
        TypeDocumentationOptions options = new() { NoExtension = true };
        string md = this.Render(typeof(MyClass), options);

        // Inheritance link target should not end in .md
        Assert.DoesNotContain("./myclasslib.myclass.md", md);
    }

    [Fact]
    public void NoExtension_with_NoPrefix_strips_md_extension_and_dot_slash_prefix()
    {
        TypeDocumentationOptions options = new() { NoExtension = true, NoPrefix = true };
        string md = this.Render(typeof(MyClass), options);

        Assert.DoesNotContain("./myclasslib.myclass", md);
        Assert.DoesNotContain(".md)", md);
    }

    [Fact]
    public void Tree_structure_uses_slash_separated_links()
    {
        TypeDocumentationOptions options = new() { Structure = DocumentationStructure.Tree };
        string md = this.Render(typeof(GenericClass<>), options);

        // Link to the type itself (e.g. inheritance) should use "/" instead of "."
        Assert.Contains("myclasslib/subnamespace/genericclass", md);
    }

    [Fact]
    public void BackButton_top_appends_top_link()
    {
        TypeDocumentationOptions options = new() { BackButton = true };
        string md = this.Render(typeof(MyClass), options);

        Assert.Contains("[`< Back`]", md);
    }

    [Fact]
    public void Examples_directory_inlines_member_specific_markdown()
    {
        string examplesDir = Path.Combine(
            Path.GetDirectoryName(this.fixture.DllPath)!,
            "test-examples");
        Directory.CreateDirectory(examplesDir);
        try
        {
            File.WriteAllText(
                Path.Combine(examplesDir, "MyClassLib.MyClass.md"),
                "## Custom Example\n\nA snippet from disk.");

            TypeDocumentationOptions options = new() { ExamplesDirectory = examplesDir };
            string md = this.Render(typeof(MyClass), options);

            Assert.Contains("Custom Example", md);
            Assert.Contains("A snippet from disk.", md);
        }
        finally
        {
            Directory.Delete(examplesDir, recursive: true);
        }
    }

    [Fact]
    public void Static_class_signature_includes_static_keyword()
    {
        string md = this.Render(typeof(MyStaticClass));
        Assert.Contains("public static class MyStaticClass", md);
    }

    [Fact]
    public void Abstract_class_signature_includes_abstract_keyword()
    {
        string md = this.Render(typeof(MyAbstractClass));
        Assert.Contains("public abstract class MyAbstractClass", md);
    }

    [Fact]
    public void Interface_renders_methods_section()
    {
        string md = this.Render(typeof(IMyInterface));
        Assert.Contains("public interface IMyInterface", md);
        Assert.Contains("## Methods", md);
        Assert.Contains("Do(", md);
    }

    [Fact]
    public void Constructor_throws_on_null_args()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new TypeDocumentation(null!, typeof(MyClass), this.fixture.Documentation));
        Assert.Throws<ArgumentNullException>(() =>
            new TypeDocumentation(this.fixture.Assembly, null!, this.fixture.Documentation));
        Assert.Throws<ArgumentNullException>(() =>
            new TypeDocumentation(this.fixture.Assembly, typeof(MyClass), null!));
    }
}
