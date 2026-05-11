using System.Reflection;
using System.Text;
using XMLDoc2Markdown.Members;
using XMLDoc2Markdown.Signatures.Modifiers;

namespace XMLDoc2Markdown.Signatures;

/// <summary>
/// Renders C# indexer signatures: <c>this[Type index]</c>. The bracketed
/// parameter list replaces the synthesized <c>Item</c> name a regular
/// property would carry.
/// </summary>
internal static class IndexerSignatureBuilder
{
    internal static string GetIndexerSignature(this PropertyInfo propertyInfo, bool full = false)
        => GetIndexerSignature(propertyInfo, null, full);

    internal static string GetIndexerSignature(this PropertyInfo propertyInfo, NullabilityInfoContext? nullCtx, bool full)
    {
        SignatureBuilder b = new();

        if (full)
        {
            bool isStatic = (propertyInfo.GetMethod?.IsStatic ?? false)
                || (propertyInfo.SetMethod?.IsStatic ?? false);
            bool isAbstract = (propertyInfo.GetMethod?.IsAbstract ?? false)
                || (propertyInfo.SetMethod?.IsAbstract ?? false);
            bool isInterface = propertyInfo.DeclaringType?.IsInterface ?? false;

            b.AppendAccessibilityUnlessInterface(propertyInfo.GetAccessibility(), isInterface)
             .AppendIfStatic(isStatic)
             .AppendIfAbstract(isAbstract, isInterface, isStatic);

            DisplayMeta meta = nullCtx == null
                ? DisplayMeta.Empty
                : DisplayMeta.For(propertyInfo, nullCtx);
            b.AppendVirtuality(propertyInfo)
             .AppendReturnType(propertyInfo.GetReturnType(), meta);
        }

        b.Append($"this{FormatIndexParameters(propertyInfo, nullCtx, full)}");

        if (full)
        {
            b.AppendAccessors(propertyInfo);
        }

        return b.ToString();
    }

    private static string FormatIndexParameters(PropertyInfo propertyInfo, NullabilityInfoContext? nullCtx, bool full)
    {
        ParameterInfo[] @params = propertyInfo.GetIndexParameters();
        StringBuilder sb = new("[");
        for (int i = 0; i < @params.Length; i++)
        {
            if (i > 0) sb.Append(", ");
            sb.Append(ParameterListModifier.FormatParameter(@params[i], nullCtx, full));
        }
        return sb.Append(']').ToString();
    }
}
