using System.Reflection;
using System.Xml.Linq;

namespace XMLDoc2Markdown.Rendering;

/// <summary>
/// Expands <c>&lt;inheritdoc/&gt;</c> elements by merging documentation from
/// the inheritance source. Sources are resolved either from an explicit
/// <c>cref</c> attribute or, when absent, by walking the inheritance chain:
/// overridden member, implemented interface member, or base type/constructor.
/// Local tags on the target win over inherited ones; <c>param</c>,
/// <c>typeparam</c> and <c>exception</c> are matched by <c>name</c>/<c>cref</c>.
/// Cycle protection is enforced.
/// </summary>
/// <remarks>
/// The <c>path="..."</c> attribute (XPath filtering) and inheritdoc child-filter
/// syntax are not supported in this pass.
/// </remarks>
internal sealed class InheritDocResolver
{
    private const BindingFlags AllInstance =
        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

    private readonly XmlDocumentation documentation;
    private readonly CrefResolver crefResolver;

    internal InheritDocResolver(XmlDocumentation documentation, CrefResolver crefResolver)
    {
        this.documentation = documentation;
        this.crefResolver = crefResolver;
    }

    internal XElement Resolve(MemberInfo member, XElement original)
    {
        if (!HasInheritDoc(original))
        {
            return original;
        }

        HashSet<string> visited = new(StringComparer.Ordinal) { IdentityKey(member) };
        return this.Expand(member, original, visited);
    }

    private XElement Expand(MemberInfo member, XElement original, HashSet<string> visited)
    {
        XElement merged = new(original);

        foreach (XElement placeholder in merged.Elements("inheritdoc").ToList())
        {
            XElement? sourceDoc = this.ResolveSourceDoc(member, placeholder, visited);
            if (sourceDoc is not null)
            {
                foreach (XElement child in sourceDoc.Elements())
                {
                    if (child.Name == "inheritdoc")
                    {
                        continue;
                    }
                    if (!IsAlreadyPresent(merged, child))
                    {
                        placeholder.AddBeforeSelf(new XElement(child));
                    }
                }
            }
            placeholder.Remove();
        }

        return merged;
    }

    private XElement? ResolveSourceDoc(MemberInfo target, XElement placeholder, HashSet<string> visited)
    {
        MemberInfo? source = this.ResolveSourceMember(target, placeholder);
        if (source is null)
        {
            return null;
        }

        if (!visited.Add(IdentityKey(source)))
        {
            return null;
        }

        XElement? raw = this.documentation.GetMember(source);
        if (raw is null)
        {
            return null;
        }

        return HasInheritDoc(raw) ? this.Expand(source, raw, visited) : raw;
    }

    private MemberInfo? ResolveSourceMember(MemberInfo target, XElement placeholder)
    {
        string? cref = placeholder.Attribute("cref")?.Value;
        if (!string.IsNullOrEmpty(cref))
        {
            return this.crefResolver.TryResolve(cref, out MemberInfo? resolved) ? resolved : null;
        }

        return this.FindInheritanceSource(target);
    }

    private static bool HasInheritDoc(XElement element) => element.Elements("inheritdoc").Any();

    private static bool IsAlreadyPresent(XElement merged, XElement candidate)
    {
        string tag = candidate.Name.LocalName;
        string? nameAttr = tag is "param" or "typeparam" ? candidate.Attribute("name")?.Value : null;
        string? crefAttr = tag is "exception" ? candidate.Attribute("cref")?.Value : null;

        foreach (XElement existing in merged.Elements(tag))
        {
            if (existing.Name == "inheritdoc")
            {
                continue;
            }
            if (nameAttr is not null)
            {
                if (existing.Attribute("name")?.Value == nameAttr) return true;
            }
            else if (crefAttr is not null)
            {
                if (existing.Attribute("cref")?.Value == crefAttr) return true;
            }
            else
            {
                return true;
            }
        }
        return false;
    }

