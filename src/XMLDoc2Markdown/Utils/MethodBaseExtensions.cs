using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Markdown;

namespace XMLDoc2Markdown.Utils
{
    internal static class MethodBaseExtensions
    {
        internal static Visibility GetVisibility(this MethodBase methodBase)
        {
            if (methodBase.IsPublic)
            {
                return Visibility.Public;
            }
            else if (methodBase.IsAssembly)
            {
                return Visibility.Internal;
            }
            else if (methodBase.IsFamily)
            {
                return Visibility.Protected;
            }
            else if (methodBase.IsFamilyOrAssembly)
            {
                return Visibility.ProtectedInternal;
            }
            else if (methodBase.IsPrivate)
            {
                return Visibility.Private;
            }
            else
            {
                return Visibility.None;
            }
        }

        internal static string GetSignature(this MethodBase methodBase, bool full = false)
        {
            List<string> signature = new List<string>();

            if (full)
            {
                if (methodBase.DeclaringType.IsClass)
                {
                    signature.Add(methodBase.GetVisibility().Print());

                    if (methodBase.IsStatic)
                    {
                        signature.Add("static");
                    }

                    if (methodBase.IsAbstract)
                    {
                        signature.Add("abstract");
                    }
                }

                if (methodBase is MethodInfo methodInfo)
                {
                    signature.Add(methodInfo.ReturnType.GetDisplayName(simplifyName: true));
                }
            }

            string displayName = methodBase.MemberType == MemberTypes.Constructor ? methodBase.DeclaringType.Name : methodBase.Name;
            int genericCharIndex = displayName.IndexOf('`');
            if (genericCharIndex > -1)
            {
                displayName = displayName.Substring(0, genericCharIndex);
            }
            if (methodBase is MethodInfo methodInfo1)
            {
                Type[] genericArguments = methodInfo1.GetGenericArguments();
                if (genericArguments.Length > 0)
                {
                    displayName += $"<{string.Join(", ", genericArguments.Select(a => a.Name))}>";
                }
            }
            ParameterInfo[] @params = methodBase.GetParameters();
            IEnumerable<string> paramsNames = @params
                .Select(p => $"{p.ParameterType.GetDisplayName(simplifyName: full)}{(full ? $" {p.Name}" : null)}");
            displayName += $"({string.Join(", ", paramsNames)})";
            signature.Add(displayName);

            return string.Join(' ', signature);
        }
        
        internal static MarkdownInlineElement GetMethodLink(this MethodBase methodInfo, bool noExtension = false, bool noPrefix = false)
        {
            Type boundingType = methodInfo.DeclaringType;

            if (boundingType.FullName == null)
            {
                return new MarkdownText(methodInfo.GetSignature());
            }

            string typeName = boundingType.FullName.ToLower();
            string link =
                noPrefix ? string.Empty : "./" +
                typeName +
                (noExtension ? string.Empty : ".md") +
                $"#{methodInfo.Name}";
            ParameterInfo[] parameters = methodInfo.GetParameters();
            link += GetMethodHeaderLink(parameters.Select(info => info.ParameterType).ToArray());
            return new MarkdownLink(methodInfo.GetSignature(), link.ToLower());
        }

        internal static string GetMethodHeaderLink(IReadOnlyList<Type> types)
        {
            string link = string.Empty;
            for (int i = 0; i < types.Count; i++)
            {
                Type iType = types[i];
                if (i != 0)
                {
                    link += "-";
                }
                if (iType.IsArray)
                {
                    link += iType.Name[..^2];
                }
                else if (iType.IsGenericType)
                {
                    link += iType.Name[..^2];
                    link += GetMethodHeaderLink(iType.GenericTypeArguments);
                }
                else
                {
                    link += iType.Name;
                }
            }

            return link.ToLower();
        }
    }
}
