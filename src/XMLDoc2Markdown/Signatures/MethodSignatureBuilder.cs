using System.Reflection;
using System.Runtime.CompilerServices;
using XMLDoc2Markdown.Members;
using XMLDoc2Markdown.Signatures.Modifiers;

namespace XMLDoc2Markdown.Signatures;

internal static class MethodSignatureBuilder
{
    internal static string GetSignature(this MethodBase methodBase, bool full = false)
    {
        if (OperatorNames.IsOperator(methodBase))
        {
            return ((MethodInfo)methodBase).GetOperatorSignature(full);
        }

        SignatureBuilder b = new();

        if (full && (methodBase.DeclaringType?.IsClass ?? false))
        {
            b.AppendAccessibility(methodBase.GetAccessibility())
             .AppendIfStatic(methodBase.IsStatic)
             .AppendIfAbstract(methodBase.IsAbstract)
             .AppendIfAsync(methodBase);
        }

        if (full && methodBase is MethodInfo methodInfo)
        {
            b.AppendReturnType(methodInfo.ReturnType);
        }

        b.AppendMethodName(methodBase)
         .AppendParameterList(methodBase, full);

        return b.ToString();
    }

    private static SignatureBuilder AppendIfAsync(this SignatureBuilder b, MethodBase methodBase)
        => methodBase.IsDefined(typeof(AsyncStateMachineAttribute), inherit: false)
            ? b.Append("async")
            : b;
}
