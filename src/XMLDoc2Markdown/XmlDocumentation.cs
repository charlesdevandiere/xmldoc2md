using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace XMLDoc2Markdown
{
    public class XmlDocumentation
    {
        public string AssemblyName { get; }
        public IEnumerable<XElement> Members { get; }

        public XmlDocumentation(string dllPath)
        {
            string xmlPath = Path.Combine(Directory.GetParent(dllPath).FullName, Path.GetFileNameWithoutExtension(dllPath) + ".xml");

            if (!File.Exists(xmlPath))
            {
                throw new Exception($"Not XML documentation file founded for library {dllPath}.");
            }

            try
            {
                var xDocument = XDocument.Parse(File.ReadAllText(xmlPath));

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
            string name = memberInfo.Name == ".ctor" ? "#ctor" : memberInfo.Name;
            string fullName = $"{memberInfo.DeclaringType.Namespace}.{memberInfo.DeclaringType.Name}.{name}";
            if (memberInfo is MethodBase methodBase)
            {
                ParameterInfo[] parameterInfos = methodBase.GetParameters();
                if (parameterInfos.Length > 0)
                {
                    fullName = string.Concat(fullName, $"({string.Join(',', parameterInfos.Select(p => p.ParameterType))})");
                }
            }

            return this.GetMember($"{memberInfo.MemberType.GetAlias()}:{fullName}");
        }

        public XElement GetMember(string name)
        {
            return this.Members.FirstOrDefault(member => member.Attribute("name").Value == name);
        }
    }
}
