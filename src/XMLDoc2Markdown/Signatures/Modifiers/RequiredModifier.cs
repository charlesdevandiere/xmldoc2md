using System.Reflection;

namespace XMLDoc2Markdown.Signatures.Modifiers;

internal static class RequiredModifier
{
    private const string RequiredMemberAttributeFullName =
        "System.Runtime.CompilerServices.RequiredMemberAttribute";

    internal static SignatureBuilder AppendIfRequired(this SignatureBuilder b, MemberInfo member)
        => member.CustomAttributes.Any(a => a.AttributeType.FullName == RequiredMemberAttributeFullName)
            ? b.Append("required")
            : b;
}
