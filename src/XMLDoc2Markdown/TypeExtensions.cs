using System;
using System.Collections.Generic;

namespace XMLDoc2Markdown
{
    public static class TypeExtensions
    {
        private static readonly IReadOnlyDictionary<Type, string> simplifiedTypeNames = new Dictionary<Type, string>
        {
            // void
            { typeof(void), "void" },
            // object
            { typeof(object), "object" },
            // bool
            { typeof(bool), "bool" },
            // numeric
            { typeof(sbyte), "sbyte" },
            { typeof(byte), "byte" },
            { typeof(short), "short" },
            { typeof(ushort), "ushort" },
            { typeof(int), "int" },
            { typeof(uint), "uint" },
            { typeof(long), "long" },
            { typeof(ulong), "ulong" },
            { typeof(float), "float" },
            { typeof(double), "double" },
            { typeof(decimal), "decimal" },
            // text
            { typeof(char), "char" },
            { typeof(string), "string" },
        };

        public static string GetSimplifiedName(this Type type)
        {
            return simplifiedTypeNames.TryGetValue(type, out string simplifiedName) ? simplifiedName : type.Name;
        }

        public static string GetVisibility(this Type type)
        {
            if (type.IsPublic)
            {
                return "public";
            }
            else
            {
                return string.Empty;
            }
        }

        public static string GetSignature(this Type type, bool full = false)
        {
            var signature = new List<string>();

            if (full)
            {
                signature.Add(type.GetVisibility());

                if (type.IsClass)
                {
                    if (type.IsAbstract && type.IsSealed)
                    {
                        signature.Add("static");
                    }
                    else if (type.IsAbstract)
                    {
                        signature.Add("abstract");
                    }
                    else if (type.IsSealed)
                    {
                        signature.Add("sealed");
                    }

                    signature.Add("class");
                }
                else if (type.IsInterface)
                {
                    signature.Add("interface");
                }
                else if (type.IsEnum)
                {
                    signature.Add("enum");
                }
                else if (type.IsValueType)
                {
                    signature.Add("struct");
                }
            }

            signature.Add(type.Name);

            return string.Join(' ', signature);
        }
    }
}
