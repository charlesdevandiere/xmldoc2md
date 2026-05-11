using System.Reflection;

namespace XMLDoc2Markdown.XmlDocId;

internal static class MemberTypesAliases
{
    // Note: Constructor and Method both use the alias 'M' in C# XML doc IDs, and TypeInfo and NestedType
    // both use 'T'. TryGetMemberType returns the first match for an alias — Method and TypeInfo are listed
    // first so reverse lookups land on the more general member type.
    internal static readonly (MemberTypes memberType, char alias)[] ALIASES = [
        (MemberTypes.Method, 'M'),
        (MemberTypes.Constructor, 'M'),
        (MemberTypes.TypeInfo, 'T'),
        (MemberTypes.NestedType, 'T'),
        (MemberTypes.Event, 'E'),
        (MemberTypes.Field, 'F'),
        (MemberTypes.Property, 'P'),
    ];

    internal static bool TryGetAlias(MemberTypes memberType, out char alias)
    {
        int index = Array.FindIndex(ALIASES, e => e.memberType == memberType);
        if (index > -1)
        {
            alias = ALIASES[index].alias;
            return true;
        }
        alias = default;
        return false;
    }

    internal static bool TryGetMemberType(char alias, out MemberTypes memberType)
    {
        int index = Array.FindIndex(ALIASES, e => e.alias == alias);
        if (index > -1)
        {
            memberType = ALIASES[index].memberType;
            return true;
        }
        memberType = default;
        return false;
    }

    internal static char GetAlias(this MemberTypes memberType)
    {
        if (TryGetAlias(memberType, out char alias))
        {
            return alias;
        }
        throw new NotImplementedException();
    }
}
