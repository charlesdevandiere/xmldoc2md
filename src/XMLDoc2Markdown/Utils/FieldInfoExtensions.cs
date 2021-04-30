using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace XMLDoc2Markdown.Utils
{
    internal static class FieldInfoExtensions
    {
        internal static Visibility GetVisibility(this FieldInfo fieldInfo)
        {
            if (fieldInfo.IsPublic)
            {
                return Visibility.Public;
            }
            else if (fieldInfo.IsAssembly)
            {
                return Visibility.Internal;
            }
            else if (fieldInfo.IsFamily)
            {
                return Visibility.Protected;
            }
            else if (fieldInfo.IsFamilyOrAssembly)
            {
                return Visibility.ProtectedInternal;
            }
            else if (fieldInfo.IsPrivate)
            {
                return Visibility.Private;
            }
            else
            {
                return Visibility.None;
            }
        }

        internal static string GetSignature(this FieldInfo fieldInfo, bool full = false)
        {
            var signature = new List<string>();

            if (full)
            {
                signature.Add(fieldInfo.GetVisibility().Print());

                if (fieldInfo.IsStatic)
                {
                    signature.Add("static");
                }

                signature.Add(fieldInfo.FieldType.GetDisplayName(simplifyName: true));
            }

            signature.Add($"{fieldInfo.Name}{(full ? ";" : string.Empty)}");

            return string.Join(' ', signature);
        }
    }
}
