using XMLDoc2Markdown.XmlDocId;

namespace XMLDoc2Markdown.Linking;

internal static class DocsFileNameProvider
{
    internal static string GetDocsFileName(this Type type, DocumentationStructure structure)
    {
        ArgumentNullException.ThrowIfNull(type);
        string name = type.GetIdentifier().ToLower().Replace('`', '-');

        return structure switch
        {
            DocumentationStructure.Tree => name.Replace('.', '/'),
            _ => name
        };
    }
}
