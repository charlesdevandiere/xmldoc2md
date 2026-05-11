using System.Reflection;
using XMLDoc2Markdown.Members;
using XMLDoc2Markdown.Signatures.Modifiers;

namespace XMLDoc2Markdown.Signatures;

internal static class MethodSignatureBuilder
{
    internal static string GetSignature(this MethodBase methodBase, bool full = false)
    {
        SignatureBuilder b = new();

        if (full && (methodBase.DeclaringType?.IsClass ?? false))
        {
            b.AppendAccessibility(methodBase.GetAccessibility())
             .AppendIfStatic(methodBase.IsStatic)
             .AppendIfAbstract(methodBase.IsAbstract);
        }

        if (full && methodBase is MethodInfo methodInfo)
        {
            b.AppendReturnType(methodInfo.ReturnType);
        }

        b.AppendMethodName(methodBase)
         .AppendParameterList(methodBase, full);

        return b.ToString();
    }
}
