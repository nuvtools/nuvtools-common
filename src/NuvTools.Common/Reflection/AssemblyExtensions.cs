using System.Globalization;
using System.Reflection;

namespace NuvTools.Common.Reflection;

/// <summary>
/// Provides extension methods for <see cref="Assembly"/> objects.
/// </summary>
public static class AssemblyExtensions
{
    /// <summary>
    /// Lists all referenced assemblies (components) from the specified assembly.
    /// </summary>
    /// <param name="assembly">The assembly to inspect.</param>
    /// <param name="fullName">If true, returns full assembly names; otherwise returns name and version.</param>
    /// <returns>A list of component names, or null if the assembly is null.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="assembly"/> is null.</exception>
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

    /// <summary>
    /// Gets the name of the assembly.
    /// </summary>
    /// <param name="assembly">The assembly to get the name from.</param>
    /// <returns>The name of the assembly, or null if not available.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="assembly"/> is null.</exception>
    public static string? Name(this Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(assembly);
        return assembly.GetName().Name;
    }

    /// <summary>
    /// Gets the description of the assembly from its <see cref="AssemblyDescriptionAttribute"/>.
    /// </summary>
    /// <param name="assembly">The assembly to get the description from.</param>
    /// <returns>The description of the assembly, or null if not available.</returns>
    public static string? Description(this Assembly assembly)
    {
        if (assembly is null) return null;
        return
            ((AssemblyDescriptionAttribute)
             assembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)[0]).Description;
    }

    /// <summary>
    /// Gets the version of the assembly in the format "major.minor.build.revision".
    /// </summary>
    /// <param name="assembly">The assembly to get the version from.</param>
    /// <returns>The formatted version string, or null if not available.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="assembly"/> is null.</exception>
    /// <exception cref="VersionException">Thrown when there is an error retrieving the version information.</exception>
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

    /// <summary>
    /// Extracts comprehensive program information from the assembly.
    /// </summary>
    /// <param name="assembly">The assembly to extract information from.</param>
    /// <returns>An <see cref="IProgramInfo"/> object containing name, description, version, and component information.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="assembly"/> is null or assembly name is null.</exception>
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