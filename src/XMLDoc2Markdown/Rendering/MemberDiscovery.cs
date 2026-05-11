using System.Reflection;
using XMLDoc2Markdown.Members;
using XMLDoc2Markdown.Signatures;

namespace XMLDoc2Markdown.Rendering;

/// <summary>
/// Single point of truth for "which members do we render?".
/// Centralizes filtering (accessibility, compiler-generated, special-name).
/// </summary>
internal sealed class MemberDiscovery
{
    private const string BackingFieldName = ">k__BackingField";
    private const BindingFlags AllInstanceAndStatic =
        BindingFlags.Public | BindingFlags.NonPublic |
        BindingFlags.Instance | BindingFlags.Static;

    private readonly Type type;
    private readonly Accessibility minAccessibility;

    internal MemberDiscovery(Type type, Accessibility minAccessibility)
    {
        this.type = type;
        this.minAccessibility = minAccessibility;
    }

    internal FieldInfo[] GetFields()
    {
        HashSet<string> eventNames = new(this.GetEvents().Select(e => e.Name), StringComparer.Ordinal);
        return this.type.GetFields(AllInstanceAndStatic)
            .Where(f => !f.Name.EndsWith(BackingFieldName))
            .Where(f => !eventNames.Contains(f.Name))
            .Where(f => f.GetAccessibility() >= this.minAccessibility)
            .ToArray();
    }

    internal FieldInfo[] GetEnumFields() =>
        this.type.GetFields(AllInstanceAndStatic)
            .Where(f => !f.IsSpecialName)
            .ToArray();

    internal PropertyInfo[] GetProperties() =>
        this.type.GetProperties(AllInstanceAndStatic)
            .Where(p => p.GetIndexParameters().Length == 0)
            .Where(p => p.GetAccessibility() >= this.minAccessibility)
            .ToArray();

    internal PropertyInfo[] GetIndexers() =>
        this.type.GetProperties(AllInstanceAndStatic)
            .Where(p => p.GetIndexParameters().Length > 0)
            .Where(p => p.GetAccessibility() >= this.minAccessibility)
            .ToArray();

    internal ConstructorInfo[] GetConstructors() =>
        this.type.GetConstructors(AllInstanceAndStatic)
            .Where(c => c.GetAccessibility() >= this.minAccessibility)
            .ToArray();

    internal MethodInfo[] GetMethods() =>
        this.type.GetMethods(AllInstanceAndStatic | BindingFlags.DeclaredOnly)
            .Where(m => !m.IsSpecialName)
            .Where(m => m.GetAccessibility() >= this.minAccessibility)
            .ToArray();

    internal MethodInfo[] GetOperators() =>
        this.type.GetMethods(AllInstanceAndStatic | BindingFlags.DeclaredOnly)
            .Where(OperatorNames.IsOperator)
            .Where(m => m.GetAccessibility() >= this.minAccessibility)
            .ToArray();

    internal EventInfo[] GetEvents() =>
        this.type.GetEvents(AllInstanceAndStatic)
            .Where(e => e.GetAccessibility() >= this.minAccessibility)
            .ToArray();
}
