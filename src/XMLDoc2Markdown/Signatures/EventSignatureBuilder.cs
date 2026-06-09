using System.Reflection;
using XMLDoc2Markdown.Members;
using XMLDoc2Markdown.Signatures.Modifiers;

namespace XMLDoc2Markdown.Signatures;

internal static class EventSignatureBuilder
{
    internal static string GetSignature(this EventInfo eventInfo, bool full = false)
        => GetSignature(eventInfo, null, full);

    internal static string GetSignature(this EventInfo eventInfo, NullabilityInfoContext? nullCtx, bool full)
    {
        SignatureBuilder b = new();

        if (full)
        {
            bool isStatic = (eventInfo.AddMethod?.IsStatic ?? false)
                || (eventInfo.RaiseMethod?.IsStatic ?? false)
                || (eventInfo.RemoveMethod?.IsStatic ?? false);
            bool isAbstract = (eventInfo.AddMethod?.IsAbstract ?? false)
                || (eventInfo.RaiseMethod?.IsAbstract ?? false)
                || (eventInfo.RemoveMethod?.IsAbstract ?? false);
            bool isInterface = eventInfo.DeclaringType?.IsInterface ?? false;

            b.AppendAccessibilityUnlessInterface(eventInfo.GetAccessibility(), isInterface)
             .AppendIfStatic(isStatic)
             .AppendIfAbstract(isAbstract, isInterface, isStatic)
             .AppendVirtuality(eventInfo)
             .Append("event");

            if (eventInfo.EventHandlerType != null)
            {
                DisplayMeta meta = nullCtx == null
                    ? DisplayMeta.Empty
                    : DisplayMeta.For(eventInfo, nullCtx);
                b.Append(eventInfo.EventHandlerType.GetDisplayName(meta, simplifyName: true));
            }
        }

        b.Append(eventInfo.Name);

        if (full)
        {
            b.AppendJoined(";");
        }

        return b.ToString();
    }
}
