using System.Reflection;
using System.Text;

namespace XMLDoc2Markdown.Utils
{
    internal static class ParameterInfoExtension
    {
        public static Type GetActualType(this ParameterInfo parameter)
            => parameter.ParameterType.IsByRef ? parameter.ParameterType.GetElementType()! : parameter.ParameterType;

        public static string GetDisplayLabel(this ParameterInfo parameter, bool full = false)
        {
            StringBuilder nameBuilder = new();

            if (full && parameter.ParameterType.IsByRef)
            {
                string prefix = parameter.IsIn ? "in "
                              : parameter.IsOut ? "out "
                              : "ref ";
                nameBuilder.Append(prefix);
            }

            nameBuilder.Append(parameter.GetActualType().GetDisplayName(simplifyName: full));

            if (full)
                nameBuilder.Append($" {parameter.Name}");

            return nameBuilder.ToString();
        }
    }
}
