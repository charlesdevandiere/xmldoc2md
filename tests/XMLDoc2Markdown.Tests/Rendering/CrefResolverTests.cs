using System.Reflection;
using MyClassLib;
using XMLDoc2Markdown.Rendering;

namespace XMLDoc2Markdown.Tests.Rendering;

[Collection(nameof(SampleAssemblyCollection))]
public class CrefResolverTests
{
    private readonly CrefResolver resolver;

    public CrefResolverTests(SampleAssemblyFixture fixture)
    {
        this.resolver = new CrefResolver(fixture.Assembly);
    }

    [Fact]
    public void TryResolve_top_level_type()
    {
        Assert.True(this.resolver.TryResolve("T:MyClassLib.MyClass", out MemberInfo? m));
        Assert.Equal(typeof(MyClass), m);
    }

    [Fact]
    public void TryResolve_nested_type_via_dot_notation()
    {
        Assert.True(this.resolver.TryResolve("T:MyClassLib.MyClass.Nested", out MemberInfo? m));
        Assert.Equal(typeof(MyClass.Nested), m);
    }

    [Fact]
    public void TryResolve_property_on_nested_type_cascades()
    {
        Assert.True(this.resolver.TryResolve("P:MyClassLib.MyClass.Nested.Value", out MemberInfo? m));
        Assert.NotNull(m);
        Assert.Equal("Value", m!.Name);
        Assert.Equal(typeof(MyClass.Nested), m.DeclaringType);
    }

    [Fact]
    public void TryResolve_unknown_type_returns_false()
    {
        Assert.False(this.resolver.TryResolve("T:MyClassLib.DoesNotExist", out MemberInfo? m));
        Assert.Null(m);
    }

    [Fact]
    public void TryResolve_unknown_nested_type_returns_false()
    {
        Assert.False(this.resolver.TryResolve("T:MyClassLib.MyClass.NoSuchNested", out MemberInfo? m));
        Assert.Null(m);
    }

    [Fact]
    public void TryResolve_bcl_nested_type_resolves()
    {
        // Dictionary<,>.Enumerator — exercises the dot-walk fallback against Type.GetType
        // (mscorlib types live outside the sample assembly).
        Assert.True(this.resolver.TryResolve("T:System.Collections.Generic.Dictionary`2.Enumerator", out MemberInfo? m));
        Assert.NotNull(m);
        Assert.Equal("Enumerator", m!.Name);
    }
}
