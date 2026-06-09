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
        => GetSignature(propertyInfo, null, full);

    internal static string GetSignature(this PropertyInfo propertyInfo, NullabilityInfoContext? nullCtx, bool full)
    {
        if (propertyInfo.GetIndexParameters().Length > 0)
        {
            return propertyInfo.GetIndexerSignature(nullCtx, full);
        }

        SignatureBuilder b = new();

        if (full)
        {
            bool isStatic = (propertyInfo.GetMethod?.IsStatic ?? false)
                || (propertyInfo.SetMethod?.IsStatic ?? false);
            bool isAbstract = (propertyInfo.GetMethod?.IsAbstract ?? false)
                || (propertyInfo.SetMethod?.IsAbstract ?? false);
            bool isInterface = propertyInfo.DeclaringType?.IsInterface ?? false;

            b.AppendAccessibilityUnlessInterface(propertyInfo.GetAccessibility(), isInterface)
             .AppendIfRequired(propertyInfo)
             .AppendIfStatic(isStatic)
             .AppendIfAbstract(isAbstract, isInterface, isStatic);

            DisplayMeta meta = nullCtx == null
                ? DisplayMeta.Empty
                : DisplayMeta.For(propertyInfo, nullCtx);
            b.AppendVirtuality(propertyInfo)
             .AppendReturnType(propertyInfo.GetReturnType(), meta);
        }

        b.Append(propertyInfo.Name);

        if (full)
        {
            b.AppendAccessors(propertyInfo);
        }

        return b.ToString();
    }
}
