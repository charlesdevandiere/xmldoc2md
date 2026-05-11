using System.Reflection;
using XMLDoc2Markdown.Members;
using XMLDoc2Markdown.Signatures.Modifiers;

namespace XMLDoc2Markdown.Signatures;

internal static class EventSignatureBuilder
{
    internal static string GetSignature(this EventInfo eventInfo, bool full = false)
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

            b.AppendAccessibility(eventInfo.GetAccessibility())
             .AppendIfStatic(isStatic)
             .AppendIfAbstract(isAbstract)
             .Append("event");

            if (eventInfo.EventHandlerType != null)
            {
                b.Append(eventInfo.EventHandlerType.GetDisplayName(simplifyName: true));
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
