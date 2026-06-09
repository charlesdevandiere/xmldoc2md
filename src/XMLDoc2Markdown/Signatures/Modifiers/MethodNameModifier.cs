using System.Reflection;

namespace XMLDoc2Markdown.Signatures.Modifiers;

internal static class MethodNameModifier
{
    internal static SignatureBuilder AppendMethodName(this SignatureBuilder b, MethodBase methodBase)
    {
        string name = methodBase.MemberType == MemberTypes.Constructor && methodBase.DeclaringType != null
            ? methodBase.DeclaringType.Name
            : methodBase.Name;

        int tickIdx = name.IndexOf('`');
        if (tickIdx > -1)
        {
            name = name[..tickIdx];
        }

        if (methodBase is MethodInfo methodInfo)
        {
            Type[] genericArgs = methodInfo.GetGenericArguments();
            if (genericArgs.Length > 0)
            {
                name += $"<{string.Join(", ", genericArgs.Select(a => a.Name))}>";
            }
        }

        return b.Append(name);
    }
}
