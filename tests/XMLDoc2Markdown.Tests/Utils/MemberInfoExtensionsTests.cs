using System.Reflection;
using MyClassLib;
using MyClassLib.SubNamespace;
using XMLDoc2Markdown.Utils;

namespace XMLDoc2Markdown.Tests.Utils;

[Collection(nameof(SampleAssemblyCollection))]
public class MemberInfoExtensionsTests
{
    [Fact]
    public void GetIdentifier_for_type_uses_full_name_with_dot_for_nested()
    {
        Assert.Equal("MyClassLib.MyClass", typeof(MyClass).GetIdentifier());
        Assert.Equal("MyClassLib.MyClass.Nested", typeof(MyClass.Nested).GetIdentifier());
    }

    [Fact]
    public void GetIdentifier_for_open_generic_type_keeps_arity()
    {
        Assert.Equal("MyClassLib.SubNamespace.GenericClass`1", typeof(GenericClass<>).GetIdentifier());
    }

    [Fact]
    public void GetIdentifier_for_property_includes_declaring_type()
    {
        PropertyInfo prop = typeof(MyClass).GetProperty(nameof(MyClass.MyProperty))!;
        Assert.Equal("MyClassLib.MyClass.MyProperty", prop.GetIdentifier());
    }

    [Fact]
    public void GetIdentifier_for_field_includes_declaring_type()
    {
        FieldInfo field = typeof(MyClass).GetField(nameof(MyClass.myField))!;
        Assert.Equal("MyClassLib.MyClass.myField", field.GetIdentifier());
    }

    [Fact]
    public void GetIdentifier_for_event_includes_declaring_type()
    {
        EventInfo evt = typeof(MyClass).GetEvent(nameof(MyClass.MyEvent))!;
        Assert.Equal("MyClassLib.MyClass.MyEvent", evt.GetIdentifier());
    }

    [Fact]
    public void GetIdentifier_for_constructor_uses_hash_ctor()
    {
        ConstructorInfo ctor = typeof(MyClass).GetConstructor(Type.EmptyTypes)!;
        Assert.Equal("MyClassLib.MyClass.#ctor", ctor.GetIdentifier());
    }

    [Fact]
    public void GetIdentifier_for_constructor_with_params_includes_param_signature()
    {
        ConstructorInfo ctor = typeof(MyClass).GetConstructor([typeof(string), typeof(int)])!;
        Assert.Equal("MyClassLib.MyClass.#ctor(System.String,System.Int32)", ctor.GetIdentifier());
    }

    [Fact]
    public void GetIdentifier_for_method_with_no_params()
    {
        MethodInfo method = typeof(MyClass).GetMethod(nameof(MyClass.StaticMethod))!;
        Assert.Equal("MyClassLib.MyClass.StaticMethod", method.GetIdentifier());
    }

    [Fact]
    public void GetIdentifier_for_method_uses_double_backtick_for_method_generics()
    {
        MethodInfo method = typeof(MyClass).GetMethod(nameof(MyClass.DoGeneric))!;
        Assert.Equal("MyClassLib.MyClass.DoGeneric``1(``0)", method.GetIdentifier());
    }

    [Fact]
    public void GetIdentifier_for_method_with_generic_param_type_uses_braces()
    {
        MethodInfo method = typeof(MyClass).GetMethod(nameof(MyClass.Get))!;
        Assert.Equal(
            "MyClassLib.MyClass.Get(System.Collections.Generic.List{System.String})",
            method.GetIdentifier());
    }

    [Fact]
    public void GetIdentifier_for_method_with_dictionary_param()
    {
        MethodInfo method = typeof(MyClass).GetMethod(nameof(MyClass.Counts))!;
        Assert.Equal(
            "MyClassLib.MyClass.Counts(System.Collections.Generic.IDictionary{System.String,System.Int32})",
            method.GetIdentifier());
    }

    [Fact]
    public void GetIdentifier_for_method_on_generic_type_uses_single_backtick_for_type_generics()
    {
        MethodInfo method = typeof(GenericClass<>).GetMethod(
            "GetGenericInstance",
            BindingFlags.Public | BindingFlags.Instance,
            [])!;
        Assert.Equal(
            "MyClassLib.SubNamespace.GenericClass`1.GetGenericInstance``1",
            method.GetIdentifier());
    }

    [Fact]
    public void GetSignature_dispatches_by_member_type()
    {
        Type type = typeof(MyClass);
        MethodInfo method = typeof(MyClass).GetMethod(nameof(MyClass.StaticMethod))!;
        PropertyInfo prop = typeof(MyClass).GetProperty(nameof(MyClass.MyProperty))!;
        FieldInfo field = typeof(MyClass).GetField(nameof(MyClass.myField))!;
        EventInfo evt = typeof(MyClass).GetEvent(nameof(MyClass.MyEvent))!;

        Assert.Equal(type.GetSignature(full: true), ((MemberInfo)type).GetSignature(full: true));
        Assert.Equal(method.GetSignature(full: true), ((MemberInfo)method).GetSignature(full: true));
        Assert.Equal(prop.GetSignature(full: true), ((MemberInfo)prop).GetSignature(full: true));
        Assert.Equal(field.GetSignature(full: true), ((MemberInfo)field).GetSignature(full: true));
        Assert.Equal(evt.GetSignature(full: true), ((MemberInfo)evt).GetSignature(full: true));
    }
}
