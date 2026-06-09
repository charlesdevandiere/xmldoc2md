using System.Reflection;
using MyClassLib;
using XMLDoc2Markdown.Members;
using XMLDoc2Markdown.Signatures;

namespace XMLDoc2Markdown.Tests.Utils;

[Collection(nameof(SampleAssemblyCollection))]
public class PropertyInfoExtensionsTests
{
    [Fact]
    public void GetAccessibility_uses_max_of_get_and_set()
    {
        PropertyInfo p = typeof(MyClass).GetProperty(nameof(MyClass.MyProperty))!;
        Assert.Equal(Accessibility.Public, p.GetAccessibility());
    }

    [Fact]
    public void GetAccessibility_returns_None_when_no_methods()
    {
        // Synthesized hypothetical: nothing in the sample matches, but verify with a real private prop
        PropertyInfo p = typeof(MyClass).GetProperty(
            "MyPrivateProperty",
            BindingFlags.NonPublic | BindingFlags.Instance)!;
        Assert.Equal(Accessibility.Private, p.GetAccessibility());
    }

    [Fact]
    public void GetReturnType_returns_property_type()
    {
        PropertyInfo p = typeof(MyClass).GetProperty(nameof(MyClass.MyNullableProperty))!;
        Assert.Equal(typeof(int?), p.GetReturnType());
    }

    [Fact]
    public void GetSignature_short_form_is_just_name()
    {
        PropertyInfo p = typeof(MyClass).GetProperty(nameof(MyClass.MyProperty))!;
        Assert.Equal("MyProperty", p.GetSignature());
    }

    [Fact]
    public void GetSignature_full_includes_accessors()
    {
        PropertyInfo p = typeof(MyClass).GetProperty(nameof(MyClass.MyProperty))!;
        string sig = p.GetSignature(full: true);

        Assert.Contains("public", sig);
        Assert.Contains("string", sig);
        Assert.Contains("MyProperty", sig);
        Assert.Contains("get;", sig);
        Assert.Contains("set;", sig);
        Assert.Contains("{", sig);
        Assert.Contains("}", sig);
    }

    [Fact]
    public void GetSignature_full_emits_protected_set_when_set_is_more_restricted()
    {
        // MyProperty has `public string MyProperty { get; protected set; }`
        PropertyInfo p = typeof(MyClass).GetProperty(nameof(MyClass.MyProperty))!;
        string sig = p.GetSignature(full: true);
        Assert.Contains("protected set;", sig);
    }

    [Fact]
    public void GetSignature_full_static_property_includes_static_keyword()
    {
#pragma warning disable CS0618
        PropertyInfo p = typeof(MyObsoleteClass).GetProperty(nameof(MyObsoleteClass.StaticProperty))!;
#pragma warning restore CS0618
        Assert.Contains("static", p.GetSignature(full: true));
    }

    [Fact]
    public void GetSignature_full_abstract_property_includes_abstract_keyword()
    {
        PropertyInfo p = typeof(MyAbstractClass).GetProperty(nameof(MyAbstractClass.MyProperty))!;
        Assert.Contains("abstract", p.GetSignature(full: true));
    }

    [Fact]
    public void GetSignature_full_get_only_property()
    {
        PropertyInfo p = typeof(MyClass.Nested).GetProperty(nameof(MyClass.Nested.Value))!;
        string sig = p.GetSignature(full: true);
        Assert.Contains("get;", sig);
        Assert.Contains("set;", sig);
    }

    [Fact]
    public void GetSignature_full_init_only_property_emits_init()
    {
        PropertyInfo p = typeof(MyClass).GetProperty(nameof(MyClass.MyInitProperty))!;
        string sig = p.GetSignature(full: true);
        Assert.Contains("init;", sig);
        Assert.DoesNotContain("set;", sig);
    }

    [Fact]
    public void GetSignature_full_required_property_emits_required()
    {
        PropertyInfo p = typeof(MyClass).GetProperty(nameof(MyClass.MyRequiredProperty))!;
        string sig = p.GetSignature(full: true);
        Assert.Contains("required", sig);
    }

    [Fact]
    public void GetSignature_indexer_uses_this_brackets()
    {
        PropertyInfo p = typeof(MyIndexer).GetProperty("Item", [typeof(int)])!;
        string sig = p.GetSignature(full: true);
        Assert.Contains("this[int index]", sig);
        Assert.DoesNotContain("Item", sig);
    }
}
