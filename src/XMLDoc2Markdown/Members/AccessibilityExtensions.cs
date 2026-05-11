using System.Reflection;

namespace XMLDoc2Markdown.Members;

internal static class AccessibilityExtensions
{
    internal static string Print(this Accessibility accessibility) => accessibility switch
    {
        Accessibility.Public => "public",
        Accessibility.Internal => "internal",
        Accessibility.Protected => "protected",
        Accessibility.ProtectedInternal => "protected internal",
        Accessibility.Private => "private",
        _ => string.Empty
    };

    internal static Accessibility GetAccessibility(this Type type)
        => type.IsPublic ? Accessibility.Public : Accessibility.Internal;

    internal static Accessibility GetAccessibility(this MethodBase methodBase)
    {
        if (methodBase.IsPublic) return Accessibility.Public;
        if (methodBase.IsAssembly) return Accessibility.Internal;
        if (methodBase.IsFamily) return Accessibility.Protected;
        if (methodBase.IsFamilyOrAssembly) return Accessibility.ProtectedInternal;
        if (methodBase.IsPrivate) return Accessibility.Private;
        return Accessibility.None;
    }

    internal static Accessibility GetAccessibility(this FieldInfo fieldInfo)
    {
        if (fieldInfo.IsPublic) return Accessibility.Public;
        if (fieldInfo.IsAssembly) return Accessibility.Internal;
        if (fieldInfo.IsFamily) return Accessibility.Protected;
        if (fieldInfo.IsFamilyOrAssembly) return Accessibility.ProtectedInternal;
        if (fieldInfo.IsPrivate) return Accessibility.Private;
        return Accessibility.None;
    }

    internal static Accessibility GetAccessibility(this PropertyInfo propertyInfo)
    {
        Accessibility getter = propertyInfo.GetMethod?.GetAccessibility() ?? Accessibility.None;
        Accessibility setter = propertyInfo.SetMethod?.GetAccessibility() ?? Accessibility.None;
        return getter.CompareTo(setter) >= 0 ? getter : setter;
    }

    internal static Accessibility GetAccessibility(this EventInfo eventInfo)
    {
        Accessibility add = eventInfo.AddMethod?.GetAccessibility() ?? Accessibility.None;
        Accessibility raise = eventInfo.RaiseMethod?.GetAccessibility() ?? Accessibility.None;
        Accessibility remove = eventInfo.RemoveMethod?.GetAccessibility() ?? Accessibility.None;
        Accessibility addOrRaise = add.CompareTo(raise) >= 0 ? add : raise;
        return remove.CompareTo(addOrRaise) >= 0 ? remove : addOrRaise;
    }
}
