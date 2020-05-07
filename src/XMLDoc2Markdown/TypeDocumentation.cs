using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Markdown;

namespace XMLDoc2Markdown
{
    public class TypeDocumentation
    {
        private readonly Type type;
        private readonly XmlDocumentation documentation;
        private readonly IMarkdownDocument document = new MarkdownDocument();

        public TypeDocumentation(Type type, XmlDocumentation documentation)
        {
            this.type = type;
            this.documentation = documentation;
        }

        public override string ToString()
        {
            this.document.AppendHeader(this.type.Name, 1);

            this.document.AppendParagraph($"Namespace: {this.type.Namespace}");

            this.WriteMemberInfoDocumentation(this.type);

            if (this.type.BaseType != null)
            {
                this.document.AppendParagraph($"Inheritance {string.Join(" → ", this.type.GetInheritanceHierarchy().Reverse().Select(t => t.Name))}");
            }

            Type[] interfaces = this.type.GetInterfaces();
            if (interfaces.Length > 0)
            {
                this.document.AppendParagraph($"Implements {string.Join(", ", interfaces.Select(i => i.Name))}");
            }

            this.document.AppendHeader("Properties", 2);
            foreach (PropertyInfo property in this.type.GetProperties())
            {
                this.WriteMemberInfoDocumentation(property);
            }

            this.WriteMethodBasesDocumentation(this.type.GetConstructors());
            this.WriteMethodBasesDocumentation(
                this.type
                    .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)
                    .Where(m => !m.IsSpecialName)
                    .Where(m => !m.IsPrivate)
                );

            return this.document.ToString();
        }

        private void WriteMemberInfoDocumentation(MemberInfo memberInfo)
        {
            XElement doc = this.documentation.GetMember(memberInfo);
            string summary = doc?.Element("summary")?.Value;
            this.document.AppendParagraph(summary ?? "");

            this.document.AppendCode(
                "csharp",
                memberInfo.GetSignature(full: true)
            );
        }

        private void WriteMethodBasesDocumentation(IEnumerable<MethodBase> methodBases)
        {
            if (methodBases.Count() <= 0)
            {
                return;
            }

            string title = methodBases.First().MemberType == MemberTypes.Constructor ? "Constructors" : "Methods";
            this.document.AppendHeader(title, 2);

            foreach (MethodBase methodBase in methodBases)
            {
                this.document.AppendHeader(methodBase.GetSignature(), 3);

                this.WriteMemberInfoDocumentation(methodBase);

                XElement methodDoc = this.documentation.GetMember(methodBase);

                ParameterInfo[] @params = methodBase.GetParameters();

                if (@params.Length > 0)
                {
                    this.document.AppendHeader("Parameters", 4);

                    foreach (ParameterInfo param in @params)
                    {
                        string paramDoc = methodDoc?.Elements("param").FirstOrDefault(e => e.Attribute("name")?.Value == param.Name)?.Value;
                        this.document.AppendParagraph(
                            $"{new MarkdownInlineCode(param.Name)} {param.ParameterType.Name}<br>{paramDoc}");
                    }
                }

                if (methodBase is MethodInfo methodInfo && methodInfo.ReturnType != typeof(void))
                {
                    this.document.AppendHeader("Returns", 4);
                    string returnsDoc = methodDoc?.Element("returns")?.Value;
                    this.document.AppendParagraph(
                        $"{methodInfo.ReturnType.Name}<br>{returnsDoc}");
                }

                IEnumerable<XElement> exceptionDocs = methodDoc?.Elements("exception");

                if (exceptionDocs?.Count() > 0)
                {
                    this.document.AppendHeader("Exceptions", 4);

                    foreach (XElement exceptionDoc in exceptionDocs)
                    {
                        var text = new List<string>(2);

                        string cref = exceptionDoc.Attribute("cref")?.Value;
                        if (cref != null && cref.Length > 2)
                        {
                            int index = cref.LastIndexOf('.');
                            string exception = cref.Substring(index + 1);
                            if (!string.IsNullOrEmpty(exception))
                            {
                                text.Add(exception);
                            }
                        }

                        if (!string.IsNullOrEmpty(exceptionDoc.Value))
                        {
                            text.Add(exceptionDoc.Value);
                        }

                        if (text.Count() > 0)
                        {
                            this.document.AppendParagraph(string.Join("<br>", text));
                        }
                    }
                }
            }
        }
    }
}
