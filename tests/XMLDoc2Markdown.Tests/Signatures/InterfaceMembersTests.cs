using System.Reflection;
using MyClassLib;
using XMLDoc2Markdown.Signatures;

namespace XMLDoc2Markdown.Tests.Signatures;

[Collection(nameof(SampleAssemblyCollection))]
public class InterfaceMembersTests
{
    [Fact]
    public void Plain_interface_method_omits_public_and_abstract()
    {
        MethodInfo m = typeof(IMyInterface).GetMethod(nameof(IMyInterface.Do))!;
        string sig = m.GetSignature(full: true);
        Assert.Equal("void Do(string firstParam, int secondParam)", sig);
    }

    [Fact]
    public void Default_interface_method_emits_virtual_keyword()
    {
        MethodInfo m = typeof(IMyInterface).GetMethod(nameof(IMyInterface.Greet))!;
        string sig = m.GetSignature(full: true);
        Assert.Contains("virtual", sig);
        Assert.DoesNotContain("abstract", sig);
        Assert.DoesNotContain("public", sig);
    }

    [Fact]
    public void Static_abstract_interface_method_keeps_static_abstract()
    {
        MethodInfo m = typeof(INumeric<>).GetMethod("Zero")!;
        string sig = m.GetSignature(full: true);
        Assert.Contains("static", sig);
        Assert.Contains("abstract", sig);
    }

    [Fact]
    public void Static_virtual_interface_method_keeps_static_virtual()
    {
        MethodInfo m = typeof(INumeric<>).GetMethod("Square")!;
        string sig = m.GetSignature(full: true);
        Assert.Contains("static", sig);
        Assert.Contains("virtual", sig);
        Assert.DoesNotContain("abstract", sig);
    }

    [Fact]
    public void Static_abstract_operator_emits_keywords_in_canonical_order()
    {
        MethodInfo m = typeof(INumeric<>).GetMethod("op_Addition")!;
        string sig = m.GetSignature(full: true);
        Assert.Contains("static abstract", sig);
        Assert.Contains("operator +", sig);
    }
}
