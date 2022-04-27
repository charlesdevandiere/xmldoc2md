using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace XMLDoc2Markdown.Utils
{
    internal static class PropertyInfoExtensions
    {
        internal static Visibility GetVisibility(this PropertyInfo propertyInfo)
        {
            Visibility getMethodeVisibility = propertyInfo.GetMethod?.GetVisibility() ?? Visibility.None;
            Visibility setMethodeVisibility = propertyInfo.SetMethod?.GetVisibility() ?? Visibility.None;

            return getMethodeVisibility.CompareTo(setMethodeVisibility) >= 0 ? getMethodeVisibility : setMethodeVisibility;
        }

        internal static Type GetReturnType(this PropertyInfo propertyInfo)
        {
            return propertyInfo.GetMethod?.ReturnType ?? propertyInfo.SetMethod?.GetParameters()?.FirstOrDefault()?.ParameterType;
        }

        internal static string GetSignature(this PropertyInfo propertyInfo, bool full = false)
        {
            List<string> signature = new List<string>();

            if (full)
            {
                signature.Add(propertyInfo.GetVisibility().Print());

                if (propertyInfo.GetMethod != null && propertyInfo.GetMethod.IsStatic ||
                    propertyInfo.SetMethod != null && propertyInfo.SetMethod.IsStatic)
                {
                    signature.Add("static");
                }

                if (propertyInfo.GetMethod != null && propertyInfo.GetMethod.IsAbstract ||
                    propertyInfo.SetMethod != null && propertyInfo.SetMethod.IsAbstract)
                {
                    signature.Add("abstract");
                }

                signature.Add(propertyInfo.GetReturnType()?.GetDisplayName(simplifyName: true));
            }

            signature.Add(propertyInfo.Name);

            if (full)
            {
                signature.Add("{");

                if (propertyInfo.GetMethod != null)
                {
                    Visibility getMethodeVisibility = propertyInfo.GetMethod.GetVisibility();
                    if (getMethodeVisibility < propertyInfo.GetVisibility())
                    {
                        signature.Add(getMethodeVisibility.Print());
                    }

                    signature.Add("get;");
                }

                if (propertyInfo.SetMethod != null)
                {
                    Visibility setMethodeVisibility = propertyInfo.SetMethod.GetVisibility();
                    if (setMethodeVisibility < propertyInfo.GetVisibility())
                    {
                        signature.Add(setMethodeVisibility.Print());
                    }

                    signature.Add("set;");
                }

                signature.Add("}");
            }

            return string.Join(' ', signature);
        }
    }
}
