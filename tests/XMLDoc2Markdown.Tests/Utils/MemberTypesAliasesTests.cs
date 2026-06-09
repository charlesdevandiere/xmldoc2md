using System.Reflection;
using XMLDoc2Markdown.XmlDocId;

namespace XMLDoc2Markdown.Tests.Utils;

public class MemberTypesAliasesTests
{
    [Theory]
    [InlineData(MemberTypes.Method, 'M')]
    [InlineData(MemberTypes.Constructor, 'M')]
    [InlineData(MemberTypes.TypeInfo, 'T')]
    [InlineData(MemberTypes.NestedType, 'T')]
    [InlineData(MemberTypes.Event, 'E')]
    [InlineData(MemberTypes.Field, 'F')]
    [InlineData(MemberTypes.Property, 'P')]
    public void TryGetAlias_known_member_types(MemberTypes type, char expected)
    {
        Assert.True(MemberTypesAliases.TryGetAlias(type, out char alias));
        Assert.Equal(expected, alias);
    }

    [Fact]
    public void TryGetAlias_unknown_member_type_returns_false()
    {
        Assert.False(MemberTypesAliases.TryGetAlias(MemberTypes.Custom, out char alias));
        Assert.Equal(default(char), alias);
    }

    [Theory]
    [InlineData('M', MemberTypes.Method)]
    [InlineData('T', MemberTypes.TypeInfo)]
    [InlineData('E', MemberTypes.Event)]
    [InlineData('F', MemberTypes.Field)]
    [InlineData('P', MemberTypes.Property)]
    public void TryGetMemberType_resolves_first_match_for_alias(char alias, MemberTypes expected)
    {
        Assert.True(MemberTypesAliases.TryGetMemberType(alias, out MemberTypes type));
        Assert.Equal(expected, type);
    }

    [Fact]
    public void TryGetMemberType_unknown_alias_returns_false()
    {
        Assert.False(MemberTypesAliases.TryGetMemberType('Z', out _));
    }

    [Fact]
    public void MemberTypesExtensions_GetAlias_throws_for_unknown()
    {
        Assert.Throws<NotImplementedException>(() => MemberTypes.Custom.GetAlias());
    }

    [Theory]
    [InlineData(MemberTypes.Property, 'P')]
    [InlineData(MemberTypes.Field, 'F')]
    public void MemberTypesExtensions_GetAlias_known(MemberTypes type, char expected)
    {
        Assert.Equal(expected, type.GetAlias());
    }
}
