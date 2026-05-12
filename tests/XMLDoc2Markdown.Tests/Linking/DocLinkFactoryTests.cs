using System.Xml.Linq;
using XMLDoc2Markdown.Linking;

namespace XMLDoc2Markdown.Tests.Linking;

public class DocLinkFactoryTests
{
    [Theory]
    [InlineData("System")]
    [InlineData("System.Text.Json")]
    [InlineData("Microsoft")]
    [InlineData("Microsoft.Extensions.Logging")]
    [InlineData("Windows")]
    [InlineData("Windows.Foundation")]
    public void LooksLikeMicrosoftType_returns_true_for_msft_namespaces(string @namespace)
    {
        Assert.True(DocLinkFactory.LooksLikeMicrosoftType(@namespace));
    }

    [Theory]
    [InlineData("MyClassLib")]
    [InlineData("Newtonsoft.Json")]
    [InlineData("Newtonsoft.Json.Linq")]
    [InlineData("Systemic")]      // not "System."
    [InlineData("Microsoftish")]  // not "Microsoft."
    [InlineData("")]
    [InlineData(null)]
    public void LooksLikeMicrosoftType_returns_false_for_other_namespaces(string? @namespace)
    {
        Assert.False(DocLinkFactory.LooksLikeMicrosoftType(@namespace));
    }

    [Fact]
    public void GetMSDocsUrl_accepts_non_mscorlib_microsoft_type()
    {
        // XElement lives in System.Xml.Linq, which is NOT in mscorlib/System.Private.CoreLib.
        // The relaxed guard should accept it.
        string url = typeof(XElement).GetMSDocsUrl();
        Assert.Equal("https://learn.microsoft.com/en-us/dotnet/api/system.xml.linq.xelement", url);
    }

    [Fact]
    public void MsDocsUrlFromCref_builds_url_for_type_cref()
    {
        string? url = DocLinkFactory.MsDocsUrlFromCref("T:System.Text.Json.JsonElement");
        Assert.Equal("https://learn.microsoft.com/en-us/dotnet/api/system.text.json.jsonelement", url);
    }

    [Fact]
    public void MsDocsUrlFromCref_drops_parameter_list_for_method_cref()
    {
        string? url = DocLinkFactory.MsDocsUrlFromCref("M:System.String.Concat(System.String,System.String)");
        Assert.Equal("https://learn.microsoft.com/en-us/dotnet/api/system.string.concat", url);
    }

    [Fact]
    public void MsDocsUrlFromCref_replaces_generic_arity_marker_with_dash()
    {
        string? url = DocLinkFactory.MsDocsUrlFromCref("T:System.Collections.Generic.Dictionary`2");
        Assert.Equal("https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2", url);
    }

    [Fact]
    public void MsDocsUrlFromCref_returns_null_for_non_microsoft_namespace()
    {
        Assert.Null(DocLinkFactory.MsDocsUrlFromCref("T:Newtonsoft.Json.Linq.JObject"));
        Assert.Null(DocLinkFactory.MsDocsUrlFromCref("T:MyClassLib.MyClass"));
    }

    [Fact]
    public void MsDocsUrlFromCref_returns_null_for_malformed_cref()
    {
        Assert.Null(DocLinkFactory.MsDocsUrlFromCref(null));
        Assert.Null(DocLinkFactory.MsDocsUrlFromCref(""));
        Assert.Null(DocLinkFactory.MsDocsUrlFromCref("T:"));
        Assert.Null(DocLinkFactory.MsDocsUrlFromCref("System.Foo"));   // missing prefix colon
        Assert.Null(DocLinkFactory.MsDocsUrlFromCref("T:System"));     // no dot, no namespace
    }

    [Fact]
    public void MsDocsUrlFromCref_handles_unresolvable_but_microsoft_namespace()
    {
        // By policy: namespace looks Microsoft-owned → MS Learn URL even if the page may 404.
        string? url = DocLinkFactory.MsDocsUrlFromCref("T:System.NoSuchType");
        Assert.Equal("https://learn.microsoft.com/en-us/dotnet/api/system.nosuchtype", url);
    }
}
