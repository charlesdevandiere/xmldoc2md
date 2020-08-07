using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace XMLDoc2Markdown
{
    internal static class AssemblyExtensions
    {
        internal static IEnumerable<string> GetDeclaredNamespaces(this Assembly assembly)
        {
            return assembly.GetTypes().Select(type => type.Namespace).Distinct();
        }
    }
}
