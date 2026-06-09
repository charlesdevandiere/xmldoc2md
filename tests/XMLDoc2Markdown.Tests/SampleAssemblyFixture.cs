using System.Reflection;

namespace XMLDoc2Markdown.Tests;

public class SampleAssemblyFixture
{
    public Assembly Assembly { get; }
    public string DllPath { get; }
    internal XmlDocumentation Documentation { get; }

    public SampleAssemblyFixture()
    {
        this.DllPath = typeof(MyClassLib.MyClass).Assembly.Location;
        this.Assembly = typeof(MyClassLib.MyClass).Assembly;
        this.Documentation = new XmlDocumentation(this.DllPath);
    }
}

[CollectionDefinition(nameof(SampleAssemblyCollection))]
public class SampleAssemblyCollection : ICollectionFixture<SampleAssemblyFixture>
{
}
