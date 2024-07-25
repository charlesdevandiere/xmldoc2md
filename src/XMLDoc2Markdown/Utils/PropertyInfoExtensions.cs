using System.Reflection;

namespace XMLDoc2Markdown.Utils;

internal static class PropertyInfoExtensions
{
    internal static Accessibility GetAccessibility(this PropertyInfo propertyInfo)
    {
        Accessibility getMethodeVisibility = propertyInfo.GetMethod?.GetAccessibility() ?? Accessibility.None;
        Accessibility setMethodeVisibility = propertyInfo.SetMethod?.GetAccessibility() ?? Accessibility.None;

        return getMethodeVisibility.CompareTo(setMethodeVisibility) >= 0 ? getMethodeVisibility : setMethodeVisibility;
    }

    internal static Type? GetReturnType(this PropertyInfo propertyInfo)
    {
        return propertyInfo.GetMethod?.ReturnType ?? propertyInfo.SetMethod?.GetParameters()?.FirstOrDefault()?.ParameterType;
    }

    internal static string GetSignature(this PropertyInfo propertyInfo, bool full = false)
    {
        List<string?> signature = [];

        if (full)
        {
            signature.Add(propertyInfo.GetAccessibility().Print());

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
                Accessibility getMethodeVisibility = propertyInfo.GetMethod.GetAccessibility();
                if (getMethodeVisibility < propertyInfo.GetAccessibility())
                {
                    signature.Add(getMethodeVisibility.Print());
                }

                signature.Add("get;");
            }

            if (propertyInfo.SetMethod != null)
            {
                Accessibility setMethodeVisibility = propertyInfo.SetMethod.GetAccessibility();
                if (setMethodeVisibility < propertyInfo.GetAccessibility())
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
