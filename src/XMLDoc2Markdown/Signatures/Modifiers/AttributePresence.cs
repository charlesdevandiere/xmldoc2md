using System.Reflection;

namespace XMLDoc2Markdown.Signatures.Modifiers;

/// <summary>
/// Shared <c>[Attribute]</c>-by-full-name check. Modifiers match by full name
/// (not <c>typeof</c>) so a target assembly built against a different
/// reference of the same compiler-services type still matches.
/// </summary>
internal static class AttributePresence
{
    internal static bool HasAttribute(this MemberInfo member, string fullName)
        => member.CustomAttributes.Any(a => a.AttributeType.FullName == fullName);

    internal static bool HasAttribute(this ParameterInfo parameter, string fullName)
        => parameter.CustomAttributes.Any(a => a.AttributeType.FullName == fullName);
}
