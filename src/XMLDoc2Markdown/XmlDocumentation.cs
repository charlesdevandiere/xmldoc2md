using System.Reflection;
using System.Xml.Linq;
using XMLDoc2Markdown.Utils;

namespace XMLDoc2Markdown;

internal class XmlDocumentation
{
    internal string AssemblyName { get; }
    internal IReadOnlyDictionary<string, XElement> Members { get; }

    internal XmlDocumentation(string dllPath)
    {
        string xmlPath = Path.Combine(
            Path.GetDirectoryName(dllPath) ?? string.Empty,
            Path.GetFileNameWithoutExtension(dllPath) + ".xml");

        if (!File.Exists(xmlPath))
        {
            throw new FileNotFoundException($"Could not load XML documentation file '{Path.GetFullPath(xmlPath)}'. File not found.", xmlPath);
        }

        try
        {
            XDocument xDocument = XDocument.Load(xmlPath);

            this.AssemblyName = xDocument.Descendants("assembly").First().Elements("name").First().Value;

            Dictionary<string, XElement> members = new(StringComparer.Ordinal);
            foreach (XElement member in xDocument.Descendants("members").First().Elements("member"))
            {
                string? name = member.Attribute("name")?.Value;
                if (name is not null)
                {
                    members[name] = member;
                }
            }
            this.Members = members;
        }
        catch (Exception e)
        {
            throw new Exception("Unable to parse XML documentation", e);
        }
    }

    internal XElement? GetMember(MemberInfo memberInfo)
    {
        return this.GetMember($"{memberInfo.MemberType.GetAlias()}:{memberInfo.GetIdentifier()}");
    }

    internal XElement? GetMember(string name)
    {
        return this.Members.TryGetValue(name, out XElement? element) ? element : null;
    }
}
