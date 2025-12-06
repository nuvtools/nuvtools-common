using System.Reflection;

namespace NuvTools.Common.Reflection;

/// <summary>
/// Provides helper methods for working with assemblies and embedded resources.
/// </summary>
public static class AssemblyHelper
{
    /// <summary>
    /// Retrieves an embedded resource stream by name from a specified assembly.
    /// </summary>
    /// <param name="resourceName">The name of the embedded resource to retrieve.</param>
    /// <param name="assemblyName">The name of the assembly containing the resource.</param>
    /// <returns>A <see cref="Stream"/> containing the resource data, or null if the resource is not found.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="resourceName"/> or <paramref name="assemblyName"/> is null or empty.</exception>
    public static Stream? ResourceByName(string resourceName, string assemblyName)
    {
        ArgumentException.ThrowIfNullOrEmpty(assemblyName, nameof(assemblyName));
        ArgumentException.ThrowIfNullOrEmpty(resourceName, nameof(resourceName));

        Assembly assembly = Assembly.Load(new AssemblyName(assemblyName));
        return assembly.Resource(resourceName);
    }
}