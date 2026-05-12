using System.Reflection;
using System.Xml.Linq;
using MyClassLib;
using XMLDoc2Markdown.Rendering;

namespace XMLDoc2Markdown.Tests.Rendering;

[Collection(nameof(SampleAssemblyCollection))]
public class InheritDocResolverTests
{
    private readonly SampleAssemblyFixture fixture;
    private readonly InheritDocResolver resolver;

    public InheritDocResolverTests(SampleAssemblyFixture fixture)
    {
        this.fixture = fixture;
        CrefResolver cref = new(fixture.Assembly);
        this.resolver = new InheritDocResolver(fixture.Documentation, cref);
    }

    private XElement Resolve(MemberInfo member)
    {
        XElement raw = this.fixture.Documentation.GetMember(member)
            ?? throw new InvalidOperationException($"No XML doc for {member.Name}");
        return this.resolver.Resolve(member, raw);
    }

    [Fact]
    public void Resolve_returns_original_when_no_inheritdoc()
    {
        MethodInfo method = typeof(MyClass).GetMethod(nameof(MyClass.Do), [typeof(string), typeof(int)])!;
        XElement raw = this.fixture.Documentation.GetMember(method)!;
        XElement resolved = this.resolver.Resolve(method, raw);
        Assert.Same(raw, resolved);
    }

    [Fact]
    public void Resolve_overridden_method_inherits_from_base()
    {
        MethodInfo method = typeof(InheritDocDerived).GetMethod(nameof(InheritDocDerived.Process))!;
        XElement resolved = this.Resolve(method);

        Assert.Empty(resolved.Elements("inheritdoc"));
        Assert.Equal("Processes an input value.", resolved.Element("summary")!.Value.Trim());
        Assert.Equal("value", resolved.Element("param")!.Attribute("name")!.Value);
        Assert.NotNull(resolved.Element("returns"));
        Assert.NotNull(resolved.Element("exception"));
    }

    [Fact]
    public void Resolve_overridden_property_inherits_summary_and_value()
    {
        PropertyInfo prop = typeof(InheritDocDerived).GetProperty(nameof(InheritDocDerived.Name))!;
        XElement resolved = this.Resolve(prop);

        Assert.Equal("The configured name.", resolved.Element("summary")!.Value.Trim());
        Assert.Equal("The display name.", resolved.Element("value")!.Value.Trim());
    }

    [Fact]
    public void Resolve_interface_implementation_inherits_from_interface()
    {
        MethodInfo method = typeof(InheritDocImpl).GetMethod(nameof(InheritDocImpl.Lookup))!;
        XElement resolved = this.Resolve(method);

        Assert.Equal("Looks up an item by key.", resolved.Element("summary")!.Value.Trim());
        Assert.Equal("key", resolved.Element("param")!.Attribute("name")!.Value);
        Assert.NotNull(resolved.Element("returns"));
    }

    [Fact]
    public void Resolve_explicit_cref_inherits_from_target()
    {
        MethodInfo method = typeof(InheritDocImpl).GetMethod(nameof(InheritDocImpl.ReuseFromCref))!;
        XElement resolved = this.Resolve(method);

        Assert.Equal("Processes an input value.", resolved.Element("summary")!.Value.Trim());
        Assert.NotNull(resolved.Element("param"));
        Assert.NotNull(resolved.Element("returns"));
    }

    [Fact]
    public void Resolve_local_summary_wins_over_inherited()
    {
        MethodInfo method = typeof(InheritDocDerived).GetMethod(nameof(InheritDocDerived.ProcessTwice))!;
        XElement resolved = this.Resolve(method);

        XElement summary = resolved.Element("summary")!;
        Assert.Contains("Keeps its own summary", summary.Value);
        Assert.DoesNotContain("Processes an input value.", summary.Value);

        Assert.Single(resolved.Elements("summary"));
        Assert.NotNull(resolved.Element("param"));
        Assert.NotNull(resolved.Element("returns"));
    }

    [Fact]
    public void Resolve_existing_param_wins_over_inherited_param()
    {
        // Synthesize: target has its own <param name="value"> plus <inheritdoc/>.
        MethodInfo method = typeof(InheritDocDerived).GetMethod(nameof(InheritDocDerived.Process))!;
        XElement local = new("member",
            new XElement("param", new XAttribute("name", "value"), "Local override of the param doc."),
            new XElement("inheritdoc"));

        XElement resolved = this.resolver.Resolve(method, local);

        XElement[] @params = resolved.Elements("param").ToArray();
        Assert.Single(@params);
        Assert.Equal("Local override of the param doc.", @params[0].Value);
    }

    [Fact]
    public void Resolve_returns_no_inheritdoc_for_non_override_method()
    {
        // Label() on MyDerivedClass is a virtual hook with no base.
        MethodInfo method = typeof(MyDerivedClass).GetMethod(nameof(MyDerivedClass.Label))!;
        XElement raw = this.fixture.Documentation.GetMember(method)!;
        // It doesn't have <inheritdoc/> in source — resolve should be a no-op.
        Assert.Same(raw, this.resolver.Resolve(method, raw));
    }

    [Fact]
    public void Resolve_drops_unresolved_inheritdoc()
    {
        // A virtual member with <inheritdoc/> but no inheritance source.
        MethodInfo method = typeof(MyDerivedClass).GetMethod(nameof(MyDerivedClass.Label))!;
        XElement local = new("member", new XElement("inheritdoc"));
        XElement resolved = this.resolver.Resolve(method, local);
        Assert.Empty(resolved.Elements("inheritdoc"));
        Assert.Empty(resolved.Elements());
    }

    [Fact]
    public void Resolve_cycle_does_not_loop()
    {
        // Even if a chain of inheritdocs pointed back at the target, resolver must terminate.
        // We exercise the visited-set guard by re-resolving the same target through its own cref.
        MethodInfo method = typeof(InheritDocImpl).GetMethod(nameof(InheritDocImpl.ReuseFromCref))!;
        XElement local = new("member",
            new XElement("inheritdoc",
                new XAttribute("cref", $"M:{method.DeclaringType!.FullName}.{method.Name}(System.Int32)")));
        XElement resolved = this.resolver.Resolve(method, local);
        // Self-cref: visited set blocks the recursion; result has no inheritdoc and no inherited content.
        Assert.Empty(resolved.Elements("inheritdoc"));
    }
}
