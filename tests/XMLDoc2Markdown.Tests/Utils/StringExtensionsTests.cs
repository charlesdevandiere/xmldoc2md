using XMLDoc2Markdown.Utils;

namespace XMLDoc2Markdown.Tests.Utils;

public class StringExtensionsTests
{
    [Theory]
    [InlineData("List<string>", "List&lt;string&gt;")]
    [InlineData("Dictionary<TKey, TValue>", "Dictionary&lt;TKey, TValue&gt;")]
    [InlineData("plain", "plain")]
    [InlineData("", "")]
    [InlineData("<>", "&lt;&gt;")]
    public void FormatChevrons_escapes_angle_brackets(string input, string expected)
    {
        Assert.Equal(expected, input.FormatChevrons());
    }

    [Theory]
    [InlineData("MyMethod()", "mymethod")]
    // Punctuation chars are removed in place (not replaced with '-'); only spaces become '-'.
    [InlineData("Do(string firstParam, int secondParam)", "dostring-firstparam-int-secondparam")]
    [InlineData("DoGeneric<T>(T value)", "dogenerictt-value")]
    [InlineData("Get(List<string> param)", "getliststring-param")]
    [InlineData("Foo[] bar", "foo-bar")]
    public void ToAnchorLink_strips_punctuation_and_lowercases(string input, string expected)
    {
        Assert.Equal(expected, input.ToAnchorLink());
    }
}
