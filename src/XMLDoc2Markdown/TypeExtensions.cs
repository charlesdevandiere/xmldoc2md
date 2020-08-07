using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace XMLDoc2Markdown
{
    internal static class TypeExtensions
    {
        internal static readonly IReadOnlyDictionary<Type, string> simplifiedTypeNames = new Dictionary<Type, string>
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

        internal static string GetSimplifiedName(this Type type)
        {
            return simplifiedTypeNames.TryGetValue(type, out string simplifiedName) ? simplifiedName : type.Name;
        }

        internal static Visibility GetVisibility(this Type type)
        {
            if (type.IsPublic)
            {
                return Visibility.Public;
            }
            else
            {
                return Visibility.None;
            }
        }

        internal static string GetSignature(this Type type, bool full = false)
        {
            var signature = new List<string>();

            if (full)
            {
                signature.Add(type.GetVisibility().Print());

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

            signature.Add(type.GetDisplayName());

            if (type.IsClass || type.IsInterface)
            {
                var baseTypeAndInterfaces = new List<Type>();

                if (type.IsClass && type.BaseType != null && type.BaseType != typeof(object))
                {
                    baseTypeAndInterfaces.Add(type.BaseType);
                }

                baseTypeAndInterfaces.AddRange(type.GetInterfaces());

                if (baseTypeAndInterfaces.Count > 0)
                {
                    signature.Add($": {string.Join(", ", baseTypeAndInterfaces.Select(t => t.Namespace != type.Namespace ? t.FullName : t.Name))}");
                }
            }

            return string.Join(' ', signature);
        }

        internal static string GetDisplayName(this Type type)
        {
            TypeInfo typeInfo = type.GetTypeInfo();
            if (typeInfo.GenericTypeParameters.Length > 0)
            {
                string @base = type.Name.Substring(0, type.Name.IndexOf('`'));
                return $"{@base}<{string.Join(", ", typeInfo.GenericTypeParameters.Select(t => t.GetDisplayName()))}>";
            }
            else
            {
                return type.Name;
            }
        }

        internal static IEnumerable<Type> GetInheritanceHierarchy(this Type type)
        {
            for (Type current = type; current != null; current = current.BaseType)
            {
                yield return current;
            }
        }

    }
}
