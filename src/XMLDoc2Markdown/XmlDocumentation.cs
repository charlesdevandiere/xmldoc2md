using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using XMLDoc2Markdown.Utils;

namespace XMLDoc2Markdown;

public class XmlDocumentation
{
    public string AssemblyName { get; }
    public IEnumerable<XElement> Members { get; }

    public XmlDocumentation(string dllPath)
    {
        string xmlPath = Path.Combine(Directory.GetParent(dllPath).FullName, Path.GetFileNameWithoutExtension(dllPath) + ".xml");

        if (!File.Exists(xmlPath))
        {
            throw new FileNotFoundException($"Could not load XML documentation file '{Path.GetFullPath(xmlPath)}'. File not found.", xmlPath);
        }

        try
        {
            XDocument xDocument = XDocument.Parse(File.ReadAllText(xmlPath));

            this.AssemblyName = xDocument.Descendants("assembly").First().Elements("name").First().Value;
            this.Members = xDocument.Descendants("members").First().Elements("member");
        }
        catch (Exception e)
        {
            throw new Exception("Unable to parse XML documentation", e);
        }
    }

    public XElement GetMember(MemberInfo memberInfo)
    {
        return this.GetMember($"{memberInfo.MemberType.GetAlias()}:{memberInfo.GetIdentifier()}");
    }

    public XElement GetMember(string name)
    {
        return this.Members.FirstOrDefault(member => member.Attribute("name").Value == name);
    }
}
