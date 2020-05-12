using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Dawn;
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
            this.document.AppendHeader(this.type.GetDisplayName().Replace("<", "&lt;").Replace("g", "&gt;"), 1);

            this.document.AppendParagraph($"Namespace: {this.type.Namespace}");

            this.WriteMemberInfoSummary(this.type);
            this.WriteMemberInfoSignature(this.type);

            if (this.type.BaseType != null)
            {
                this.document.AppendParagraph($"Inheritance {string.Join(" → ", this.type.GetInheritanceHierarchy().Reverse().Select(t => t.GetDisplayName().Replace("<", "&lt;").Replace("g", "&gt;")))}");
            }

            Type[] interfaces = this.type.GetInterfaces();
            if (interfaces.Length > 0)
            {
                this.document.AppendParagraph($"Implements {string.Join(", ", interfaces.Select(i => i.GetDisplayName().Replace("<", "&lt;").Replace("g", "&gt;")))}");
            }

            this.WriteMembersDocumentation(this.type.GetProperties());
            this.WriteMembersDocumentation(this.type.GetConstructors());
            this.WriteMembersDocumentation(
                this.type
                    .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)
                    .Where(m => !m.IsSpecialName)
                    .Where(m => !m.IsPrivate)
                );
            this.WriteMembersDocumentation(this.type.GetEvents());

            return this.document.ToString();
        }

        private void WriteMemberInfoSummary(MemberInfo memberInfo)
        {
            XElement doc = this.documentation.GetMember(memberInfo);
            string summary = doc?.Element("summary")?.Value;
            this.document.AppendParagraph(summary ?? "");
        }

        private void WriteMemberInfoSignature(MemberInfo memberInfo)
        {
            this.document.AppendCode(
                "csharp",
                memberInfo.GetSignature(full: true)
            );
        }

        private void WriteMembersDocumentation(IEnumerable<MemberInfo> members)
        {
            Guard.Argument(members, nameof(members)).NotNull();

            members = members.Where(member => member != null);

            if (members.Count() <= 0)
            {
                return;
            }

            string title = members.First().MemberType switch
            {
                MemberTypes.Property => "Properties",
                MemberTypes.Constructor => "Constructors",
                MemberTypes.Method => "Methods",
                MemberTypes.Event => "Events",
                _ => throw new NotImplementedException()
            };
            this.document.AppendHeader(title, 2);

            foreach (MemberInfo member in members)
            {
                this.document.AppendHeader(member.GetSignature().Replace("<", "&lt;").Replace("g", "&gt;"), 3);

                this.WriteMemberInfoSummary(member);
                this.WriteMemberInfoSignature(member);

                XElement memberDocElement = this.documentation.GetMember(member);

                if (member is MethodBase methodBase)
                {
                    this.WriteMethodParams(methodBase, memberDocElement);

                    if (methodBase is MethodInfo methodInfo && methodInfo.ReturnType != typeof(void))
                    {
                        this.WriteMethodReturnType(methodInfo, memberDocElement);
                    }
                }

                if (member is PropertyInfo propertyInfo)
                {
                    this.document.AppendHeader("Property Value", 4);

                    string valueDoc = memberDocElement?.Element("value")?.Value;
                    this.document.AppendParagraph(
                        $"{propertyInfo.GetReturnType()?.Name}<br>{valueDoc}");
                }

                this.WriteExceptions(memberDocElement);
            }
        }

        private void WriteExceptions(XElement memberDocElement)
        {
            IEnumerable<XElement> exceptionDocs = memberDocElement?.Elements("exception");

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

        private void WriteMethodReturnType(MethodInfo methodInfo, XElement memberDocElement)
        {
            Guard.Argument(methodInfo, nameof(methodInfo)).NotNull();

            this.document.AppendHeader("Returns", 4);

            string returnsDoc = memberDocElement?.Element("returns")?.Value;
            this.document.AppendParagraph(
                $"{methodInfo.ReturnType.Name}<br>{returnsDoc}");
        }

        private void WriteMethodParams(MethodBase methodBase, XElement memberDocElement)
        {
            Guard.Argument(methodBase, nameof(methodBase)).NotNull();

            ParameterInfo[] @params = methodBase.GetParameters();

            if (@params.Length > 0)
            {
                this.document.AppendHeader("Parameters", 4);

                foreach (ParameterInfo param in @params)
                {
                    string paramDoc = memberDocElement?.Elements("param").FirstOrDefault(e => e.Attribute("name")?.Value == param.Name)?.Value;
                    this.document.AppendParagraph(
                        $"{new MarkdownInlineCode(param.Name)} {param.ParameterType.Name}<br>{paramDoc}");
                }
            }
        }
    }
}
