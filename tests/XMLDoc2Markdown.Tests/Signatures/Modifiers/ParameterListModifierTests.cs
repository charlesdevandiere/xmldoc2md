using System.Reflection;
using MyClassLib;
using XMLDoc2Markdown.Signatures;
using XMLDoc2Markdown.Signatures.Modifiers;

namespace XMLDoc2Markdown.Tests.Signatures.Modifiers;

[Collection(nameof(SampleAssemblyCollection))]
public class ParameterListModifierTests
{
    [Fact]
    public void Ref_parameter_renders_with_ref_keyword()
    {
        MethodInfo m = typeof(MyParameterModifiers).GetMethod(nameof(MyParameterModifiers.RefIncrement))!;
        Assert.Contains("ref int value", m.GetSignature(full: true));
    }

    [Fact]
    public void In_parameter_renders_with_in_keyword()
    {
        MethodInfo m = typeof(MyParameterModifiers).GetMethod(nameof(MyParameterModifiers.Read))!;
        Assert.Contains("in int value", m.GetSignature(full: true));
    }

    [Fact]
    public void Out_parameter_renders_with_out_keyword()
    {
        MethodInfo m = typeof(MyParameterModifiers).GetMethod(nameof(MyParameterModifiers.TryParse))!;
        Assert.Contains("out int value", m.GetSignature(full: true));
    }

    [Fact]
    public void Params_parameter_renders_with_params_keyword()
    {
        MethodInfo m = typeof(MyParameterModifiers).GetMethod(nameof(MyParameterModifiers.Join))!;
        Assert.Contains("params String[] items", m.GetSignature(full: true));
    }

    [Fact]
    public void Default_string_value_renders_quoted()
    {
        MethodInfo m = typeof(MyParameterModifiers).GetMethod(nameof(MyParameterModifiers.Defaults))!;
        Assert.Contains("string name = \"anakin\"", m.GetSignature(full: true));
    }

    [Fact]
    public void Default_int_value_renders_unquoted()
    {
        MethodInfo m = typeof(MyParameterModifiers).GetMethod(nameof(MyParameterModifiers.Defaults))!;
        Assert.Contains("int count = 42", m.GetSignature(full: true));
    }

    [Fact]
    public void Default_double_value_renders_with_d_suffix()
    {
        MethodInfo m = typeof(MyParameterModifiers).GetMethod(nameof(MyParameterModifiers.Defaults))!;
        Assert.Contains("double ratio = 1.5d", m.GetSignature(full: true));
    }

    [Fact]
    public void Default_bool_value_renders_as_csharp_literal()
    {
        MethodInfo m = typeof(MyParameterModifiers).GetMethod(nameof(MyParameterModifiers.Defaults))!;
        Assert.Contains("bool flag = true", m.GetSignature(full: true));
    }

    [Fact]
    public void Default_enum_value_renders_as_typed_member()
    {
        MethodInfo m = typeof(MyParameterModifiers).GetMethod(nameof(MyParameterModifiers.Defaults))!;
        Assert.Contains("MyEnum kind = MyEnum.First", m.GetSignature(full: true));
    }

    [Fact]
    public void Default_null_value_renders_as_null()
    {
        MethodInfo m = typeof(MyParameterModifiers).GetMethod(nameof(MyParameterModifiers.Defaults))!;
        Assert.Contains("note = null", m.GetSignature(full: true));
    }

    [Theory]
    [InlineData(nameof(MyParameterModifiers.RefIncrement), 0, "ref")]
    [InlineData(nameof(MyParameterModifiers.Read), 0, "in")]
    [InlineData(nameof(MyParameterModifiers.TryParse), 1, "out")]
    [InlineData(nameof(MyParameterModifiers.Join), 0, "params")]
    public void GetPassingModifier_returns_expected_keyword(string methodName, int paramIndex, string expected)
    {
        ParameterInfo p = typeof(MyParameterModifiers).GetMethod(methodName)!.GetParameters()[paramIndex];
        Assert.Equal(expected, ParameterListModifier.GetPassingModifier(p));
    }

    [Fact]
    public void GetPassingModifier_returns_null_for_plain_value_parameter()
    {
        ParameterInfo p = typeof(MyParameterModifiers).GetMethod(nameof(MyParameterModifiers.TryParse))!.GetParameters()[0];
        Assert.Null(ParameterListModifier.GetPassingModifier(p));
    }
}
