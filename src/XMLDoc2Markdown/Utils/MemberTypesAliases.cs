using System.Reflection;

namespace XMLDoc2Markdown.Utils;

internal static class MemberTypesAliases
{
    // Note: Constructor and Method both use the alias 'M' in C# XML doc IDs, and TypeInfo and NestedType
    // both use 'T'. TryGetMemberType returns the first match for an alias — Method and TypeInfo are listed
    // first so reverse lookups land on the more general member type. Forward lookups (TryGetAlias) are
    // keyed by MemberTypes and remain correct regardless of order.
    internal static readonly (MemberTypes memberType, char alias)[] ALIASES = [
        (MemberTypes.Method, 'M'),
        (MemberTypes.Constructor, 'M'),
        (MemberTypes.TypeInfo, 'T'),
        (MemberTypes.NestedType, 'T'),
        (MemberTypes.Event, 'E'),
        (MemberTypes.Field, 'F'),
        (MemberTypes.Property, 'P'),
    ];

    /// <summary>
    /// Gets the alias associated with the specified member type.
    /// </summary>
    /// <param name="memberType">The member type of the alias to get.</param>
    /// <param name="alias">
    /// When this method returns, contains the alias associated with the specified member type, if the member type is found;
    /// otherwise, the default char. This parameter is passed uninitialized
    /// </param>
    /// <returns>true if the member type is found; otherwise, false</returns>
    internal static bool TryGetAlias(MemberTypes memberType, out char alias)
    {
        int index = Array.FindIndex(ALIASES, element => element.memberType == memberType);
        if (index > -1)
        {
            alias = ALIASES[index].alias;
            return true;
        }

        alias = default;
        return false;
    }

    /// <summary>
    /// Gets the member type associated with the specified alias.
    /// </summary>
    /// <param name="memberType">The alias of the member type to get.</param>
    /// <param name="alias">
    /// When this method returns, contains the member type associated with the specified alias, if the alias is found;
    /// otherwise, the default member type. This parameter is passed uninitialized
    /// </param>
    /// <returns>true if the alias is found; otherwise, false</returns>
    internal static bool TryGetMemberType(char alias, out MemberTypes memberType)
    {
        int index = Array.FindIndex(ALIASES, element => element.alias == alias);
        if (index > -1)
        {
            memberType = ALIASES[index].memberType;
            return true;
        }

        memberType = default;
        return false;
    }
}
