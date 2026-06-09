using System.Reflection;
using MyClassLib;
using XMLDoc2Markdown.Signatures;

namespace XMLDoc2Markdown.Tests.Signatures;

[Collection(nameof(SampleAssemblyCollection))]
public class NullableReferenceTypeTests
{
    private static readonly NullabilityInfoContext NullCtx = new();

    [Fact]
    public void Nullable_reference_property_emits_question_mark()
    {
        PropertyInfo p = typeof(MyNullableClass).GetProperty(nameof(MyNullableClass.OptionalName))!;
        Assert.Contains("string?", p.GetSignature(NullCtx, full: true));
    }

    [Fact]
    public void Non_nullable_reference_property_has_no_question_mark()
    {
        PropertyInfo p = typeof(MyNullableClass).GetProperty(nameof(MyNullableClass.RequiredName))!;
        string sig = p.GetSignature(NullCtx, full: true);
        Assert.Contains("string RequiredName", sig);
        Assert.DoesNotContain("string?", sig);
    }

    [Fact]
    public void Value_type_Nullable_collapses_to_question_mark_syntax()
    {
        PropertyInfo p = typeof(MyNullableClass).GetProperty(nameof(MyNullableClass.Counter))!;
        string sig = p.GetSignature(NullCtx, full: true);
        Assert.Contains("int?", sig);
        Assert.DoesNotContain("Nullable<", sig);
    }

    [Fact]
    public void Nullable_generic_argument_is_propagated()
    {
        FieldInfo f = typeof(MyNullableClass).GetField(nameof(MyNullableClass.OptionalNames))!;
        Assert.Contains("IEnumerable<string?>?", f.GetSignature(NullCtx, full: true));
    }

    [Fact]
    public void Nullable_parameter_and_return_render_with_question_mark()
    {
        MethodInfo m = typeof(MyNullableClass).GetMethod(nameof(MyNullableClass.Echo))!;
        string sig = m.GetSignature(NullCtx, full: true);
        Assert.Contains("string? Echo", sig);
        Assert.Contains("string? input", sig);
    }

    [Fact]
    public void Unconstrained_generic_parameter_does_not_gain_question_mark()
    {
        // `T value` in MyClass.DoGeneric<T>(T value) — unconstrained, source has no `?`,
        // and the metadata's "Nullable" reading must not bleed into the rendered name.
        MethodInfo m = typeof(MyClass).GetMethod(nameof(MyClass.DoGeneric))!;
        string sig = m.GetSignature(NullCtx, full: true);
        Assert.Contains("T value", sig);
        Assert.DoesNotContain("T? value", sig);
    }
}
