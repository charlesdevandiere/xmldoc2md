using System.Reflection;
using System.Xml.Linq;
using MyClassLib;

namespace XMLDoc2Markdown.Tests;

[Collection(nameof(SampleAssemblyCollection))]
public class XmlDocumentationTests
{
    private readonly SampleAssemblyFixture fixture;

    public XmlDocumentationTests(SampleAssemblyFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public void Constructor_throws_when_xml_file_missing()
    {
        Assert.Throws<FileNotFoundException>(() => new XmlDocumentation("/tmp/does-not-exist.dll"));
    }

    [Fact]
    public void AssemblyName_matches_sample()
    {
        Assert.Equal("MyClassLib", this.fixture.Documentation.AssemblyName);
    }

    [Fact]
    public void Members_indexes_every_documented_member()
    {
        Assert.True(this.fixture.Documentation.Members.Count > 0);
        Assert.Contains("T:MyClassLib.MyClass", this.fixture.Documentation.Members.Keys);
        Assert.Contains("P:MyClassLib.MyClass.MyProperty", this.fixture.Documentation.Members.Keys);
        Assert.Contains("F:MyClassLib.MyClass.myField", this.fixture.Documentation.Members.Keys);
        Assert.Contains("E:MyClassLib.MyClass.MyEvent", this.fixture.Documentation.Members.Keys);
        Assert.Contains("M:MyClassLib.MyClass.#ctor", this.fixture.Documentation.Members.Keys);
    }

    [Fact]
    public void GetMember_by_name_returns_xelement_with_summary()
    {
        XElement? element = this.fixture.Documentation.GetMember("T:MyClassLib.MyClass");
        Assert.NotNull(element);
        Assert.Equal("My class.", element!.Element("summary")!.Value.Trim());
    }

    [Fact]
    public void GetMember_by_name_returns_null_for_unknown_member()
    {
        Assert.Null(this.fixture.Documentation.GetMember("T:Nonexistent.Type"));
    }

    [Fact]
    public void GetMember_by_reflection_resolves_type()
    {
        XElement? element = this.fixture.Documentation.GetMember(typeof(MyClass));
        Assert.NotNull(element);
        Assert.Equal("My class.", element!.Element("summary")!.Value.Trim());
    }

    [Fact]
    public void GetMember_by_reflection_resolves_method()
    {
        MethodInfo method = typeof(MyClass).GetMethod(nameof(MyClass.Do))!;
        XElement? element = this.fixture.Documentation.GetMember(method);
        Assert.NotNull(element);
        Assert.Equal("Do some thing.", element!.Element("summary")!.Value.Trim());
    }

    [Fact]
    public void GetMember_by_reflection_resolves_property()
    {
        PropertyInfo prop = typeof(MyClass).GetProperty(nameof(MyClass.MyProperty))!;
        XElement? element = this.fixture.Documentation.GetMember(prop);
        Assert.NotNull(element);
        Assert.Equal("My property.", element!.Element("summary")!.Value.Trim());
    }

    [Fact]
    public void GetMember_by_reflection_resolves_field()
    {
        FieldInfo field = typeof(MyClass).GetField(nameof(MyClass.myField))!;
        XElement? element = this.fixture.Documentation.GetMember(field);
        Assert.NotNull(element);
        Assert.Equal("My field.", element!.Element("summary")!.Value.Trim());
    }

    [Fact]
    public void GetMember_by_reflection_resolves_event()
    {
        EventInfo evt = typeof(MyClass).GetEvent(nameof(MyClass.MyEvent))!;
        XElement? element = this.fixture.Documentation.GetMember(evt);
        Assert.NotNull(element);
        Assert.Equal("My event.", element!.Element("summary")!.Value.Trim());
    }

    [Fact]
    public void GetMember_by_reflection_resolves_constructor_with_params()
    {
        ConstructorInfo ctor = typeof(MyClass).GetConstructor([typeof(string), typeof(int)])!;
        XElement? element = this.fixture.Documentation.GetMember(ctor);
        Assert.NotNull(element);
        // The constructor doc carries two <param> elements named firstParam and secondParam.
        string[] paramNames = element!.Elements("param")
            .Select(p => p.Attribute("name")?.Value ?? string.Empty)
            .ToArray();
        Assert.Contains("firstParam", paramNames);
        Assert.Contains("secondParam", paramNames);
    }

    [Fact]
    public void GetMember_by_reflection_resolves_generic_method()
    {
        MethodInfo method = typeof(MyClass).GetMethod(nameof(MyClass.DoGeneric))!;
        XElement? element = this.fixture.Documentation.GetMember(method);
        Assert.NotNull(element);
        Assert.NotNull(element!.Element("typeparam"));
    }
}
