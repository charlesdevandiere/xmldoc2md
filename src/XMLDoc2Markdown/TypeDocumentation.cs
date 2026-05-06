using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Markdown;
using XMLDoc2Markdown.Utils;

namespace XMLDoc2Markdown;

internal partial class TypeDocumentation
{
    private const string BackingFieldName = ">k__BackingField";

    [GeneratedRegex("[ ]{2,}")]
    private static partial Regex CollapseSpacesRegex();

    private readonly Assembly assembly;
    private readonly Type type;
    private readonly XmlDocumentation documentation;
    private readonly TypeDocumentationOptions options;
    private readonly MarkdownDocument document = new();

    internal TypeDocumentation(Assembly assembly, Type type, XmlDocumentation documentation, TypeDocumentationOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(assembly);
        ArgumentNullException.ThrowIfNull(type);
        ArgumentNullException.ThrowIfNull(documentation);

        this.assembly = assembly;
        this.type = type;
        this.documentation = documentation;
        this.options = options ?? new();
    }

    public override string ToString()
    {
        if (this.options.BackButton)
        {
            this.WriteBackButton(top: true);
        }

        this.document.AppendHeader(this.type.GetDisplayName().FormatChevrons(), 1);

        if (this.type.Namespace != null)
        {
            this.document.AppendParagraph($"Namespace: {this.type.Namespace}");
        }

        XElement? typeDocElement = this.documentation.GetMember(this.type);

        if (typeDocElement != null)
        {
            Logger.Info("    (documented)");
        }

        this.WriteObsolete();
        this.WriteMemberInfoSummary(typeDocElement);
        this.WriteMemberInfoSignature(this.type);
        this.WriteTypeParameters(this.type, typeDocElement);
        this.WriteInheritanceAndImplementsAndAttributes();
        this.WriteMemberInfoRemarks(typeDocElement);

        if (this.type.IsEnum)
        {
            this.WriteEnumFields(this.GetFields().Where(m => !m.IsSpecialName).ToArray());
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
                .Where(m => m.GetAccessibility() >= this.options.MemberAccessibilityLevel)
                .ToArray());
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

        int deep = this.type.GetDocsFileName(this.options.Structure).Count(f => f == '/');
        string route = deep > 0
            ? string.Join('/', Enumerable.Repeat("..", deep))
            : ".";
        route += "/";
        this.document.AppendParagraph(new MarkdownLink(new MarkdownInlineCode("< Back"), route));

        if (top)
        {
            this.document.AppendHorizontalRule();
        }
    }

