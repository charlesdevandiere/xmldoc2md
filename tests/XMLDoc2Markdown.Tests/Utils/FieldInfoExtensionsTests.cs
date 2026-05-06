using System.Reflection;
using MyClassLib;
using XMLDoc2Markdown.Utils;

namespace XMLDoc2Markdown.Tests.Utils;

[Collection(nameof(SampleAssemblyCollection))]
public class FieldInfoExtensionsTests
{
    [Fact]
    public void GetAccessibility_public_field()
    {
        FieldInfo f = typeof(MyClass).GetField(nameof(MyClass.myField))!;
        Assert.Equal(Accessibility.Public, f.GetAccessibility());
    }

    [Fact]
    public void GetAccessibility_private_field()
    {
        FieldInfo f = typeof(MyClass).GetField(
            "myPrivateField",
            BindingFlags.NonPublic | BindingFlags.Instance)!;
        Assert.Equal(Accessibility.Private, f.GetAccessibility());
    }

    [Fact]
    public void GetAccessibility_internal_field()
    {
        FieldInfo f = typeof(MyClass).GetField(
            "myInternalField",
            BindingFlags.NonPublic | BindingFlags.Instance)!;
        Assert.Equal(Accessibility.Internal, f.GetAccessibility());
    }

    [Fact]
    public void GetAccessibility_protected_field()
    {
        FieldInfo f = typeof(MyClass).GetField(
            "myProtectedField",
            BindingFlags.NonPublic | BindingFlags.Instance)!;
        Assert.Equal(Accessibility.Protected, f.GetAccessibility());
    }

    [Fact]
    public void GetSignature_short_form_is_name_only()
    {
        FieldInfo f = typeof(MyClass).GetField(nameof(MyClass.myField))!;
        Assert.Equal("myField", f.GetSignature());
    }

    [Fact]
    public void GetSignature_full_includes_accessibility_type_and_trailing_semicolon()
    {
        FieldInfo f = typeof(MyClass).GetField(nameof(MyClass.myField))!;
        Assert.Equal("public int myField;", f.GetSignature(full: true));
    }
}
