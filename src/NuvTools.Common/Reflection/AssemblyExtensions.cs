using System.Globalization;
using System.Reflection;

namespace NuvTools.Common.Reflection;

public static class AssemblyExtensions
{

    public static List<string>? ListComponent(this Assembly assembly, bool fullName = false)
    {
        ArgumentNullException.ThrowIfNull(assembly);

        AssemblyName[] referenceList = assembly.GetReferencedAssemblies().OrderBy(e => e.Name).ToArray();

        List<string> list = [];

        foreach (AssemblyName reference in referenceList)
        {
            Assembly libraryReference = Assembly.Load(reference.FullName);
            if (libraryReference is null) continue;

            var detailComponent = fullName ? libraryReference.FullName : libraryReference.Name() + " - " + libraryReference.Version();

            if (detailComponent is null) continue;

            list.Add(detailComponent);
        }

        return list.Distinct().ToList();
    }

    public static string? Name(this Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(assembly);
        return assembly.GetName().Name;
    }

    public static string? Description(this Assembly assembly)
    {
        if (assembly is null) return null;
        return
            ((AssemblyDescriptionAttribute)
             assembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)[0]).Description;
    }

    public static string? Version(this Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(assembly);

        try
        {
            var version = assembly.GetName().Version;

            if (version is null) return null;

            string mainVersion = version.Major + "." +
                                     version.Minor + "." +
                                     version.Build;

            string extraVersion = version.Revision.ToString("0000", CultureInfo.InvariantCulture);

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
        ArgumentNullException.ThrowIfNull(assembly);
        ArgumentNullException.ThrowIfNull(assembly.Name());

        return new ProgramInfo
        {
            Name = assembly.Name()!,
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
    public static Stream? Resource(this Assembly assembly, string name)
    {
        ArgumentNullException.ThrowIfNull(assembly);
        return assembly.GetManifestResourceStream(assembly.GetName().Name + "." + name);
    }
}