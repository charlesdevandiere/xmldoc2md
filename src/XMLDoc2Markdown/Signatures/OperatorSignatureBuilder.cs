using System.Reflection;
using XMLDoc2Markdown.Members;
using XMLDoc2Markdown.Signatures.Modifiers;

namespace XMLDoc2Markdown.Signatures;

/// <summary>
/// Renders C# operator signatures. Conversion operators (<c>op_Implicit</c>,
/// <c>op_Explicit</c>) render as <c>implicit operator T(...)</c>; all other
/// <c>op_*</c> methods render as <c>T operator +(...)</c>.
/// </summary>
internal static class OperatorSignatureBuilder
{
    internal static string GetOperatorSignature(this MethodInfo methodInfo, bool full = false)
    {
        SignatureBuilder b = new();

        if (full)
        {
            b.AppendAccessibility(methodInfo.GetAccessibility())
             .AppendIfStatic(methodInfo.IsStatic);
        }

        if (OperatorNames.IsConversion(methodInfo))
        {
            b.Append(methodInfo.Name == "op_Implicit" ? "implicit" : "explicit")
             .Append("operator")
             .Append(methodInfo.ReturnType.GetDisplayName(simplifyName: true));
        }
        else
        {
            if (full)
            {
                b.AppendReturnType(methodInfo.ReturnType);
            }
            b.Append("operator")
             .Append(OperatorNames.TryGetSymbol(methodInfo.Name) ?? methodInfo.Name);
        }

        b.AppendParameterList(methodInfo, full);

        return b.ToString();
    }
}
