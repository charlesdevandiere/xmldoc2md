using System.Reflection;
using XMLDoc2Markdown.Members;
using XMLDoc2Markdown.Signatures.Modifiers;

namespace XMLDoc2Markdown.Signatures;

internal static class FieldSignatureBuilder
{
    internal static string GetSignature(this FieldInfo fieldInfo, bool full = false)
    {
        SignatureBuilder b = new();

        if (full)
        {
            b.AppendAccessibility(fieldInfo.GetAccessibility())
             .AppendIfStatic(fieldInfo.IsStatic)
             .Append(fieldInfo.FieldType.GetDisplayName(simplifyName: true));
        }

        b.Append(fieldInfo.Name);

        if (full)
        {
            b.AppendJoined(";");
        }

        return b.ToString();
    }
}
