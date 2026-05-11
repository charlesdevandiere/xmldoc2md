using MyClassLib;
using XMLDoc2Markdown.Members;
using XMLDoc2Markdown.Rendering;

namespace XMLDoc2Markdown.Tests.Rendering;

[Collection(nameof(SampleAssemblyCollection))]
public class MemberDiscoveryTests
{
    [Fact]
    public void GetProperties_excludes_indexers()
    {
        MemberDiscovery discovery = new(typeof(MyIndexer), Accessibility.Public);
        Assert.Empty(discovery.GetProperties());
    }

    [Fact]
    public void GetIndexers_returns_only_indexer_properties()
    {
        MemberDiscovery discovery = new(typeof(MyIndexer), Accessibility.Public);
        Assert.Equal(2, discovery.GetIndexers().Length);
        Assert.All(discovery.GetIndexers(), p => Assert.NotEmpty(p.GetIndexParameters()));
    }

    [Fact]
    public void GetOperators_returns_op_methods()
    {
        MemberDiscovery discovery = new(typeof(MyOperators), Accessibility.Public);
        string[] names = discovery.GetOperators().Select(m => m.Name).ToArray();
        Assert.Contains("op_Addition", names);
        Assert.Contains("op_Implicit", names);
        Assert.Contains("op_Explicit", names);
    }

    [Fact]
    public void GetMethods_excludes_operators()
    {
        MemberDiscovery discovery = new(typeof(MyOperators), Accessibility.Public);
        Assert.DoesNotContain(discovery.GetMethods(), m => m.Name.StartsWith("op_"));
    }

    [Fact]
    public void GetMethods_on_record_class_excludes_synthesized_members()
    {
        MemberDiscovery discovery = new(typeof(Person), Accessibility.Public);
        string[] names = discovery.GetMethods().Select(m => m.Name).ToArray();
        Assert.DoesNotContain("<Clone>$", names);
        Assert.DoesNotContain("PrintMembers", names);
        Assert.DoesNotContain("Deconstruct", names);
        Assert.DoesNotContain("Equals", names);
        Assert.DoesNotContain("GetHashCode", names);
        Assert.DoesNotContain("ToString", names);
    }

    [Fact]
    public void GetProperties_on_record_class_excludes_EqualityContract()
    {
        MemberDiscovery discovery = new(typeof(Person), Accessibility.Public);
        Assert.DoesNotContain(discovery.GetProperties(), p => p.Name == "EqualityContract");
    }

    [Fact]
    public void GetOperators_on_record_class_excludes_synthesized_equality()
    {
        MemberDiscovery discovery = new(typeof(Person), Accessibility.Public);
        string[] names = discovery.GetOperators().Select(m => m.Name).ToArray();
        Assert.DoesNotContain("op_Equality", names);
        Assert.DoesNotContain("op_Inequality", names);
    }

    [Fact]
    public void GetMethods_on_record_struct_excludes_synthesized_members()
    {
        MemberDiscovery discovery = new(typeof(Point), Accessibility.Public);
        string[] names = discovery.GetMethods().Select(m => m.Name).ToArray();
        Assert.DoesNotContain("PrintMembers", names);
        Assert.DoesNotContain("Deconstruct", names);
        Assert.DoesNotContain("Equals", names);
        Assert.DoesNotContain("GetHashCode", names);
        Assert.DoesNotContain("ToString", names);
    }
}
