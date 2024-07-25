using System.Reflection;

namespace XMLDoc2Markdown.Utils;

internal static class EventInfoExtensions
{
    internal static Accessibility GetAccessibility(this EventInfo eventInfo)
    {
        Accessibility addMethodeVisibility = eventInfo.AddMethod?.GetAccessibility() ?? Accessibility.None;
        Accessibility raiseMethodeVisibility = eventInfo.RaiseMethod?.GetAccessibility() ?? Accessibility.None;
        Accessibility removeMethodeVisibility = eventInfo.RemoveMethod?.GetAccessibility() ?? Accessibility.None;

        Accessibility addOrRaiseMethodVisibility = addMethodeVisibility.CompareTo(raiseMethodeVisibility) >= 0 ? addMethodeVisibility : raiseMethodeVisibility;

        return removeMethodeVisibility.CompareTo(addOrRaiseMethodVisibility) >= 0 ? removeMethodeVisibility : addOrRaiseMethodVisibility;
    }

    internal static string GetSignature(this EventInfo eventInfo, bool full = false)
    {
        List<string> signature = [];

        if (full)
        {
            signature.Add(eventInfo.GetAccessibility().Print());

            if ((eventInfo.AddMethod?.IsStatic ?? false) ||
                (eventInfo.RaiseMethod?.IsStatic ?? false) ||
                (eventInfo.RemoveMethod?.IsStatic ?? false))
            {
                signature.Add("static");
            }

            if ((eventInfo.AddMethod?.IsAbstract ?? false) ||
                (eventInfo.RaiseMethod?.IsAbstract ?? false) ||
                (eventInfo.RemoveMethod?.IsAbstract ?? false))
            {
                signature.Add("abstract");
            }

            signature.Add("event");

            if (eventInfo.EventHandlerType != null)
            {
                signature.Add(eventInfo.EventHandlerType.GetDisplayName(simplifyName: true));
            }
        }

        signature.Add($"{eventInfo.Name}{(full ? ";" : string.Empty)}");

        return string.Join(' ', signature);
    }
}
