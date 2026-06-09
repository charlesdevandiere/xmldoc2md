using XMLDoc2Markdown.Members;

namespace XMLDoc2Markdown.Tests.Utils;

public class AccessibilityExtensionsTests
{
    [Theory]
    [InlineData((int)Accessibility.Public, "public")]
    [InlineData((int)Accessibility.Internal, "internal")]
    [InlineData((int)Accessibility.Protected, "protected")]
    [InlineData((int)Accessibility.ProtectedInternal, "protected internal")]
    [InlineData((int)Accessibility.Private, "private")]
    [InlineData((int)Accessibility.None, "")]
    public void Print_returns_csharp_keyword(int accessibility, string expected)
    {
        Assert.Equal(expected, ((Accessibility)accessibility).Print());
    }

    [Fact]
    public void Accessibility_levels_are_ordered_lowest_to_highest()
    {
        Assert.True(Accessibility.None < Accessibility.Private);
        Assert.True(Accessibility.Private < Accessibility.Internal);
        Assert.True(Accessibility.Internal < Accessibility.Protected);
        Assert.True(Accessibility.Protected < Accessibility.ProtectedInternal);
        Assert.True(Accessibility.ProtectedInternal < Accessibility.Public);
    }
}
