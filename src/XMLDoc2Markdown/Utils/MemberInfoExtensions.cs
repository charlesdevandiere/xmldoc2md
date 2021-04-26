using System;
using System.Collections.Generic;
using System.Linq;
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
            if (memberInfo is MethodBase methodBase)
            {
                Type[] genericArguments = methodBase switch {
                    ConstructorInfo _ => methodBase.DeclaringType.GetGenericArguments(),
                    MethodInfo _ => methodBase.GetGenericArguments(),
                    _ => Array.Empty<Type>()
                };

                if (methodBase is MethodInfo && methodBase.IsGenericMethod)
                {
                    identifier += $"``{genericArguments.Length}";
                }

                ParameterInfo[] parameterInfos = methodBase.GetParameters();
                if (parameterInfos.Length > 0)
                {
                    IEnumerable<string> @params = parameterInfos.Select(p =>
                    {
                        int index = Array.IndexOf(genericArguments, p.ParameterType);
                        return index > -1 ? $"{(methodBase is MethodInfo ? "``" : "`")}{index}" : p.ParameterType.ToString();
                    });
                    identifier = string.Concat(identifier, $"({string.Join(',', @params)})");
                }
            }

            return identifier;
        }
    }
}
