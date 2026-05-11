using System.Reflection;
using XMLDoc2Markdown.XmlDocId;

namespace XMLDoc2Markdown.Rendering;

/// <summary>
/// Resolves a C# XML-doc <c>cref</c> attribute to a <see cref="MemberInfo"/>
/// (when it points into the assembly under documentation) and produces the
/// display text for the link. Pure parsing — no Markdown emission.
/// </summary>
internal sealed class CrefResolver
{
    private const BindingFlags AllBindings =
        BindingFlags.Public | BindingFlags.NonPublic |
        BindingFlags.Instance | BindingFlags.Static;

    private readonly Assembly assembly;

    internal CrefResolver(Assembly assembly)
    {
        this.assembly = assembly;
    }

    internal bool TryResolve(string? cref, out MemberInfo? memberInfo)
    {
        memberInfo = null;

        if (cref is null || cref.Length <= 2 || cref[1] != ':'
            || !MemberTypesAliases.TryGetMemberType(cref[0], out MemberTypes memberType))
        {
            return false;
        }

        string memberFullName = cref[2..];

        if (memberType is MemberTypes.Constructor or MemberTypes.Method)
        {
            memberInfo = this.ResolveMethod(memberFullName);
        }
        else if (memberType is MemberTypes.Event or MemberTypes.Field or MemberTypes.Property)
        {
            memberInfo = this.ResolveSimpleMember(memberFullName);
        }
        else if (memberType is MemberTypes.TypeInfo or MemberTypes.NestedType)
        {
            memberInfo = this.GetTypeFromFullName(memberFullName);
        }

        return memberInfo != null;
    }

    /// <summary>
    /// For a self-closing <c>&lt;see cref="..."/&gt;</c> with a generic-type cref,
    /// produces a sensible display name (e.g. <c>Dictionary&lt;String, Int32&gt;</c>).
    /// </summary>
    internal static string? FormatDisplayName(string? cref)
    {
        if (cref is null || cref.Length <= 2 || cref[1] != ':')
        {
            return null;
        }
        if ((cref[0] != 'T' && cref[0] != '!') || !cref.Contains('{'))
        {
            return null;
        }
        return FormatCrefDisplayName(cref[2..]);
    }

    private MethodBase? ResolveMethod(string memberFullName)
    {
        (string @namespace, string methodSignature, int genericCount, int parameterCount) = DeconstructMember(memberFullName);
        Type? currentType = this.GetTypeFromFullName(@namespace);
        if (currentType is null)
        {
            return null;
        }

        MemberInfo[] candidates = currentType.GetMember(
            $"{methodSignature}*",
            MemberTypes.Constructor | MemberTypes.Method,
            AllBindings);

        MemberInfo? match = candidates.FirstOrDefault(info =>
        {
            MethodBase mb = (MethodBase)info;
            if (mb.ContainsGenericParameters && mb.GetGenericArguments().Length != genericCount)
            {
                return false;
            }
            return mb.GetParameters().Length == parameterCount;
        }) ?? candidates.FirstOrDefault();

        return match as MethodBase;
    }

    private MemberInfo? ResolveSimpleMember(string memberFullName)
    {
        int idx = memberFullName.LastIndexOf('.');
        Type? currentType = this.GetTypeFromFullName(memberFullName[..idx]);
        return currentType?.GetMember(memberFullName[(idx + 1)..], AllBindings).FirstOrDefault();
    }

    private Type? GetTypeFromFullName(string typeFullName)
    {
        string normalized = NormalizeGenericTypeName(typeFullName);
        return Type.GetType(normalized) ?? this.assembly.GetType(normalized);
    }

    // XML doc cref values denote closed generic types as `Type{Arg1,Arg2}` (e.g. Dictionary{System.String,System.Int32}).
    // For type lookup we collapse each {…} to the CLR arity form `N so we can resolve the open generic and
    // produce a working docs link.
    internal static string NormalizeGenericTypeName(string name)
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
            if (c == '{') depth++;
            else if (c == '}') depth--;
            else if (c == ',' && depth == 0) arity++;
        }

        string head = name[..braceIdx];
        string tail = name[(closeBrace + 1)..];
        return $"{head}`{arity}{NormalizeGenericTypeName(tail)}";
    }

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
            if (c is '{' or '(' or '[') depth++;
            else if (c is '}' or ')' or ']') depth--;
            else if (c == ',' && depth == 0) count++;
        }
        return count;
    }

    private static int FindMatchingBrace(string s, int openIdx)
    {
        int depth = 1;
        for (int i = openIdx + 1; i < s.Length; i++)
        {
            if (s[i] == '{') depth++;
            else if (s[i] == '}')
            {
                depth--;
                if (depth == 0) return i;
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
            if (c == '{') depth++;
            else if (c == '}') depth--;
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
