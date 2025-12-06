namespace NuvTools.Common.Reflection;

/// <summary>
/// Represents program metadata and information.
/// </summary>
public interface IProgramInfo
{
    /// <summary>
    /// Gets the name of the program.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the description of the program.
    /// </summary>
    string? Description { get; }

    /// <summary>
    /// Gets the version of the program.
    /// </summary>
    string? Version { get; }

    /// <summary>
    /// Gets the list of program components or dependencies.
    /// </summary>
    List<string>? Components { get; }
}