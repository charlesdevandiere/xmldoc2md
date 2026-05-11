namespace XMLDoc2Markdown.Signatures.Modifiers;

internal static class BaseListModifier
{
    internal static SignatureBuilder AppendBaseList(this SignatureBuilder b, Type type)
    {
        if (!type.IsClass && !type.IsInterface)
        {
            return b;
        }

        List<Type> bases = [];
        if (type.IsClass && type.BaseType != null && type.BaseType != typeof(object))
        {
            bases.Add(type.BaseType);
        }
        bases.AddRange(type.GetInterfaces());

        if (bases.Count == 0)
        {
            return b;
        }

        IEnumerable<string> names = bases.Select(t =>
            t.Namespace != type.Namespace ? t.FullName ?? t.Name : t.Name);

        return b.Append($": {string.Join(", ", names)}");
    }
}