    private MemberInfo? FindInheritanceSource(MemberInfo target) => target switch
    {
        Type t => this.FindBaseType(t),
        ConstructorInfo c => FindBaseConstructor(c),
        MethodInfo m => FindMethodSource(m),
        PropertyInfo p => FindPropertySource(p),
        EventInfo e => FindEventSource(e),
        _ => null
    };

    private Type? FindBaseType(Type type)
    {
        Type? baseType = type.BaseType;
        bool baseIsDocumentable = baseType is not null
            && baseType != typeof(object)
            && baseType != typeof(ValueType)
            && baseType != typeof(Enum)
            && baseType != typeof(MulticastDelegate)
            && baseType != typeof(Delegate);

        if (baseIsDocumentable && this.documentation.GetMember(baseType!) is not null)
        {
            return baseType;
        }

        foreach (Type iface in type.GetInterfaces())
        {
            if (this.documentation.GetMember(iface) is not null)
            {
                return iface;
            }
        }

        return baseIsDocumentable ? baseType : type.GetInterfaces().FirstOrDefault();
    }

    private static ConstructorInfo? FindBaseConstructor(ConstructorInfo ctor)
    {
        Type? baseType = ctor.DeclaringType?.BaseType;
        if (baseType is null || baseType == typeof(object))
        {
            return null;
        }
        Type[] paramTypes = ctor.GetParameters().Select(p => p.ParameterType).ToArray();
        return baseType.GetConstructor(AllInstance, binder: null, paramTypes, modifiers: null);
    }

    private static MethodInfo? FindMethodSource(MethodInfo method)
    {
        if (method.IsVirtual)
        {
            MethodInfo baseDef = method.GetBaseDefinition();
            if (baseDef != method && baseDef.DeclaringType != method.DeclaringType)
            {
                return baseDef;
            }
        }

        Type? declaring = method.DeclaringType;
        if (declaring is null || declaring.IsInterface)
        {
            return null;
        }

        foreach (Type iface in declaring.GetInterfaces())
        {
            InterfaceMapping map;
            try { map = declaring.GetInterfaceMap(iface); }
            catch { continue; }

            for (int i = 0; i < map.TargetMethods.Length; i++)
            {
                if (map.TargetMethods[i] == method)
                {
                    return map.InterfaceMethods[i];
                }
            }
        }

        return null;
    }

    private static PropertyInfo? FindPropertySource(PropertyInfo property)
    {
        MethodInfo? accessor = property.GetMethod ?? property.SetMethod;
        if (accessor is null)
        {
            return null;
        }
        MethodInfo? sourceAccessor = FindMethodSource(accessor);
        if (sourceAccessor?.DeclaringType is null)
        {
            return null;
        }

        foreach (PropertyInfo p in sourceAccessor.DeclaringType.GetProperties(AllInstance))
        {
            if (p.GetMethod == sourceAccessor || p.SetMethod == sourceAccessor)
            {
                return p;
            }
        }
        return null;
    }

    private static EventInfo? FindEventSource(EventInfo evt)
    {
        MethodInfo? accessor = evt.AddMethod ?? evt.RemoveMethod;
        if (accessor is null)
        {
            return null;
        }
        MethodInfo? sourceAccessor = FindMethodSource(accessor);
        if (sourceAccessor?.DeclaringType is null)
        {
            return null;
        }

        foreach (EventInfo e in sourceAccessor.DeclaringType.GetEvents(AllInstance))
        {
            if (e.AddMethod == sourceAccessor || e.RemoveMethod == sourceAccessor)
            {
                return e;
            }
        }
        return null;
    }

    private static string IdentityKey(MemberInfo member) => member switch
    {
        Type t => "T:" + (t.FullName ?? t.Name),
        _ => $"{member.MemberType}:{member.DeclaringType?.FullName}.{member.Name}#{member.MetadataToken}"
    };
}
