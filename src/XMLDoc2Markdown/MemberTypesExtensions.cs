using System;
using System.Reflection;

namespace XMLDoc2Markdown
{
    internal static class MemberTypesExtensions
    {
        internal static char GetAlias(this MemberTypes memberType)
        {
            if (MemberTypesAliases.TryGetAlias(memberType, out char alias))
            {
                return alias;
            }

            throw new NotImplementedException();
        }
    }
}
