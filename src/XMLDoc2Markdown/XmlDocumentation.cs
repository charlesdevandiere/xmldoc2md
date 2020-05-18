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
            string fullName;
            if (memberInfo is Type type)
            {
                fullName = type.FullName;
            }
            else
            {
                string name = memberInfo is ConstructorInfo ? "#ctor" : memberInfo.Name;
                fullName = $"{memberInfo.DeclaringType.Namespace}.{memberInfo.DeclaringType.Name}.{name}";
            }
            if (memberInfo is MethodBase methodBase)
            {
                Type[] genericArguments = methodBase switch {
                    ConstructorInfo _ => methodBase.DeclaringType.GetGenericArguments(),
                    MethodInfo _ => methodBase.GetGenericArguments(),
                    _ => new Type[0]
                };

                if (methodBase is MethodInfo && methodBase.IsGenericMethod)
                {
                    fullName += $"``{genericArguments.Length}";
                }

                ParameterInfo[] parameterInfos = methodBase.GetParameters();
                if (parameterInfos.Length > 0)
                {
                    IEnumerable<string> @params = parameterInfos.Select(p =>
                    {
                        int index = Array.IndexOf(genericArguments, p.ParameterType);
                        return index > -1 ? $"{(methodBase is MethodInfo ? "``" : "`")}{index}" : p.ParameterType.ToString();
                    });
                    fullName = string.Concat(fullName, $"({string.Join(',', @params)})");
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
