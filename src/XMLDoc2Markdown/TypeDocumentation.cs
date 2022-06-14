using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Markdown;
using XMLDoc2Markdown.Utils;

namespace XMLDoc2Markdown;

public class TypeDocumentation
{
    private readonly Assembly assembly;
    private readonly Type type;
    private readonly XmlDocumentation documentation;
    private readonly TypeDocumentationOptions options;
    private readonly IMarkdownDocument document = new MarkdownDocument();

    public TypeDocumentation(Assembly assembly, Type type, XmlDocumentation documentation, TypeDocumentationOptions options = null)
    {
        RequiredArgument.NotNull(assembly, nameof(assembly));
        RequiredArgument.NotNull(type, nameof(type));
        RequiredArgument.NotNull(documentation, nameof(documentation));

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
        this.WriteMemberInfoRemarks(typeDocElement);

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
        List<string> lignes = new();

        if (this.type.BaseType != null)
        {
            IEnumerable<MarkdownInlineElement> inheritanceHierarchy = this.type.GetInheritanceHierarchy()
                .Reverse()
                .Select(t => t.GetDocsLink(
                    this.assembly,
                    noExtension: this.options.GitHubPages || this.options.GitlabWiki,
                    noPrefix: this.options.GitlabWiki));
            lignes.Add($"Inheritance {string.Join(" → ", inheritanceHierarchy)}");
        }

        Type[] interfaces = this.type.GetInterfaces();
        if (interfaces.Length > 0)
        {
            IEnumerable<MarkdownInlineElement> implements = interfaces
                .Select(i => i.GetDocsLink(
                    this.assembly,
                    noExtension: this.options.GitHubPages || this.options.GitlabWiki,
                    noPrefix: this.options.GitlabWiki));
            lignes.Add($"Implements {string.Join(", ", implements)}");
        }

        if (lignes.Any())
        {
            this.document.AppendParagraph(string.Join($"<br>{Environment.NewLine}", lignes));
        }
    }

    private void WriteMemberInfoSummary(XElement memberDocElement)
    {
        IEnumerable<XNode> nodes = memberDocElement?.Element("summary")?.Nodes();
        if (nodes != null)
        {
            MarkdownParagraph summary = this.XNodesToMarkdownParagraph(nodes);
            this.document.Append(summary);
        }
    }

    private void WriteMemberInfoRemarks(XElement memberDocElement)
    {
        IEnumerable<XNode> nodes = memberDocElement?.Element("remarks")?.Nodes();
        if (nodes != null)
        {
            this.document.AppendParagraph(new MarkdownStrongEmphasis("Remarks:"));
            this.document.Append(this.XNodesToMarkdownParagraph(nodes));
        }
    }

    private MarkdownTextElement XElementToMarkdown(XElement element)
    {
        return element.Name.ToString() switch
        {
            "see" => this.GetLinkFromReference(element.Attribute("cref")?.Value ?? element.Attribute("href")?.Value, element.Value),
            "seealso" => this.GetLinkFromReference(element.Attribute("cref")?.Value, element.Value),
            "c" => new MarkdownInlineCode(element.Value),
            "br" => new MarkdownText("<br>"),
            "para" => this.XNodesToMarkdownParagraph(element.Nodes()),
            "example" => this.XNodesToMarkdownParagraph(element.Nodes()),
            "code" => new MarkdownCode("csharp", TypeDocumentation.FormatCodeElementValue(element.Value)),
            _ => new MarkdownText(element.Value)
        };
    }

    private static string FormatCodeElementValue(string code)
    {
        if (string.IsNullOrEmpty(code))
        {
            return code;
        }

        code = code.TrimStart('\n');
        code = code.TrimEnd('\n', ' ');

        static int getIndent(string value)
        {
            int indent = 0;
            foreach (char @char in value)
            {
                if (@char == ' ')
                {
                    indent++;
                }
                else
                {
                    break;
                }
            }
            return indent;
        }
        static string reindentLine(string line, int indent)
        {
            string result = string.Empty;
            int i;
            for (i = 0; i < indent; i++)
            {
                if (line[i] != ' ')
                {
                    break;
                }
            }
            return line[i..];
        }
        int indent = getIndent(code);

        IEnumerable<string> lines = code
            .Split('\n')
            .Select(line => reindentLine(line, indent));

        return string.Join(Environment.NewLine, lines);
    }

