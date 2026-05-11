using XMLDoc2Markdown.Members;
using XMLDoc2Markdown.Signatures.Modifiers;

namespace XMLDoc2Markdown.Signatures;

internal static class TypeSignatureBuilder
{
    internal static string GetSignature(this Type type, bool full = false)
    {
        SignatureBuilder b = new();

        if (full)
        {
            b.AppendAccessibility(type.GetAccessibility())
             .AppendTypeKind(type);
        }

        b.Append(type.GetDisplayName())
         .AppendBaseList(type);

        if (full)
        {
            b.AppendGenericConstraints(type);
        }

        return b.ToString();
    }
}
