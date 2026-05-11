using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace XMLDoc2Markdown.Signatures.Modifiers;

/// <summary>
/// Recognises types synthesised by the C# <c>record</c> / <c>record struct</c>
/// compiler. Detection relies on the compiler-generated cloning / equality
/// scaffolding that Roslyn always emits for records.
/// </summary>
internal static class RecordDetection
{
    private const BindingFlags AnyInstance =
        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

    internal static bool IsRecord(Type type) => IsRecordClass(type) || IsRecordStruct(type);

    internal static bool IsRecordClass(Type type)
    {
        if (!type.IsClass)
        {
            return false;
        }

        return type.GetMethod("<Clone>$", AnyInstance) != null;
    }

    internal static bool IsRecordStruct(Type type)
    {
        if (!type.IsValueType || type.IsEnum)
        {
            return false;
        }

        MethodInfo? print = type.GetMethod(
            "PrintMembers",
            AnyInstance,
            binder: null,
            types: [typeof(StringBuilder)],
            modifiers: null);

        return print != null
            && print.ReturnType == typeof(bool)
            && print.IsDefined(typeof(CompilerGeneratedAttribute), inherit: false);
    }
}
