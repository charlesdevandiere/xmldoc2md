using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Markdown;
using XMLDoc2Markdown.Utils;

namespace XMLDoc2Markdown;

public class TypeDocumentation
{
    private const string BackingFieldName = ">k__BackingField";

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

        this.WriteObsolete();
        this.WriteMemberInfoSummary(typeDocElement);
        this.WriteMemberInfoSignature(this.type);
        this.WriteTypeParameters(this.type, typeDocElement);
        this.WriteInheritanceAndImplements();
        this.WriteMemberInfoRemarks(typeDocElement);

        if (this.type.IsEnum)
        {
            this.WriteEnumFields(this.GetFields().Where(m => !m.IsSpecialName));
        }
        else
        {
            this.WriteMembersDocumentation(this.GetFields());
        }

        this.WriteMembersDocumentation(this.GetProperties());
        this.WriteMembersDocumentation(this.GetConstructors());
        this.WriteMembersDocumentation(
            this.type
                .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)
                .Where(m => !m.IsSpecialName)
                .Where(m => !m.IsPrivate || this.options.IncludePrivateMembers)
            );
        this.WriteMembersDocumentation(this.GetEvents());

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
        List<string> lines = new();

        if (this.type.BaseType != null)
        {
            IEnumerable<MarkdownInlineElement> inheritanceHierarchy = this.type.GetInheritanceHierarchy()
                .Reverse()
                .Select(t => t.GetDocsLink(
                    this.assembly,
                    noExtension: this.options.GitHubPages || this.options.GitlabWiki,
                    noPrefix: this.options.GitlabWiki));
            lines.Add($"Inheritance {string.Join(" → ", inheritanceHierarchy)}");
        }

        Type[] interfaces = this.type.GetInterfaces();
        if (interfaces.Length > 0)
        {
            IEnumerable<MarkdownInlineElement> implements = interfaces
                .Select(i => i.GetDocsLink(
                    this.assembly,
                    noExtension: this.options.GitHubPages || this.options.GitlabWiki,
                    noPrefix: this.options.GitlabWiki));
            lines.Add($"Implements {string.Join(", ", implements)}");
        }

        if (lines.Any())
        {
            this.document.AppendParagraph(string.Join($"<br>{Environment.NewLine}", lines));
        }
    }

    private void WriteObsolete()
    {
        IEnumerable<ObsoleteAttribute> attribute = this.type.GetCustomAttributes<ObsoleteAttribute>();
        WriteObsolete(attribute, this.document, "This type is obsolete.");
    }

    private void WriteObsoleteMember(MemberInfo member)
    {
        IEnumerable<ObsoleteAttribute> attribute = member.GetCustomAttributes<ObsoleteAttribute>();
        WriteObsolete(attribute, this.document, "This member is obsolete.");
    }

    private static void WriteObsolete(IEnumerable<ObsoleteAttribute> attribute, IMarkdownDocument document, string defaultMessage)
    {
        if (attribute.Any())
        {
            document.AppendHeader("Caution", 4);

            string message = attribute.First().Message;
            if (string.IsNullOrEmpty(message))
            {
                document.AppendParagraph(defaultMessage);
            }
            else
            {
                document.AppendParagraph(message);
            }
            document.AppendHorizontalRule();
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
        if (nodes is null)
        {
            return new MarkdownParagraph("");
        }

        foreach (XNode node in nodes)
        {
            MarkdownTextElement element = this.XNodeToMarkdown(node);
            if (element is null)
            {
                continue;
            }
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

            this.WriteObsoleteMember(member);
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

                MarkdownInlineElement typeName = propertyInfo.GetReturnType()?
                    .GetDocsLink(
                        this.assembly,
                        noExtension: this.options.GitHubPages || this.options.GitlabWiki,
                        noPrefix: this.options.GitlabWiki);
                IEnumerable<XNode> nodes = memberDocElement?.Element("value")?.Nodes();
                MarkdownParagraph valueDoc = this.XNodesToMarkdownParagraph(nodes);

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

        if (!(exceptionDocs?.Count() > 0))
        {
            return;
        }
        this.document.AppendHeader("Exceptions", 4);

        foreach (XElement exceptionDoc in exceptionDocs)
        {
            string cref = exceptionDoc.Attribute("cref")?.Value;
            MarkdownInlineElement exceptionTypeName = this.GetLinkFromReference(cref);
            MarkdownParagraph exceptionSummary = this.XNodesToMarkdownParagraph(exceptionDoc.Nodes());

            this.document.AppendParagraph(string.Join($"<br>{Environment.NewLine}", exceptionTypeName, exceptionSummary));
        }
    }

    private void WriteMethodReturnType(MethodInfo methodInfo, XElement memberDocElement)
    {
        RequiredArgument.NotNull(methodInfo, nameof(methodInfo));

        this.document.AppendHeader("Returns", 4);

        MarkdownInlineElement typeName = methodInfo.ReturnType.GetDocsLink(
            this.assembly,
            noExtension: this.options.GitHubPages || this.options.GitlabWiki,
            noPrefix: this.options.GitlabWiki);
        IEnumerable<XNode> nodes = memberDocElement?.Element("returns")?.Nodes();
        MarkdownParagraph typeParamDoc = this.XNodesToMarkdownParagraph(nodes);

        this.document.AppendParagraph($"{typeName}<br>{Environment.NewLine}{typeParamDoc}");
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

        if (typeParams.Length == 0)
        {
            return;
        }

        this.document.AppendHeader("Type Parameters", 4);

        foreach (Type typeParam in typeParams)
        {
            MarkdownInlineElement typeName = typeParam.GetDocsLink(
                this.assembly,
                noExtension: this.options.GitHubPages || this.options.GitlabWiki,
                noPrefix: this.options.GitlabWiki);
            IEnumerable<XNode> nodes = memberDocElement?.Elements("typeparam").FirstOrDefault(e => e.Attribute("name")?.Value == typeParam.Name)?.Nodes();
            MarkdownParagraph typeParamDoc = this.XNodesToMarkdownParagraph(nodes);

            this.document.AppendParagraph(string.Join($"<br>{Environment.NewLine}", new MarkdownInlineCode(typeName), typeParamDoc));
        }
    }

    private void WriteMethodParams(MethodBase methodBase, XElement memberDocElement)
    {
        RequiredArgument.NotNull(methodBase, nameof(methodBase));

        ParameterInfo[] @params = methodBase.GetParameters();

        if (@params.Length == 0)
        {
            return;
        }

        this.document.AppendHeader("Parameters", 4);

        foreach (ParameterInfo param in @params)
        {
            MarkdownInlineElement typeName = param.ParameterType.GetDocsLink(
                this.assembly,
                noExtension: this.options.GitHubPages || this.options.GitlabWiki,
                noPrefix: this.options.GitlabWiki);
            IEnumerable<XNode> nodes = memberDocElement?.Elements("param").FirstOrDefault(e => e.Attribute("name")?.Value == param.Name)?.Nodes();
            MarkdownParagraph paramDoc = this.XNodesToMarkdownParagraph(nodes);

            this.document.AppendParagraph($"{new MarkdownInlineCode(param.Name)} {typeName}<br>{Environment.NewLine}{paramDoc}");
        }
    }

    private void WriteEnumFields(IEnumerable<FieldInfo> fields)
    {
        RequiredArgument.NotNull(fields, nameof(fields));

        if (!fields.Any())
        {
            return;
        }
        this.document.AppendHeader("Fields", 2);

        MarkdownTableHeader header = new(
            new MarkdownTableHeaderCell("Name"),
            new MarkdownTableHeaderCell("Value", MarkdownTableTextAlignment.Right),
            new MarkdownTableHeaderCell("Description")
        );

        MarkdownTable table = new(header, fields.Count());

        foreach (FieldInfo field in fields)
        {
            IEnumerable<XNode> nodes = this.documentation.GetMember(field)?.Element("summary")?.Nodes();
            if (nodes == null)
            {
                continue;
            }

            MarkdownParagraph summary = this.XNodesToMarkdownParagraph(nodes);
            string formattedSummary = TableFormat(summary.ToString());

            table.AddRow(new MarkdownTableRow(field.Name, ((Enum)Enum.Parse(this.type, field.Name)).ToString("D"), formattedSummary));
        }

        this.document.Append(table);
    }

    private static string TableFormat(string input)
    {
        input = input.Replace("\r\n", "\n");
        StringBuilder sb = new(input.Length);

        foreach (string line in input.Split("\n"))
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                sb.Append(line);
            }
        }

        return sb.ToString();
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

        if (crefAttribute == null ||
            crefAttribute.Length <= 2 ||
            crefAttribute[1] != ':' ||
            !MemberTypesAliases.TryGetMemberType(crefAttribute[0], out MemberTypes memberType))
        {
            return memberInfo != null;
        }

        string memberFullName = crefAttribute[2..];

        if (memberType is MemberTypes.Constructor or MemberTypes.Method)
        {
            var (@namespace, methodSignature, genericCount, parameterCount) = DeconstructMember(memberFullName);
            Type type = this.GetTypeFromFullName(@namespace);
            if (type is not null)
            {
                memberInfo = type.GetMember($"{methodSignature}*")
                            .FirstOrDefault(info =>
                    {
                        MethodBase methodBase = (MethodBase)info;
                        if (methodBase.ContainsGenericParameters
                            && methodBase.GetGenericArguments().Length != genericCount)
                        {
                            return false;
                        }
                        return methodBase.GetParameters().Length == parameterCount;
                    }) ??
                    type.GetMember($"{methodSignature}*").FirstOrDefault();
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

        return memberInfo != null;
    }

    private static (string @namespace, string methodName, int genericCount, int parameterCount) DeconstructMember(string input)
    {
        int genericIndex = input.IndexOf("``");
        int parameterIndex = input.IndexOf("(");
        int genericCount = 0;
        int parameterCount = 0;

        string parameterStripped = parameterIndex > -1 ? input[..parameterIndex] : input;
        int lastDotIndex = parameterStripped.LastIndexOf('.');
        string @namespace = input[..lastDotIndex];

        string methodName = input[(lastDotIndex + 1)..];

        if (parameterIndex > -1)
        {
            parameterCount = input[parameterIndex..].Split(',').Count();
            methodName = input[(lastDotIndex + 1)..parameterIndex];
        }
        if (genericIndex > -1)
        {
            genericCount = parameterIndex > 1
                ? int.Parse(input[(genericIndex + 2)..parameterIndex])
                : int.Parse(input[(genericIndex + 2)..]);
            methodName = input[(lastDotIndex + 1)..genericIndex];
        }

        return (@namespace, methodName.Replace('#', '.'), genericCount, parameterCount);
    }

    private IEnumerable<FieldInfo> GetFields()
    {
        if (this.options.IncludePrivateMembers)
        {
            return this.type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
                .Where(x => !x.Name.EndsWith(BackingFieldName));
        }

        return this.type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
    }

    private IEnumerable<PropertyInfo> GetProperties()
    {
        if (this.options.IncludePrivateMembers)
        {
            return this.type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
        }

        return this.type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
    }

    private IEnumerable<ConstructorInfo> GetConstructors()
    {
        if (this.options.IncludePrivateMembers)
        {
            return this.type.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
        }

        return this.type.GetConstructors(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
    }

    private IEnumerable<EventInfo> GetEvents()
    {
        if (this.options.IncludePrivateMembers)
        {
            return this.type.GetEvents(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
        }

        return this.type.GetEvents(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
    }

    private Type GetTypeFromFullName(string typeFullName)
    {
        return Type.GetType(typeFullName) ?? this.assembly.GetType(typeFullName);
    }
}
