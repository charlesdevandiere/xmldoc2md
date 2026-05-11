using System.Reflection;
using MyClassLib;
using XMLDoc2Markdown.Signatures;

namespace XMLDoc2Markdown.Tests.Signatures.Modifiers;

[Collection(nameof(SampleAssemblyCollection))]
public class VirtualityModifierTests
{
    [Fact]
    public void Override_method_emits_override_keyword()
    {
        MethodInfo m = typeof(MyDerivedClass).GetMethod(nameof(MyDerivedClass.Do))!;
        string sig = m.GetSignature(full: true);
        Assert.Contains("override", sig);
        Assert.DoesNotContain("sealed", sig);
        Assert.DoesNotContain("virtual", sig);
    }

    [Fact]
    public void Virtual_method_emits_virtual_keyword()
    {
        MethodInfo m = typeof(MyDerivedClass).GetMethod(nameof(MyDerivedClass.Label))!;
        Assert.Contains("virtual", m.GetSignature(full: true));
    }

    [Fact]
    public void Sealed_override_emits_sealed_override()
    {
        MethodInfo m = typeof(MyGrandchildClass).GetMethod(nameof(MyGrandchildClass.Label))!;
        Assert.Contains("sealed override", m.GetSignature(full: true));
    }

    [Fact]
    public void New_hiding_method_emits_new_keyword()
    {
        MethodInfo m = typeof(MyGrandchildClass).GetMethod(nameof(MyGrandchildClass.Do))!;
        string sig = m.GetSignature(full: true);
        Assert.Contains("new", sig);
        Assert.DoesNotContain("override", sig);
    }

    [Fact]
    public void Override_property_emits_override_keyword()
    {
        PropertyInfo p = typeof(MyDerivedClass).GetProperty(nameof(MyDerivedClass.MyProperty))!;
        Assert.Contains("override", p.GetSignature(full: true));
    }

    [Fact]
    public void Plain_method_has_no_virtuality_keyword()
    {
        MethodInfo m = typeof(MyClass).GetMethod(nameof(MyClass.Do))!;
        string sig = m.GetSignature(full: true);
        Assert.DoesNotContain("virtual", sig);
        Assert.DoesNotContain("override", sig);
        Assert.DoesNotContain(" new ", sig);
    }
}
