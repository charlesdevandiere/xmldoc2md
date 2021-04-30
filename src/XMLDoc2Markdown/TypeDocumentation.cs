using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Dawn;
using Markdown;
using XMLDoc2Markdown.Utils;

namespace XMLDoc2Markdown
{
    public class TypeDocumentation
    {
        private readonly Assembly assembly;
        private readonly Type type;
        private readonly XmlDocumentation documentation;
        private readonly TypeDocumentationOptions options;
        private readonly IMarkdownDocument document = new MarkdownDocument();

        public TypeDocumentation(Assembly assembly, Type type, XmlDocumentation documentation, TypeDocumentationOptions options = null)
        {
            Guard.Argument(assembly, nameof(assembly)).NotNull();
            Guard.Argument(type, nameof(type)).NotNull();
            Guard.Argument(documentation, nameof(documentation)).NotNull();

            this.assembly = assembly;
            this.type = type;
            this.documentation = documentation;
            this.options = options ?? new TypeDocumentationOptions();
        }

        public override string ToString()
        {
            if (this.options.BackButton)
            {
                this.WriteBackButton(top: true);
            }

            this.document.AppendHeader(this.type.GetDisplayName().FormatChevrons(), 1);

            this.document.AppendParagraph($"Namespace: {this.type.Namespace}");

            XElement typeDocElement = this.documentation.GetMember(this.type);

            if (typeDocElement != null)
            {
                Logger.Info("    (documented)");
            }

            this.WriteMemberInfoSummary(typeDocElement);
            this.WriteMemberInfoSignature(this.type);
            this.WriteTypeParameters(this.type, typeDocElement);
            this.WriteInheritanceAndImplements();

            if (this.type.IsEnum)
            {
                this.WriteEnumFields(this.type.GetFields().Where(m => !m.IsSpecialName));
            }
            else
            {
                this.WriteMembersDocumentation(this.type.GetFields());
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

            bool example = this.WriteExample(this.type);
            if (example)
            {
                Logger.Info("    (example)");
            }

            if (this.options.BackButton)
            {
                this.WriteBackButton(bottom: true);
            }

            return this.document.ToString();
        }

        private void WriteBackButton(bool top = false, bool bottom = false)
        {
            if (top && bottom)
            {
                throw new ArgumentException("Back button cannot not be set to 'top' and 'bottom' at the same time.");
            }

            if (bottom)
            {
                this.document.AppendHorizontalRule();
            }

            this.document.AppendParagraph(new MarkdownLink(new MarkdownInlineCode("< Back"), "./"));

            if (top)
            {
                this.document.AppendHorizontalRule();
            }
        }

        private void WriteInheritanceAndImplements()
        {
            var lignes = new List<string>();

            if (this.type.BaseType != null)
            {
                IEnumerable<MarkdownInlineElement> inheritanceHierarchy = this.type.GetInheritanceHierarchy()
                    .Reverse()
                    .Select(t => t.GetDocsLink(this.assembly, noExtension: this.options.GitHubPages));
                lignes.Add($"Inheritance {string.Join(" → ", inheritanceHierarchy)}");
            }

            Type[] interfaces = this.type.GetInterfaces();
            if (interfaces.Length > 0)
            {
                IEnumerable<MarkdownInlineElement> implements = interfaces
                    .Select(i => i.GetDocsLink(this.assembly, noExtension: this.options.GitHubPages));
                lignes.Add($"Implements {string.Join(", ", implements)}");
            }

            if (lignes.Any())
            {
                this.document.AppendParagraph(string.Join($"<br>{Environment.NewLine}", lignes));
            }
        }

        private void WriteMemberInfoSummary(XElement memberDocElement)
        {
            string summary = string.Empty;
            IEnumerable<XNode> nodes = memberDocElement?.Element("summary")?.Nodes();
            if (nodes != null)
            {
                foreach (XNode node in nodes)
                {
                    summary += node switch
                    {
                        XText text => Regex.Replace(text.ToString(), "[ ]{2,}", " "),
                        XElement element => this.PrintSummaryXElement(element),
                        _ => null
                    };
                }
            }
            this.document.AppendParagraph(summary);
        }

        private string PrintSummaryXElement(XElement element)
        {
            return element.Name.ToString() switch
            {
                "see" => this.GetLinkFromReference(element.Attribute("cref")?.Value).ToString(),
                _ => element.Value
            };
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

            if (!members.Any())
            {
                return;
            }

            MemberTypes memberType = members.First().MemberType;
            string title = memberType switch
            {
                MemberTypes.Property => "Properties",
                MemberTypes.Constructor => "Constructors",
                MemberTypes.Method => "Methods",
                MemberTypes.Event => "Events",
                MemberTypes.Field => "Fields",
                _ => throw new NotImplementedException()
            };
            this.document.AppendHeader(title, 2);
            Logger.Info($"    {title}");

            foreach (MemberInfo member in members)
            {
                this.document.AppendHeader(new MarkdownStrongEmphasis(member.GetSignature().FormatChevrons()), 3);

                XElement memberDocElement = this.documentation.GetMember(member);

                this.WriteMemberInfoSummary(memberDocElement);
                this.WriteMemberInfoSignature(member);

                if (member is MethodBase methodBase)
                {
                    this.WriteTypeParameters(methodBase, memberDocElement);
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
                    MarkdownInlineElement typeName = propertyInfo.GetReturnType()?
                        .GetDocsLink(this.assembly, noExtension: this.options.GitHubPages);
                    this.document.AppendParagraph($"{typeName}<br>{Environment.NewLine}{valueDoc}");
                }

                this.WriteExceptions(memberDocElement);

                bool example = this.WriteExample(member);

                string log = $"      {member.GetIdentifier()}";
                if (memberDocElement != null)
                {
                    log += " (documented)";
                }
                if (example)
                {
                    log += " (example)";
                }

                Logger.Info(log);
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
                    string cref = exceptionDoc.Attribute("cref")?.Value;
                    MarkdownInlineElement exceptionTypeName = this.GetLinkFromReference(cref);
                    this.document.AppendParagraph(string.Join($"<br>{Environment.NewLine}", exceptionTypeName, exceptionDoc.Value));
                }
            }
        }

