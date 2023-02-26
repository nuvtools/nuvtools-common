using System.Globalization;
using System.Reflection;

namespace NuvTools.Common.Reflection;

public static class AssemblyExtensions
{

    public static List<string> ListComponent(this Assembly assembly, bool fullName = false)
    {
        if (assembly is null) return null;

        AssemblyName[] referenceList = assembly.GetReferencedAssemblies().OrderBy(e => e.Name).ToArray();

        List<string> list = new();

        foreach (AssemblyName reference in referenceList)
        {
            Assembly libraryReference = Assembly.Load(reference.FullName);
            if (libraryReference is null) continue;

            list.Add(fullName ? libraryReference.FullName : libraryReference.Name() + " - " + libraryReference.Version());
        }

        return list.Distinct().ToList();
    }

    public static string Name(this Assembly assembly)
    {
        if (assembly is null) return null;
        return assembly.GetName().Name;
    }

    public static string Description(this Assembly assembly)
    {
        if (assembly is null) return null;
        return
            ((AssemblyDescriptionAttribute)
             assembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)[0]).Description;
    }

    public static string Version(this Assembly assembly)
    {
        if (assembly is null) return null;

        try
        {

            string mainVersion = assembly.GetName().Version.Major + "." +
                                     assembly.GetName().Version.Minor + "." +
                                     assembly.GetName().Version.Build;

            string extraVersion = assembly.GetName().Version.Revision.ToString("0000", CultureInfo.InvariantCulture);

            mainVersion += "." + extraVersion;

            return mainVersion;

        }
        catch (Exception e)
        {
            throw new VersionException(e);
        }

    }

    public static IProgramInfo ProgramInfo(this Assembly assembly)
    {
        if (assembly is null) throw new ArgumentNullException(nameof(assembly));

        return new ProgramInfo
        {
            Name = assembly.Name(),
            Description = assembly.Description(),
            Version = assembly.Version(),
            Components = assembly.ListComponent()
        };
    }

    /// <summary>
    /// Returns a Embedded Resource from assembly.
    /// </summary>
    /// <param name="assembly">The assembly instance which from resource will be retrevied.</param>
    /// <param name="name">The resource name.</param>
    /// <returns></returns>
    public static Stream Resource(this Assembly assembly, string name)
    {
        if (assembly is null) return null;
        return assembly.GetManifestResourceStream(assembly.GetName().Name + "." + name);
    }
}