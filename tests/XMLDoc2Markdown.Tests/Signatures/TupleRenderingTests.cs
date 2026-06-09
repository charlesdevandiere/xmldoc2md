using System.Reflection;
using MyClassLib;
using XMLDoc2Markdown.Signatures;

namespace XMLDoc2Markdown.Tests.Signatures;

[Collection(nameof(SampleAssemblyCollection))]
public class TupleRenderingTests
{
    private static readonly NullabilityInfoContext NullCtx = new();

    [Fact]
    public void Named_tuple_return_type_renders_with_element_names()
    {
        MethodInfo m = typeof(MyNullableClass).GetMethod(nameof(MyNullableClass.Origin))!;
        Assert.Contains("(int X, int Y) Origin()", m.GetSignature(NullCtx, full: true));
    }

    [Fact]
    public void Named_tuple_parameter_renders_with_element_names()
    {
        MethodInfo m = typeof(MyNullableClass).GetMethod(nameof(MyNullableClass.Describe))!;
        Assert.Contains("(string Name, int Age) point", m.GetSignature(NullCtx, full: true));
    }

    [Fact]
    public void Positional_tuple_parameter_renders_without_names()
    {
        MethodInfo m = typeof(MyNullableClass).GetMethod(nameof(MyNullableClass.Describe))!;
        string sig = m.GetSignature(NullCtx, full: true);
        Assert.Contains("(int, int) pair", sig);
    }

    [Theory]
    [InlineData(typeof((int, int)), true)]
    [InlineData(typeof(ValueTuple<int, string, double>), true)]
    [InlineData(typeof(Tuple<int, int>), false)] // System.Tuple, not ValueTuple
    [InlineData(typeof(int), false)]
    [InlineData(typeof(MyClass), false)]
    public void IsValueTuple_recognises_value_tuple_types(Type type, bool expected)
    {
        Assert.Equal(expected, type.IsValueTuple());
    }
}
