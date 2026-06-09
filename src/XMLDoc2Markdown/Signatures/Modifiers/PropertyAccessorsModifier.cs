using System.Reflection;
using XMLDoc2Markdown.Members;

namespace XMLDoc2Markdown.Signatures.Modifiers;

internal static class PropertyAccessorsModifier
{
    internal static SignatureBuilder AppendAccessors(this SignatureBuilder b, PropertyInfo propertyInfo)
    {
        Accessibility propertyAccessibility = propertyInfo.GetAccessibility();

        b.Append("{");

        if (propertyInfo.GetMethod != null)
        {
            Accessibility getter = propertyInfo.GetMethod.GetAccessibility();
            if (getter < propertyAccessibility)
            {
                b.Append(getter.Print());
            }
            b.Append("get;");
        }

        if (propertyInfo.SetMethod != null)
        {
            Accessibility setter = propertyInfo.SetMethod.GetAccessibility();
            if (setter < propertyAccessibility)
            {
                b.Append(setter.Print());
            }
            b.Append(IsInitOnly(propertyInfo.SetMethod) ? "init;" : "set;");
        }

        return b.Append("}");
    }

    private static bool IsInitOnly(MethodInfo setter)
        => setter.ReturnParameter
            .GetRequiredCustomModifiers()
            .Any(t => t.FullName == "System.Runtime.CompilerServices.IsExternalInit");
}
