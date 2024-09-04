using System.Reflection;

namespace NuvTools.Common.Reflection;

public static class AssemblyHelper
{
    public static Stream? ResourceByName(string resourceName, string assemblyName)
    {
        ArgumentException.ThrowIfNullOrEmpty(assemblyName, nameof(assemblyName));
        ArgumentException.ThrowIfNullOrEmpty(resourceName, nameof(resourceName));

        Assembly assembly = Assembly.Load(new AssemblyName(assemblyName));
        return assembly.Resource(resourceName);
    }
}