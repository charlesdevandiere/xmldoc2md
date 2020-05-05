using System;
using System.Collections.Generic;

namespace XMLDoc2Markdown
{
    public static class TypeExtensions
    {
        private static readonly IReadOnlyDictionary<string, string> TYPE_SIMPLIFIED_NAMES = new Dictionary<string, string>
        {
            { "Void", "void" },
            { "Object", "object" },
            { "Boolean", "bool" },
            { "SByte", "sbyte" },
            { "Byte", "byte" },
            { "Int16", "short" },
            { "UInt16", "ushort" },
            { "Int32", "int" },
            { "UInt32", "uint" },
            { "Int64", "long" },
            { "UInt64", "ulong" },
            { "Single", "float" },
            { "Double", "double" },
            { "Decimal", "decimal" },
            { "Char", "char" },
            { "String", "string" },
        };

        public static string GetSimplifiedName(this Type type)
        {
            return TYPE_SIMPLIFIED_NAMES.TryGetValue(type.Name, out string simplifiedName) ? simplifiedName : type.Name;
        }
    }
}
