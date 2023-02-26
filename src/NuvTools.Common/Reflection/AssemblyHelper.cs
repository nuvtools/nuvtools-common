using System.IO;
using System.Reflection;

namespace NuvTools.Common.Reflection;

public static class AssemblyHelper
{
    public static Stream ResourceByName(string resourceName, string assemblyName = null)
    {
        Assembly assembly = Assembly.Load(new AssemblyName(assemblyName));
        return assembly.Resource(resourceName);
    }
}