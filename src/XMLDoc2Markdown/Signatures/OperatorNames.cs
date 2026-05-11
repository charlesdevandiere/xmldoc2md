using System.Reflection;

namespace XMLDoc2Markdown.Signatures;

internal static class OperatorNames
{
    private static readonly IReadOnlyDictionary<string, string> Symbols = new Dictionary<string, string>
    {
        // Unary
        { "op_UnaryPlus", "+" },
        { "op_UnaryNegation", "-" },
        { "op_LogicalNot", "!" },
        { "op_OnesComplement", "~" },
        { "op_Increment", "++" },
        { "op_Decrement", "--" },
        { "op_True", "true" },
        { "op_False", "false" },
        // Binary arithmetic
        { "op_Addition", "+" },
        { "op_Subtraction", "-" },
        { "op_Multiply", "*" },
        { "op_Division", "/" },
        { "op_Modulus", "%" },
        // Binary bitwise / shift
        { "op_BitwiseAnd", "&" },
        { "op_BitwiseOr", "|" },
        { "op_ExclusiveOr", "^" },
        { "op_LeftShift", "<<" },
        { "op_RightShift", ">>" },
        { "op_UnsignedRightShift", ">>>" },
        // Comparison
        { "op_Equality", "==" },
        { "op_Inequality", "!=" },
        { "op_LessThan", "<" },
        { "op_GreaterThan", ">" },
        { "op_LessThanOrEqual", "<=" },
        { "op_GreaterThanOrEqual", ">=" },
    };

    internal static bool IsOperator(MethodBase methodBase)
        => methodBase.IsSpecialName
        && methodBase is MethodInfo
        && methodBase.Name.StartsWith("op_", StringComparison.Ordinal);

    internal static bool IsConversion(MethodBase methodBase)
        => methodBase.Name is "op_Implicit" or "op_Explicit";

    internal static string? TryGetSymbol(string opName)
        => Symbols.TryGetValue(opName, out string? symbol) ? symbol : null;
}
