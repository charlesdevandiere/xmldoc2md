using System.Reflection;
using MyClassLib;
using MyClassLib.SubNamespace;
using XMLDoc2Markdown.Utils;

namespace XMLDoc2Markdown.Tests.Utils;

[Collection(nameof(SampleAssemblyCollection))]
public class MethodBaseExtensionsTests
{
    [Fact]
    public void GetAccessibility_public_method()
    {
        MethodInfo m = typeof(MyClass).GetMethod(nameof(MyClass.StaticMethod))!;
        Assert.Equal(Accessibility.Public, m.GetAccessibility());
    }

    [Fact]
    public void GetAccessibility_private_method()
    {
        MethodInfo m = typeof(MyClass).GetMethod(
            "PrivateStaticMethod",
            BindingFlags.NonPublic | BindingFlags.Static)!;
        Assert.Equal(Accessibility.Private, m.GetAccessibility());
    }

    [Fact]
    public void GetAccessibility_internal_method()
    {
        MethodInfo m = typeof(MyClass).GetMethod(
            "InternalStaticMethod",
            BindingFlags.NonPublic | BindingFlags.Static)!;
        Assert.Equal(Accessibility.Internal, m.GetAccessibility());
    }

    [Fact]
    public void GetAccessibility_protected_method()
    {
        MethodInfo m = typeof(MyClass).GetMethod(
            "ProtectedStaticMethod",
            BindingFlags.NonPublic | BindingFlags.Static)!;
        Assert.Equal(Accessibility.Protected, m.GetAccessibility());
    }

    [Fact]
    public void GetSignature_short_form_uses_method_name_and_param_types()
    {
        MethodInfo m = typeof(MyClass).GetMethod(nameof(MyClass.Do))!;
        Assert.Equal("Do(String, Int32)", m.GetSignature());
    }

    [Fact]
    public void GetSignature_short_form_no_params()
    {
        MethodInfo m = typeof(MyClass).GetMethod(nameof(MyClass.StaticMethod))!;
        Assert.Equal("StaticMethod()", m.GetSignature());
    }

    [Fact]
    public void GetSignature_full_includes_accessibility_return_type_and_param_names()
    {
        MethodInfo m = typeof(MyClass).GetMethod(nameof(MyClass.Do))!;
        Assert.Equal("public void Do(string firstParam, int secondParam)", m.GetSignature(full: true));
    }

    [Fact]
    public void GetSignature_full_static_method_includes_static()
    {
        MethodInfo m = typeof(MyClass).GetMethod(nameof(MyClass.StaticMethod))!;
        Assert.Equal("public static void StaticMethod()", m.GetSignature(full: true));
    }

    [Fact]
    public void GetSignature_generic_method_includes_type_parameter_names()
    {
        MethodInfo m = typeof(MyClass).GetMethod(nameof(MyClass.DoGeneric))!;
        Assert.Equal("public int DoGeneric<T>(T value)", m.GetSignature(full: true));
    }

    [Fact]
    public void GetSignature_constructor_uses_declaring_type_name()
    {
        ConstructorInfo ctor = typeof(MyClass).GetConstructor([typeof(string), typeof(int)])!;
        Assert.Equal("public MyClass(string firstParam, int secondParam)", ctor.GetSignature(full: true));
    }

    [Fact]
    public void GetSignature_abstract_method_includes_abstract_keyword()
    {
        MethodInfo m = typeof(MyAbstractClass).GetMethod(nameof(MyAbstractClass.Do))!;
        Assert.Contains("abstract", m.GetSignature(full: true));
    }

    [Fact]
    public void GetSignature_method_on_generic_type_uses_arg_names()
    {
        MethodInfo m = typeof(GenericClass<>).GetMethod("Map")!;
        string sig = m.GetSignature(full: true);
        Assert.Contains("Map<TSource, TTarget>", sig);
        Assert.Contains("TSource source", sig);
        Assert.Contains("TTarget target", sig);
    }

    [Fact]
    public void GetMSDocsUrl_throws_for_non_mscorlib_type()
    {
        MethodInfo m = typeof(MyClass).GetMethod(nameof(MyClass.StaticMethod))!;
        Assert.Throws<InvalidOperationException>(() => m.GetMSDocsUrl());
    }

    [Fact]
    public void GetInternalDocsUrl_appends_signature_anchor()
    {
        MethodInfo m = typeof(MyClass).GetMethod(nameof(MyClass.Do))!;
        string url = m.GetInternalDocsUrl(DocumentationStructure.Flat);
        // Punctuation is dropped in place (no replacement dash) — only spaces become '-'.
        Assert.Equal("./myclasslib.myclass.md#dostring-int32", url);
    }
}
