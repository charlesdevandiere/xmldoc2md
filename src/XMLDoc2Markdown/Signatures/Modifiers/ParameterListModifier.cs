using System.Reflection;

namespace XMLDoc2Markdown.Signatures.Modifiers;

internal static class ParameterListModifier
{
    internal static SignatureBuilder AppendParameterList(this SignatureBuilder b, MethodBase methodBase, bool full)
    {
        ParameterInfo[] @params = methodBase.GetParameters();
        IEnumerable<string> formatted = @params.Select(p =>
            $"{p.ParameterType.GetDisplayName(simplifyName: full)}{(full ? $" {p.Name}" : null)}");
        return b.AppendJoined($"({string.Join(", ", formatted)})");
    }
}