    private void WriteInheritanceAndImplementsAndAttributes()
    {
        List<string> lines = [];

        // inheritance
        if (this.type.BaseType != null)
        {
            IEnumerable<MarkdownInlineElement> inheritanceHierarchy = this.type.GetInheritanceHierarchy()
                .Reverse()
                .Select(t => t.GetDocsLink(
                    this.assembly,
                    this.options.Structure,
                    noExtension: this.options.GitHubPages || this.options.GitlabWiki,
                    noPrefix: this.options.GitlabWiki));
            lines.Add($"Inheritance {string.Join(" → ", inheritanceHierarchy)}");
        }

        // interfaces
        Type[] interfaces = this.type.GetInterfaces();
        if (interfaces.Length > 0)
        {
            IEnumerable<MarkdownInlineElement> implements = interfaces
                .Select(i => i.GetDocsLink(
                    this.assembly,
                    this.options.Structure,
                    noExtension: this.options.GitHubPages || this.options.GitlabWiki,
                    noPrefix: this.options.GitlabWiki));
            lines.Add($"Implements {string.Join(", ", implements)}");
        }

        // attributes
        IEnumerable<Attribute> attributes = this.type.GetCustomAttributes();
        if (attributes.Any())
        {
            IEnumerable<MarkdownInlineElement> links = attributes
                .Select(i => i.GetType().GetDocsLink(
                    this.assembly,
                    this.options.Structure,
                    noExtension: this.options.GitHubPages || this.options.GitlabWiki,
                    noPrefix: this.options.GitlabWiki));
            lines.Add($"Attributes {string.Join(", ", links)}");
        }

        if (lines.Count != 0)
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
        ObsoleteAttribute? first = attribute.FirstOrDefault();
        if (first is not null)
        {
            document.AppendHeader("Caution", 4);

            string? message = first.Message;
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

    private void WriteMemberInfoSummary(XElement? memberDocElement)
    {
        IEnumerable<XNode>? nodes = memberDocElement?.Element("summary")?.Nodes();
        if (nodes != null)
        {
            MarkdownParagraph summary = this.XNodesToMarkdownParagraph(nodes);
            this.document.Append(summary);
        }
    }

    private void WriteMemberInfoRemarks(XElement? memberDocElement)
    {
        IEnumerable<XNode>? nodes = memberDocElement?.Element("remarks")?.Nodes();
        if (nodes != null)
        {
            this.document.AppendParagraph(new MarkdownStrongEmphasis("Remarks:"));
            this.document.Append(this.XNodesToMarkdownParagraph(nodes));
        }
    }

    private object? XElementToMarkdown(XElement element)
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
            "list" => this.XElementToMarkdownList(element),
            "paramref" => new MarkdownInlineCode(element.Attribute("name")?.Value ?? string.Empty),
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
        MarkdownText? paragraph = null;
        if (nodes is null)
        {
            return new MarkdownParagraph("");
        }

        foreach (XNode node in nodes)
        {
            object? element = this.XNodeToMarkdown(node);
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

    private MarkdownList XElementToMarkdownList(XElement element)
    {
        MarkdownList markdownList = element.Attribute("type")?.Value switch
        {
            "number" => new MarkdownOrderedList(),
            _ => new MarkdownList()
        };

        foreach (XElement item in element.Elements("item"))
        {
            MarkdownText markdownListItem = new(string.Empty);

            IEnumerable<XNode> term = item.Element("term")?.Nodes() ?? [];

            MarkdownText? markdownTerm = null;

            foreach (XNode node in term)
            {
                object? md = this.XNodeToMarkdown(node);
                if (md is MarkdownInlineElement inlineElement)
                {
                    if (markdownTerm is null)
                    {
                        markdownTerm = new MarkdownText(inlineElement);
                    }
                    else
                    {
                        markdownTerm.Append(inlineElement);
                    }
                }
            }

            if (markdownTerm is not null)
            {
                markdownListItem.Append(new MarkdownStrongEmphasis(markdownTerm));
            }

            IEnumerable<XNode> description = item.Element("description")?.Nodes() ?? [];

            MarkdownText? markdownDescription = null;

            foreach (XNode node in description)
            {
                object? md = this.XNodeToMarkdown(node);
                if (md is MarkdownInlineElement inlineElement)
                {
                    if (markdownDescription is null)
                    {
                        markdownDescription = new MarkdownText(inlineElement);
                    }
                    else
                    {
                        markdownDescription.Append(inlineElement);
                    }
                }
            }

            if (markdownDescription is not null)
            {
                markdownListItem.Append(" - ");
                markdownListItem.Append(markdownDescription);
            }

            markdownList.AddItem(markdownListItem);
        }

        return markdownList;
    }

    private object? XNodeToMarkdown(XNode node)
    {
        return node switch
        {
            XText text => new MarkdownText(CollapseSpacesRegex().Replace(text.ToString(), " ")),
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

    private void WriteMembersDocumentation(IReadOnlyList<MemberInfo> members)
    {
        ArgumentNullException.ThrowIfNull(members);

        if (members.Count == 0)
        {
            return;
        }

        MemberTypes memberType = members[0].MemberType;
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

            XElement? memberDocElement = this.documentation.GetMember(member);

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

                MarkdownInlineElement? typeName = propertyInfo.GetReturnType()?
                    .GetDocsLink(
                        this.assembly,
                        this.options.Structure,
                        noExtension: this.options.GitHubPages || this.options.GitlabWiki,
                        noPrefix: this.options.GitlabWiki);
                IEnumerable<XNode> nodes = memberDocElement?.Element("value")?.Nodes() ?? [];
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

    private void WriteExceptions(XElement? memberDocElement)
    {
        XElement[] exceptionDocs = memberDocElement?.Elements("exception").ToArray() ?? [];

        if (exceptionDocs.Length == 0)
        {
            return;
        }
        this.document.AppendHeader("Exceptions", 4);

        foreach (XElement exceptionDoc in exceptionDocs)
        {
            string? cref = exceptionDoc.Attribute("cref")?.Value;
            MarkdownInlineElement? exceptionTypeName = this.GetLinkFromReference(cref);
            MarkdownParagraph exceptionSummary = this.XNodesToMarkdownParagraph(exceptionDoc.Nodes());

            this.document.AppendParagraph(string.Join($"<br>{Environment.NewLine}", exceptionTypeName, exceptionSummary));
        }
    }

    private void WriteMethodReturnType(MethodInfo methodInfo, XElement? memberDocElement)
    {
        ArgumentNullException.ThrowIfNull(methodInfo);

        this.document.AppendHeader("Returns", 4);

        MarkdownInlineElement typeName = methodInfo.ReturnType.GetDocsLink(
            this.assembly,
            this.options.Structure,
            noExtension: this.options.GitHubPages || this.options.GitlabWiki,
            noPrefix: this.options.GitlabWiki);
        IEnumerable<XNode> nodes = memberDocElement?.Element("returns")?.Nodes() ?? [];
        MarkdownParagraph typeParamDoc = this.XNodesToMarkdownParagraph(nodes);

        this.document.AppendParagraph($"{typeName}<br>{Environment.NewLine}{typeParamDoc}");
    }

    private void WriteTypeParameters(MemberInfo memberInfo, XElement? memberDocElement)
    {
        ArgumentNullException.ThrowIfNull(memberInfo);

        Type[] typeParams = memberInfo switch
        {
            TypeInfo typeInfo => typeInfo.GenericTypeParameters,
            MethodInfo methodInfo => methodInfo.GetGenericArguments(),
            _ => []
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
                this.options.Structure,
                noExtension: this.options.GitHubPages || this.options.GitlabWiki,
                noPrefix: this.options.GitlabWiki);
            IEnumerable<XNode> nodes = memberDocElement?.Elements("typeparam")?.FirstOrDefault(e => e.Attribute("name")?.Value == typeParam.Name)?.Nodes() ?? [];
            MarkdownParagraph typeParamDoc = this.XNodesToMarkdownParagraph(nodes);

            this.document.AppendParagraph(string.Join($"<br>{Environment.NewLine}", new MarkdownInlineCode(typeName), typeParamDoc));
        }
    }

    private void WriteMethodParams(MethodBase methodBase, XElement? memberDocElement)
    {
        ArgumentNullException.ThrowIfNull(methodBase);

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
                this.options.Structure,
                noExtension: this.options.GitHubPages || this.options.GitlabWiki,
                noPrefix: this.options.GitlabWiki);
            IEnumerable<XNode> nodes = memberDocElement?.Elements("param")?.FirstOrDefault(e => e.Attribute("name")?.Value == param.Name)?.Nodes() ?? [];
            MarkdownParagraph paramDoc = this.XNodesToMarkdownParagraph(nodes);

            this.document.AppendParagraph($"{new MarkdownInlineCode(param.Name ?? string.Empty)} {typeName}<br>{Environment.NewLine}{paramDoc}");
        }
    }

    private void WriteEnumFields(IReadOnlyList<FieldInfo> fields)
    {
        ArgumentNullException.ThrowIfNull(fields);

        if (fields.Count == 0)
        {
            return;
        }
        this.document.AppendHeader("Fields", 2);

        MarkdownTableHeader header = new(
            new MarkdownTableHeaderCell("Name"),
            new MarkdownTableHeaderCell("Value", MarkdownTableTextAlignment.Right),
            new MarkdownTableHeaderCell("Description")
        );

        MarkdownTable table = new(header, fields.Count);

        foreach (FieldInfo field in fields)
        {
            IEnumerable<XNode> nodes = this.documentation.GetMember(field)?.Element("summary")?.Nodes() ?? [];
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

        foreach (string line in input.Split('\n'))
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                sb.Append(line.Replace("|", "&#124;"));
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

    private MarkdownInlineElement? GetLinkFromReference(string? crefAttribute, string? text = null)
    {
        string? effectiveText = text;

        // For a self-closing <see cref="..."/>, element.Value is "" rather than null. When we have a
        // generic-type cref or an unresolved (!:) cref, prefer a parsed display name over an empty link
        // or a bare type-parameter display (Dictionary<TKey, TValue>).
        if (string.IsNullOrEmpty(effectiveText) &&
            crefAttribute is not null &&
            crefAttribute.Length > 2 &&
            crefAttribute[1] == ':' &&
            (crefAttribute[0] == 'T' || crefAttribute[0] == '!') &&
            crefAttribute.Contains('{'))
        {
            effectiveText = FormatCrefDisplayName(crefAttribute[2..]);
        }

        if (this.TryGetMemberInfoFromReference(crefAttribute, out MemberInfo? memberInfo))
        {
            return memberInfo?.GetDocsLink(
                this.assembly,
                this.options.Structure,
                text: effectiveText,
                noExtension: this.options.GitHubPages || this.options.GitlabWiki,
                noPrefix: this.options.GitlabWiki);
        }

        // No member resolved. If we still have user-provided text, render it as plain markdown.
        if (!string.IsNullOrEmpty(effectiveText))
        {
            return new MarkdownText(effectiveText);
        }

        // Fall back to the cref id itself. Strip the "T:" / "M:" / "!:" prefix and wrap in inline code
        // so backticks (XML doc generic-arity markers like ``1) survive markdown rendering.
        string fallback = crefAttribute is not null && crefAttribute.Length > 2 && crefAttribute[1] == ':'
            ? crefAttribute[2..]
            : (crefAttribute ?? string.Empty);

        return string.IsNullOrEmpty(fallback)
            ? new MarkdownText(string.Empty)
            : new MarkdownInlineCode(fallback);
    }

    private bool TryGetMemberInfoFromReference(string? crefAttribute, out MemberInfo? memberInfo)
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

        const BindingFlags AllBindings =
            BindingFlags.Public | BindingFlags.NonPublic |
            BindingFlags.Instance | BindingFlags.Static;

        if (memberType is MemberTypes.Constructor or MemberTypes.Method)
        {
            (string @namespace, string methodSignature, int genericCount, int parameterCount) = DeconstructMember(memberFullName);
            Type? currentType = this.GetTypeFromFullName(@namespace);
            if (currentType is not null)
            {
                MemberInfo[] candidates = currentType.GetMember($"{methodSignature}*", MemberTypes.Constructor | MemberTypes.Method, AllBindings);
                memberInfo = candidates
                    .FirstOrDefault(info =>
                    {
                        MethodBase methodBase = (MethodBase)info;
                        if (methodBase.ContainsGenericParameters
                            && methodBase.GetGenericArguments().Length != genericCount)
                        {
                            return false;
                        }
                        return methodBase.GetParameters().Length == parameterCount;
                    })
                    ?? candidates.FirstOrDefault();
            }
        }
        else if (memberType is MemberTypes.Event or MemberTypes.Field or MemberTypes.Property)
        {
            int idx = memberFullName.LastIndexOf('.');
            Type? currentType = this.GetTypeFromFullName(memberFullName[..idx]);
            if (currentType is not null)
            {
                memberInfo = currentType.GetMember(memberFullName[(idx + 1)..], AllBindings).FirstOrDefault();
            }
        }
        else if (memberType is MemberTypes.TypeInfo or MemberTypes.NestedType)
        {
            Type? currentType = this.GetTypeFromFullName(memberFullName);
            if (currentType is not null)
            {
                memberInfo = currentType;
            }
        }

        return memberInfo != null;
    }

    private static (string @namespace, string methodName, int genericCount, int parameterCount) DeconstructMember(string input)
    {
        int genericIndex = input.IndexOf("``");
        int parameterIndex = input.IndexOf('(');
        int genericCount = 0;
        int parameterCount = 0;

        string parameterStripped = parameterIndex > -1 ? input[..parameterIndex] : input;
        int lastDotIndex = parameterStripped.LastIndexOf('.');
        string @namespace = input[..lastDotIndex];

        string methodName = input[(lastDotIndex + 1)..];

        if (parameterIndex > -1)
        {
            int closeParenIndex = input.LastIndexOf(')');
            int paramListEnd = closeParenIndex > parameterIndex ? closeParenIndex : input.Length;
            parameterCount = CountTopLevelParameters(input, parameterIndex + 1, paramListEnd);
            methodName = input[(lastDotIndex + 1)..parameterIndex];
        }
        if (genericIndex > -1)
        {
            int genericEnd = parameterIndex > -1 ? parameterIndex : input.Length;
            if (int.TryParse(
                    input.AsSpan((genericIndex + 2)..genericEnd),
                    System.Globalization.NumberStyles.Integer,
                    System.Globalization.CultureInfo.InvariantCulture,
                    out int parsed))
            {
                genericCount = parsed;
            }
            methodName = input[(lastDotIndex + 1)..genericIndex];
        }

        return (@namespace, methodName.Replace('#', '.'), genericCount, parameterCount);
    }

    private static int CountTopLevelParameters(string input, int start, int end)
    {
        if (start >= end)
        {
            return 0;
        }

        int count = 1;
        int depth = 0;
        for (int i = start; i < end; i++)
        {
            char c = input[i];
            if (c == '{' || c == '(' || c == '[')
            {
                depth++;
            }
            else if (c == '}' || c == ')' || c == ']')
            {
                depth--;
            }
            else if (c == ',' && depth == 0)
            {
                count++;
            }
        }
        return count;
    }

    private FieldInfo[] GetFields()
    {
        HashSet<string> eventNames = new(this.GetEvents().Select(e => e.Name), StringComparer.Ordinal);
        return this.type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
            .Where(x => !x.Name.EndsWith(BackingFieldName))
            .Where(x => !eventNames.Contains(x.Name))
            .Where(x => x.GetAccessibility() >= this.options.MemberAccessibilityLevel)
            .ToArray();
    }

    private PropertyInfo[] GetProperties()
    {
        return this.type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
            .Where(x => x.GetAccessibility() >= this.options.MemberAccessibilityLevel)
            .ToArray();
    }

    private ConstructorInfo[] GetConstructors()
    {
        return this.type.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
            .Where(x => x.GetAccessibility() >= this.options.MemberAccessibilityLevel)
            .ToArray();
    }

    private EventInfo[] GetEvents()
    {
        return this.type.GetEvents(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
            .Where(x => x.GetAccessibility() >= this.options.MemberAccessibilityLevel)
            .ToArray();
    }

    private Type? GetTypeFromFullName(string typeFullName)
    {
        string normalized = NormalizeGenericTypeName(typeFullName);
        return Type.GetType(normalized) ?? this.assembly.GetType(normalized);
    }

    // XML doc cref values denote closed generic types as `Type{Arg1,Arg2}` (e.g. Dictionary{System.String,System.Int32}).
    // For type lookup we collapse each {…} to the CLR arity form `N so we can resolve the open generic and
    // produce a working docs link.
    private static string NormalizeGenericTypeName(string name)
    {
        int braceIdx = name.IndexOf('{');
        if (braceIdx == -1)
        {
            return name;
        }

        int closeBrace = FindMatchingBrace(name, braceIdx);
        if (closeBrace == -1)
        {
            return name;
        }

        int arity = 1;
        int depth = 0;
        for (int i = braceIdx + 1; i < closeBrace; i++)
        {
            char c = name[i];
            if (c == '{')
            {
                depth++;
            }
            else if (c == '}')
            {
                depth--;
            }
            else if (c == ',' && depth == 0)
            {
                arity++;
            }
        }

        string head = name[..braceIdx];
        string tail = name[(closeBrace + 1)..];
        return $"{head}`{arity}{NormalizeGenericTypeName(tail)}";
    }

    // Render a cref like `Namespace.Dictionary{System.String,System.Int32}` as `Dictionary<String, Int32>`
    // for use as link text when the cref element has no inner content.
    private static string FormatCrefDisplayName(string crefName)
    {
        int braceIdx = crefName.IndexOf('{');
        string typePart = braceIdx > -1 ? crefName[..braceIdx] : crefName;
        int lastDot = typePart.LastIndexOf('.');
        string simpleName = lastDot > -1 ? typePart[(lastDot + 1)..] : typePart;

        if (braceIdx == -1)
        {
            return simpleName;
        }

        int closeBrace = FindMatchingBrace(crefName, braceIdx);
        if (closeBrace == -1)
        {
            return simpleName;
        }

        string argsRaw = crefName[(braceIdx + 1)..closeBrace];
        IEnumerable<string> formatted = SplitTopLevelArgs(argsRaw).Select(FormatCrefDisplayName);
        return $"{simpleName}<{string.Join(", ", formatted)}>";
    }

    private static int FindMatchingBrace(string s, int openIdx)
    {
        int depth = 1;
        for (int i = openIdx + 1; i < s.Length; i++)
        {
            if (s[i] == '{')
            {
                depth++;
            }
            else if (s[i] == '}')
            {
                depth--;
                if (depth == 0)
                {
                    return i;
                }
            }
        }
        return -1;
    }

    private static IEnumerable<string> SplitTopLevelArgs(string args)
    {
        int start = 0;
        int depth = 0;
        for (int i = 0; i < args.Length; i++)
        {
            char c = args[i];
            if (c == '{')
            {
                depth++;
            }
            else if (c == '}')
            {
                depth--;
            }
            else if (c == ',' && depth == 0)
            {
                yield return args[start..i];
                start = i + 1;
            }
        }
        if (start <= args.Length)
        {
            yield return args[start..];
        }
    }
}
