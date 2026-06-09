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
        => GetOperatorSignature(methodInfo, null, full);

    internal static string GetOperatorSignature(this MethodInfo methodInfo, NullabilityInfoContext? nullCtx, bool full)
    {
        SignatureBuilder b = new();

        Type? declaring = methodInfo.DeclaringType;
        bool isInterface = declaring?.IsInterface ?? false;

        if (full)
        {
            b.AppendAccessibilityUnlessInterface(methodInfo.GetAccessibility(), isInterface)
             .AppendIfStatic(methodInfo.IsStatic)
             .AppendIfAbstract(methodInfo.IsAbstract, isInterface, methodInfo.IsStatic);
        }

        DisplayMeta returnMeta = nullCtx == null
            ? DisplayMeta.Empty
            : DisplayMeta.ForReturn(methodInfo, nullCtx);

        if (OperatorNames.IsConversion(methodInfo))
        {
            b.Append(methodInfo.Name == "op_Implicit" ? "implicit" : "explicit")
             .Append("operator")
             .Append(methodInfo.ReturnType.GetDisplayName(returnMeta, simplifyName: true));
        }
        else
        {
            if (full)
            {
                b.AppendReturnType(methodInfo.ReturnType, returnMeta);
            }
            b.Append("operator")
             .Append(OperatorNames.TryGetSymbol(methodInfo.Name) ?? methodInfo.Name);
        }

        b.AppendParameterList(methodInfo, nullCtx, full);

        return b.ToString();
    }
}
