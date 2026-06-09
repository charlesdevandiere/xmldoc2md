namespace XMLDoc2Markdown.Signatures;

internal static class TypeHierarchy
{
    internal static IEnumerable<Type> GetInheritanceHierarchy(this Type type)
    {
        for (Type? current = type; current != null; current = current.BaseType)
        {
            yield return current;
        }
    }
}
