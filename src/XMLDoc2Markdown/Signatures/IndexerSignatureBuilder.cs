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

        b.Append($"this{FormatIndexParameters(propertyInfo, full)}");

        if (full)
        {
            b.AppendAccessors(propertyInfo);
        }

        return b.ToString();
    }

    private static string FormatIndexParameters(PropertyInfo propertyInfo, bool full)
    {
        ParameterInfo[] @params = propertyInfo.GetIndexParameters();
        StringBuilder sb = new("[");
        for (int i = 0; i < @params.Length; i++)
        {
            if (i > 0) sb.Append(", ");
            sb.Append(@params[i].ParameterType.GetDisplayName(simplifyName: true));
            if (full)
            {
                sb.Append(' ').Append(@params[i].Name);
            }
        }
        return sb.Append(']').ToString();
    }
}