    private MarkdownParagraph XNodesToMarkdownParagraph(IEnumerable<XNode> nodes)
    {
        List<IMarkdownBlockElement> blocks = new();
        MarkdownText paragraph = null;
        foreach (XNode node in nodes)
        {
            MarkdownTextElement element = this.XNodeToMarkdown(node);
            if (element is not null)
            {
                switch (element)
                {
                    case MarkdownInlineElement inlineElement:
                        if (paragraph is null)
                        {
                            paragraph = new MarkdownText(inlineElement);
                        }
                        else
                        {
                            paragraph.Append(inlineElement);
                        }
                        break;
                    case IMarkdownBlockElement block:
                        if (paragraph is not null)
                        {
                            blocks.Add(new MarkdownParagraph(paragraph));
                            paragraph = null;
                        }
                        blocks.Add(block);
                        break;
                }
            }
        }

        if (paragraph is not null)
        {
            blocks.Add(new MarkdownParagraph(paragraph));
        }

        return new MarkdownParagraph(string.Join(Environment.NewLine, blocks));
    }

    private MarkdownTextElement XNodeToMarkdown(XNode node)
    {
        return node switch
        {
            XText text => new MarkdownText(Regex.Replace(text.ToString(), "[ ]{2,}", " ")),
            XElement element => this.XElementToMarkdown(element),
            _ => null
        };
    }

    private void WriteMemberInfoSignature(MemberInfo memberInfo)
    {
        this.document.AppendCode(
            "csharp",
            memberInfo.GetSignature(full: true));
    }

    private void WriteMembersDocumentation(IEnumerable<MemberInfo> members)
    {
        RequiredArgument.NotNull(members, nameof(members));

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
                    .GetDocsLink(
                        this.assembly,
                        noExtension: this.options.GitHubPages || this.options.GitlabWiki,
                        noPrefix: this.options.GitlabWiki);
                this.document.AppendParagraph($"{typeName}<br>{Environment.NewLine}{valueDoc}");
            }

            this.WriteExceptions(memberDocElement);
            this.WriteMemberInfoRemarks(memberDocElement);

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
        RequiredArgument.NotNull(methodInfo, nameof(methodInfo));

        this.document.AppendHeader("Returns", 4);

