using System.Reflection;
using System.Runtime.Loader;

namespace XMLDoc2Markdown.Utils;

internal class AssemblyLoadContext : System.Runtime.Loader.AssemblyLoadContext
{
    private readonly AssemblyDependencyResolver resolver;

    internal AssemblyLoadContext(string pluginPath)
    {
        this.resolver = new AssemblyDependencyResolver(pluginPath);
    }

    protected override Assembly? Load(AssemblyName assemblyName)
    {
        string? assemblyPath = this.resolver.ResolveAssemblyToPath(assemblyName);
        if (assemblyPath != null)
        {
            return this.LoadFromAssemblyPath(assemblyPath);
        }

        return null;
    }

    protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
    {
        string? libraryPath = this.resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
        if (libraryPath != null)
        {
            return this.LoadUnmanagedDllFromPath(libraryPath);
        }

        return IntPtr.Zero;
    }
}
