using System;
using System.Reflection;

namespace XMLDoc2Markdown
{
    public static class MemberTypesExtensions
    {
        public static char GetAlias(this MemberTypes memberType)
        {
            return memberType switch
            {
                MemberTypes.Constructor => 'M',
                MemberTypes.Event => 'E',
                MemberTypes.Field => 'F',
                MemberTypes.Method => 'M',
                MemberTypes.NestedType => 'T',
                MemberTypes.Property => 'P',
                MemberTypes.TypeInfo => 'T',
                _ => throw new NotImplementedException(),
            };
        }
    }
}