        string returnsDoc = memberDocElement?.Element("returns")?.Value;
        MarkdownInlineElement typeName = methodInfo.ReturnType.GetDocsLink(
            this.assembly,
            noExtension: this.options.GitHubPages || this.options.GitlabWiki,
            noPrefix: this.options.GitlabWiki);
        this.document.AppendParagraph(string.Join($"<br>{Environment.NewLine}", typeName, returnsDoc));
    }

    private void WriteTypeParameters(MemberInfo memberInfo, XElement memberDocElement)
    {
        RequiredArgument.NotNull(memberInfo, nameof(memberInfo));

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
                MarkdownInlineElement typeName = typeParam.GetDocsLink(
                    this.assembly,
                    noExtension: this.options.GitHubPages || this.options.GitlabWiki,
                    noPrefix: this.options.GitlabWiki);
                this.document.AppendParagraph(string.Join($"<br>{Environment.NewLine}", new MarkdownInlineCode(typeName), typeParamDoc));
            }
        }
    }

    private void WriteMethodParams(MethodBase methodBase, XElement memberDocElement)
    {
        RequiredArgument.NotNull(methodBase, nameof(methodBase));

        ParameterInfo[] @params = methodBase.GetParameters();

        if (@params.Length > 0)
        {
            this.document.AppendHeader("Parameters", 4);

            foreach (ParameterInfo param in @params)
            {
                string paramDoc = memberDocElement?.Elements("param").FirstOrDefault(e => e.Attribute("name")?.Value == param.Name)?.Value;
                MarkdownInlineElement typeName = param.ParameterType.GetDocsLink(
                    this.assembly,
                    noExtension: this.options.GitHubPages || this.options.GitlabWiki,
                    noPrefix: this.options.GitlabWiki);
                this.document.AppendParagraph($"{new MarkdownInlineCode(param.Name)} {typeName}<br>{Environment.NewLine}{paramDoc}");
            }
        }
    }

    private void WriteEnumFields(IEnumerable<FieldInfo> fields)
    {
        RequiredArgument.NotNull(fields, nameof(fields));

        if (fields.Any())
        {
            this.document.AppendHeader("Fields", 2);

            MarkdownTableHeader header = new(
                new MarkdownTableHeaderCell("Name"),
                new MarkdownTableHeaderCell("Value", MarkdownTableTextAlignment.Right),
                new MarkdownTableHeaderCell("Description")
            );

            MarkdownTable table = new(header, fields.Count());

            foreach (FieldInfo field in fields)
            {
                string paramDoc = this.documentation.GetMember(field)?.Element("summary")?.Value ?? String.Empty;
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
                using StreamReader reader = new(file);
                this.document.Append(new MarkdownParagraph(reader.ReadToEnd()));

                return true;
            }
            catch (IOException e)
            {
                Logger.Warning(e.Message);
            }
        }

        return false;
    }

    private MarkdownInlineElement GetLinkFromReference(string crefAttribute, string text = null)
    {
        if (this.TryGetMemberInfoFromReference(crefAttribute, out MemberInfo memberInfo))
        {
            return memberInfo.GetDocsLink(
                this.assembly,
                text: text,
                noExtension: this.options.GitHubPages || this.options.GitlabWiki,
                noPrefix: this.options.GitlabWiki);
        }

        return new MarkdownText(text ?? crefAttribute);
    }

    private bool TryGetMemberInfoFromReference(string crefAttribute, out MemberInfo memberInfo)
    {
        memberInfo = null;

        if (crefAttribute != null &&
            crefAttribute.Length > 2 &&
            crefAttribute[1] == ':' &&
            MemberTypesAliases.TryGetMemberType(crefAttribute[0], out MemberTypes memberType))
        {
            string memberFullName = crefAttribute[2..];

            if (memberType is MemberTypes.Constructor or MemberTypes.Method)
            {
                int parenthesisIndex = memberFullName.IndexOf('(');
                if (parenthesisIndex > -1)
                {
                    string memberFullNameWithoutParenthesis = memberFullName[..parenthesisIndex];
                    int lastDotIndex = memberFullNameWithoutParenthesis.LastIndexOf(".");
                    Type type = this.GetTypeFromFullName(memberFullNameWithoutParenthesis[..lastDotIndex]);
                    if (type is not null)
                    {
                        string methodSignature = memberFullName[(lastDotIndex + 1)..parenthesisIndex].Replace('#', '.');
                        List<string> methodArgs = new(memberFullName[(parenthesisIndex + 1)..].Split(","));
                        string lastArg = methodArgs[^1];
                        methodArgs[^1] = lastArg[..^1];
                        MemberInfo[] members = type.GetMember($"{methodSignature}*");
                        memberInfo =
                            members.FirstOrDefault(info => ((MethodBase)info).GetParameters().Length == methodArgs.Count) ??
                            members.FirstOrDefault();
                    }
                }
            }
            else if (memberType is MemberTypes.Event or MemberTypes.Field or MemberTypes.Property)
            {
                int idx = memberFullName.LastIndexOf(".");
                Type type = this.GetTypeFromFullName(memberFullName[..idx]);
                if (type is not null)
                {
                    memberInfo = type.GetMember(memberFullName[(idx + 1)..]).FirstOrDefault();
                }
            }
            else if (memberType is MemberTypes.TypeInfo or MemberTypes.NestedType)
            {
                Type type = this.GetTypeFromFullName(memberFullName);
                if (type is not null)
                {
                    memberInfo = type;
                }
            }
        }

        return memberInfo != null;
    }

    private Type GetTypeFromFullName(string typeFullName)
    {
        return Type.GetType(typeFullName) ?? this.assembly.GetType(typeFullName);
    }
}
