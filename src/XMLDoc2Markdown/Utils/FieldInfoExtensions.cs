using System.Reflection;

namespace XMLDoc2Markdown.Utils;

internal static class FieldInfoExtensions
{
    internal static Accessibility GetAccessibility(this FieldInfo fieldInfo)
    {
        if (fieldInfo.IsPublic)
        {
            return Accessibility.Public;
        }
        else if (fieldInfo.IsAssembly)
        {
            return Accessibility.Internal;
        }
        else if (fieldInfo.IsFamily)
        {
            return Accessibility.Protected;
        }
        else if (fieldInfo.IsFamilyOrAssembly)
        {
            return Accessibility.ProtectedInternal;
        }
        else if (fieldInfo.IsPrivate)
        {
            return Accessibility.Private;
        }
        else
        {
            return Accessibility.None;
        }
    }

    internal static string GetSignature(this FieldInfo fieldInfo, bool full = false)
    {
        List<string> signature = [];

        if (full)
        {
            signature.Add(fieldInfo.GetAccessibility().Print());

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
