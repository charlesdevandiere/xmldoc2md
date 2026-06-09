using System.Reflection;

namespace XMLDoc2Markdown.Signatures;

internal static class MemberSignatureDispatcher
{
    internal static string GetSignature(this MemberInfo memberInfo, bool full = false)
        => GetSignature(memberInfo, null, full);

    internal static string GetSignature(this MemberInfo memberInfo, NullabilityInfoContext? nullCtx, bool full) => memberInfo switch
    {
        Type type => type.GetSignature(full),
        MethodBase methodBase => methodBase.GetSignature(nullCtx, full),
        PropertyInfo propertyInfo => propertyInfo.GetSignature(nullCtx, full),
        EventInfo eventInfo => eventInfo.GetSignature(nullCtx, full),
        FieldInfo fieldInfo => fieldInfo.GetSignature(nullCtx, full),
        _ => throw new NotImplementedException()
    };
}
