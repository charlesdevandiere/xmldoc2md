using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace XMLDoc2Markdown.Utils
{
    internal static class EventInfoExtensions
    {
        internal static Visibility GetVisibility(this EventInfo eventInfo)
        {
            Visibility addMethodeVisibility = eventInfo.AddMethod?.GetVisibility() ?? Visibility.None;
            Visibility raiseMethodeVisibility = eventInfo.RaiseMethod?.GetVisibility() ?? Visibility.None;
            Visibility removeMethodeVisibility = eventInfo.RemoveMethod?.GetVisibility() ?? Visibility.None;

            Visibility addOrRaiseMethodVisibility = addMethodeVisibility.CompareTo(raiseMethodeVisibility) >= 0 ? addMethodeVisibility : raiseMethodeVisibility;

            return removeMethodeVisibility.CompareTo(addOrRaiseMethodVisibility) >= 0 ? removeMethodeVisibility : addOrRaiseMethodVisibility;
        }

        internal static string GetSignature(this EventInfo eventInfo, bool full = false)
        {
            var signature = new List<string>();

            if (full)
            {
                signature.Add(eventInfo.GetVisibility().Print());

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
                    signature.Add(eventInfo.EventHandlerType.GetSimplifiedName());
                }
            }

            signature.Add($"{eventInfo.Name}{(full ? ";" : string.Empty)}");

            return string.Join(' ', signature);
        }
    }
}
