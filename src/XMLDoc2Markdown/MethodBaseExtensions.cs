using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace XMLDoc2Markdown
{
    public static class MethodBaseExtensions
    {
        public static string GetVisibility(this MethodBase methodBase)
        {
            if (methodBase.IsPublic)
            {
                return "public";
            }
            else if (methodBase.IsAssembly)
            {
                return "internal";
            }
            else if (methodBase.IsFamily)
            {
                return "protected";
            }
            else if (methodBase.IsFamilyOrAssembly)
            {
                return "protected internal";
            }
            else if (methodBase.IsPrivate)
            {
                return "private";
            }
            else
            {
                return string.Empty;
            }
        }

        public static string GetSignature(this MethodBase methodBase, bool full = false)
        {
            var signature = new List<string>();

            if (full)
            {
                signature.Add(methodBase.GetVisibility());
            }

            if (full && methodBase is MethodInfo methodInfo)
            {
                signature.Add(methodInfo.ReturnType.GetSimplifiedName());
            }

            string displayName = methodBase.MemberType == MemberTypes.Constructor ? methodBase.DeclaringType.Name : methodBase.Name;
            ParameterInfo[] @params = methodBase.GetParameters();
            IEnumerable<string> paramsNames = @params
                .Select(p => $"{(full ? p.ParameterType.GetSimplifiedName() : p.ParameterType.Name)}{(full ? $" {p.Name}" : null)}");
            displayName += $"({string.Join(", ", paramsNames)})";
            signature.Add(displayName);

            return string.Join(' ', signature);
        }
    }
}