        private void WriteMethodReturnType(MethodInfo methodInfo, XElement memberDocElement)
        {
            Guard.Argument(methodInfo, nameof(methodInfo)).NotNull();

            this.document.AppendHeader("Returns", 4);

            string returnsDoc = memberDocElement?.Element("returns")?.Value;
            MarkdownInlineElement typeName = methodInfo.ReturnType.GetDocsLink(this.assembly, noExtension: this.options.GitHubPages);
            this.document.AppendParagraph(string.Join($"<br>{Environment.NewLine}", typeName, returnsDoc));
        }

        private void WriteTypeParameters(MemberInfo memberInfo, XElement memberDocElement)
        {
            Guard.Argument(memberInfo, nameof(memberInfo)).NotNull();

            Type[] typeParams = memberInfo switch
            {
                TypeInfo typeInfo => typeInfo.GenericTypeParameters,
                MethodInfo methodInfo => methodInfo.GetGenericArguments(),
                _ => Array.Empty<Type>()
            };

            if (typeParams.Length > 0)
            {
                this.document.AppendHeader("Type Parameters", 4);

                foreach (Type typeParam in typeParams)
                {
                    string typeParamDoc = memberDocElement?.Elements("typeparam").FirstOrDefault(e => e.Attribute("name")?.Value == typeParam.Name)?.Value;
                    MarkdownInlineElement typeName = typeParam.GetDocsLink(this.assembly, noExtension: this.options.GitHubPages);
                    this.document.AppendParagraph(string.Join($"<br>{Environment.NewLine}", new MarkdownInlineCode(typeName), typeParamDoc));
                }
            }
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
                    MarkdownInlineElement typeName = param.ParameterType.GetDocsLink(this.assembly, noExtension: this.options.GitHubPages);
                    this.document.AppendParagraph($"{new MarkdownInlineCode(param.Name)} {typeName}<br>{Environment.NewLine}{paramDoc}");
                }
            }
        }

        private void WriteEnumFields(IEnumerable<FieldInfo> fields)
        {
            Guard.Argument(fields, nameof(fields)).NotNull();

            if (fields.Any())
            {
                this.document.AppendHeader("Fields", 2);

                var header = new MarkdownTableHeader(
                    new MarkdownTableHeaderCell("Name"),
                    new MarkdownTableHeaderCell("Value", MarkdownTableTextAlignment.Right),
                    new MarkdownTableHeaderCell("Description")
                );

                var table = new MarkdownTable(header, fields.Count());

                foreach (FieldInfo field in fields)
                {
                    string paramDoc = this.documentation.GetMember(field)?.Element("summary")?.Value;
                    table.AddRow(new MarkdownTableRow(field.Name, ((Enum)Enum.Parse(this.type, field.Name)).ToString("D"), paramDoc.Trim()));
                }

                this.document.Append(table);
            }
        }

        private bool WriteExample(MemberInfo memberInfo)
        {
            if (this.options.ExamplesDirectory == null)
            {
                return false;
            }

            string fileName = $"{memberInfo.GetIdentifier()}.md";
            string file = Path.Combine(this.options.ExamplesDirectory, fileName);

            if (File.Exists(file))
            {
                try
                {
                    using var sr = new StreamReader(file);
                    this.document.Append(new MarkdownParagraph(sr.ReadToEnd()));

                    return true;
                }
                catch (IOException e)
                {
                    Logger.Warning(e.Message);
                }
            }

            return false;
        }

        private MarkdownInlineElement GetLinkFromReference(string crefAttribute)
        {
            if (crefAttribute != null &&
                crefAttribute.Length > 2 &&
                crefAttribute[1] == ':' &&
                MemberTypesAliases.TryGetMemberType(crefAttribute[0], out MemberTypes memberType))
            {
                string memberFullName = crefAttribute[2..];

                if (memberType == MemberTypes.TypeInfo)
                {
                    Type type = Type.GetType(memberFullName) ?? this.assembly.GetType(memberFullName);
                    if (type != null)
                    {
                        return type.GetDocsLink(this.assembly, noExtension: this.options.GitHubPages);
                    }
                }

                return new MarkdownText(memberFullName);
            }

            return new MarkdownText(crefAttribute);
        }
    }
}
