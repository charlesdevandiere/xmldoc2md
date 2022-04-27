using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace XMLDoc2Markdown.Utils
{
    internal static class MemberInfoExtensions
    {
        internal static string GetSignature(this MemberInfo memberInfo, bool full = false)
        {
            if (memberInfo is Type type)
            {
                return type.GetSignature(full);
            }
            else if (memberInfo is MethodBase methodBase)
            {
                return methodBase.GetSignature(full);
            }
            else if (memberInfo is PropertyInfo propertyInfo)
            {
                return propertyInfo.GetSignature(full);
            }
            else if (memberInfo is EventInfo eventInfo)
            {
                return eventInfo.GetSignature(full);
            }
            else if (memberInfo is FieldInfo fieldInfo)
            {
                return fieldInfo.GetSignature(full);
            }

            throw new NotImplementedException();
        }

        internal static string GetIdentifier(this MemberInfo memberInfo)
        {
            switch (memberInfo)
            {
                case Type type:
                    return Regex.Replace(type.FullName, @"\[.*\]", string.Empty).Replace('+', '.');

                case PropertyInfo _:
                case FieldInfo _:
                case EventInfo _:
                    return memberInfo.DeclaringType.GetIdentifier() + "." + memberInfo.Name;

                case MethodBase methodBase:
                    Dictionary<string, int> typeGenericMap = new Dictionary<string, int>();
                    Type[] typeGenericArguments = methodBase.DeclaringType.GetGenericArguments();
                    for (int i = 0; i < typeGenericArguments.Length; i++)
                    {
                        Type typeGeneric = typeGenericArguments[i];
                        typeGenericMap[typeGeneric.Name] = i;
                    }

                    Dictionary<string, int> methodGenericMap = new Dictionary<string, int>();
                    if (methodBase is MethodInfo)
                    {
                        Type[] methodGenericArguments = methodBase.GetGenericArguments();
                        for (int i = 0; i < methodGenericArguments.Length; i++)
                        {
                            Type methodGeneric = methodGenericArguments[i];
                            methodGenericMap[methodGeneric.Name] = i;
                        }
                    }

                    ParameterInfo[] parameterInfos = methodBase.GetParameters();

                    string identifier = GetFormatedTypeName(methodBase.DeclaringType, false, typeGenericMap, methodGenericMap);
                    identifier += ".";
                    identifier += methodBase is ConstructorInfo ? "#ctor" : methodBase.Name;
                    identifier += methodGenericMap.Count > 0 ? "``" + methodGenericMap.Count : string.Empty;
                    identifier += parameterInfos.Length > 0
                        ? $@"({string.Join(
                            ",",
                            methodBase.GetParameters().Select(x => GetFormatedTypeName(x.ParameterType, true, typeGenericMap, methodGenericMap)))})"
                        : string.Empty;

                    if (methodBase is MethodInfo methodInfo &&
                        (methodBase.Name == "op_Implicit" ||
                        methodBase.Name == "op_Explicit"))
                    {
                        identifier += "~" + GetFormatedTypeName(methodInfo.ReturnType, true, typeGenericMap, methodGenericMap);
                    }

                    return identifier;

                default:
                    throw new Exception($"{nameof(GetIdentifier)} encountered an unhandled member info: {memberInfo}");
            }
        }

        private static string GetFormatedTypeName(
            Type type,
            bool isMethodParameter,
            Dictionary<string, int> typeGenericMap,
            Dictionary<string, int> methodGenericMap)
        {
            if (type.IsGenericParameter)
            {
                return methodGenericMap.TryGetValue(type.Name, out int methodIndex)
                    ? "``" + methodIndex
                    : "`" + typeGenericMap[type.Name];
            }
            else if (type.HasElementType)
            {
                string elementTypeString = GetFormatedTypeName(
                    type.GetElementType(),
                    isMethodParameter,
                    typeGenericMap,
                    methodGenericMap);

                switch (type)
                {
                    case Type when type.IsPointer:
                        return elementTypeString + "*";

                    case Type when type.IsByRef:
                        return elementTypeString + "@";

                    case Type when type.IsArray:
                        int rank = type.GetArrayRank();
                        string arrayDimensionsString = rank > 1
                            ? $"[{string.Join(",", Enumerable.Repeat("0:", rank))}]"
                            : "[]";
                        return elementTypeString + arrayDimensionsString;

                    default:
                        throw new Exception($"{nameof(GetFormatedTypeName)} encountered an unhandled element type: {type}");
                }
            }
            else
            {
                string name = type.IsNested
                    ? GetFormatedTypeName(
                        type.DeclaringType,
                        isMethodParameter,
                        typeGenericMap,
                        methodGenericMap) + "."
                    : type.Namespace + ".";

                name += isMethodParameter
                    ? Regex.Replace(type.Name, @"`\d+", string.Empty)
                    : type.Name;

                if (type.IsGenericType && isMethodParameter)
                {
                    IEnumerable<string> genericArgs = type.GetGenericArguments()
                                .Select(argument => GetFormatedTypeName(argument, isMethodParameter, typeGenericMap, methodGenericMap));
                    name += $"{{{string.Join(",", genericArgs)}}}";
                }

                return name;
            }
        }
    }
}
