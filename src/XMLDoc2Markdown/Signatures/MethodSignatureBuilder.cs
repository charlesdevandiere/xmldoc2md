using System.Reflection;
using System.Runtime.CompilerServices;
using XMLDoc2Markdown.Members;
using XMLDoc2Markdown.Signatures.Modifiers;

namespace XMLDoc2Markdown.Signatures;

internal static class MethodSignatureBuilder
{
    internal static string GetSignature(this MethodBase methodBase, bool full = false)
        => GetSignature(methodBase, null, full);

    internal static string GetSignature(this MethodBase methodBase, NullabilityInfoContext? nullCtx, bool full)
    {
        if (OperatorNames.IsOperator(methodBase))
        {
            return ((MethodInfo)methodBase).GetOperatorSignature(nullCtx, full);
        }

        SignatureBuilder b = new();

        Type? declaring = methodBase.DeclaringType;
        bool isInterface = declaring?.IsInterface ?? false;
        bool emitModifiers = full && declaring != null && (declaring.IsClass || declaring.IsValueType || isInterface);

        if (emitModifiers)
        {
            b.AppendAccessibilityUnlessInterface(methodBase.GetAccessibility(), isInterface)
             .AppendIfStatic(methodBase.IsStatic)
             .AppendIfAbstract(methodBase.IsAbstract, isInterface, methodBase.IsStatic);

            if (methodBase is MethodInfo virtualityInfo)
            {
                b.AppendVirtuality(virtualityInfo);
            }

            b.AppendIfAsync(methodBase);
        }

        if (full && methodBase is MethodInfo methodInfo)
        {
            DisplayMeta returnMeta = nullCtx == null
                ? DisplayMeta.Empty
                : DisplayMeta.ForReturn(methodInfo, nullCtx);
            b.AppendReturnType(methodInfo.ReturnType, returnMeta);
        }

        b.AppendMethodName(methodBase)
         .AppendParameterList(methodBase, nullCtx, full);

        if (full)
        {
            b.AppendGenericConstraints(methodBase);
        }

        return b.ToString();
    }

    private static SignatureBuilder AppendIfAsync(this SignatureBuilder b, MethodBase methodBase)
        => methodBase.IsDefined(typeof(AsyncStateMachineAttribute), inherit: false)
            ? b.Append("async")
            : b;
}
