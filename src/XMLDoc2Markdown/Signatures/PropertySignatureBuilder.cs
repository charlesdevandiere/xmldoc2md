using System.Reflection;
using XMLDoc2Markdown.Members;
using XMLDoc2Markdown.Signatures.Modifiers;

namespace XMLDoc2Markdown.Signatures;

internal static class PropertySignatureBuilder
{
    internal static Type? GetReturnType(this PropertyInfo propertyInfo)
        => propertyInfo.GetMethod?.ReturnType
            ?? propertyInfo.SetMethod?.GetParameters().FirstOrDefault()?.ParameterType;

    internal static string GetSignature(this PropertyInfo propertyInfo, bool full = false)
    {
        SignatureBuilder b = new();

        if (full)
        {
            bool isStatic = (propertyInfo.GetMethod?.IsStatic ?? false)
                || (propertyInfo.SetMethod?.IsStatic ?? false);
            bool isAbstract = (propertyInfo.GetMethod?.IsAbstract ?? false)
                || (propertyInfo.SetMethod?.IsAbstract ?? false);

            b.AppendAccessibility(propertyInfo.GetAccessibility())
             .AppendIfStatic(isStatic)
             .AppendIfAbstract(isAbstract)
             .AppendReturnType(propertyInfo.GetReturnType());
        }

        b.Append(propertyInfo.Name);

        if (full)
        {
            b.AppendAccessors(propertyInfo);
        }

        return b.ToString();
    }
}
