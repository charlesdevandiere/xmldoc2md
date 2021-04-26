using System;
using System.Reflection;

namespace XMLDoc2Markdown.Utils
{
    internal static class MemberInfoExtensions
    {
        internal static string GetSignature(this MemberInfo memberInfo, bool full = false)
        {
            if (memberInfo is Type type)
            {
                return type.GetSignature(full);
            }
            else if (memberInfo is MethodBase methodBase)
            {
                return methodBase.GetSignature(full);
            }
            else if (memberInfo is PropertyInfo propertyInfo)
            {
                return propertyInfo.GetSignature(full);
            }
            else if (memberInfo is EventInfo eventInfo)
            {
                return eventInfo.GetSignature(full);
            }

            throw new NotImplementedException();
        }

        internal static string GetIdentifier(this MemberInfo memberInfo)
        {
            string identifier;
            if (memberInfo is Type type)
            {
                identifier = type.FullName;
            }
            else
            {
                string name = memberInfo is ConstructorInfo ? "#ctor" : memberInfo.Name;
                identifier = $"{memberInfo.DeclaringType.Namespace}.{memberInfo.DeclaringType.Name}.{name}";
            }

            return identifier;
        }
    }
}
