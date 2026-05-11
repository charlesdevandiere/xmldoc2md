using System.Reflection;
using XMLDoc2Markdown.Members;
using XMLDoc2Markdown.Signatures.Modifiers;

namespace XMLDoc2Markdown.Signatures;

internal static class FieldSignatureBuilder
{
    internal static string GetSignature(this FieldInfo fieldInfo, bool full = false)
        => GetSignature(fieldInfo, null, full);

    internal static string GetSignature(this FieldInfo fieldInfo, NullabilityInfoContext? nullCtx, bool full)
    {
        SignatureBuilder b = new();

        if (full)
        {
            DisplayMeta meta = nullCtx == null
                ? DisplayMeta.Empty
                : DisplayMeta.For(fieldInfo, nullCtx);
            b.AppendAccessibility(fieldInfo.GetAccessibility())
             .AppendIfRequired(fieldInfo)
             .AppendIfStatic(fieldInfo.IsStatic)
             .Append(fieldInfo.FieldType.GetDisplayName(meta, simplifyName: true));
        }

        b.Append(fieldInfo.Name);

        if (full)
        {
            b.AppendJoined(";");
        }

        return b.ToString();
    }
}
