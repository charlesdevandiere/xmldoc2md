using System.Reflection;
using MyClassLib;
using XMLDoc2Markdown.Members;
using XMLDoc2Markdown.Signatures;

namespace XMLDoc2Markdown.Tests.Utils;

[Collection(nameof(SampleAssemblyCollection))]
public class EventInfoExtensionsTests
{
    [Fact]
    public void GetAccessibility_public_event()
    {
        EventInfo e = typeof(MyClass).GetEvent(nameof(MyClass.MyEvent))!;
        Assert.Equal(Accessibility.Public, e.GetAccessibility());
    }

    [Fact]
    public void GetAccessibility_private_event()
    {
        EventInfo e = typeof(MyClass).GetEvent(
            "MyPrivateEvent",
            BindingFlags.NonPublic | BindingFlags.Instance)!;
        Assert.Equal(Accessibility.Private, e.GetAccessibility());
    }

    [Fact]
    public void GetSignature_short_form_is_name_only()
    {
        EventInfo e = typeof(MyClass).GetEvent(nameof(MyClass.MyEvent))!;
        Assert.Equal("MyEvent", e.GetSignature());
    }

    [Fact]
    public void GetSignature_full_includes_event_keyword_handler_type_and_semicolon()
    {
        EventInfo e = typeof(MyClass).GetEvent(nameof(MyClass.MyEvent))!;
        string sig = e.GetSignature(full: true);

        Assert.Contains("public", sig);
        Assert.Contains("event", sig);
        Assert.Contains("EventHandler", sig);
        Assert.EndsWith("MyEvent;", sig);
    }
}
