using System.Reflection;

namespace XMLDoc2Markdown.Signatures;

internal static class MemberSignatureDispatcher
{
    internal static string GetSignature(this MemberInfo memberInfo, bool full = false) => memberInfo switch
    {
        Type type => type.GetSignature(full),
        MethodBase methodBase => methodBase.GetSignature(full),
        PropertyInfo propertyInfo => propertyInfo.GetSignature(full),
        EventInfo eventInfo => eventInfo.GetSignature(full),
        FieldInfo fieldInfo => fieldInfo.GetSignature(full),
        _ => throw new NotImplementedException()
    };
}
