namespace NuvTools.Common.Reflection;

/// <summary>
/// Internal implementation of <see cref="IProgramInfo"/> for storing program metadata.
/// </summary>
internal class ProgramInfo : IProgramInfo
{
    /// <summary>
    /// Gets or sets the name of the program.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the description of the program.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the version of the program.
    /// </summary>
    public string? Version { get; set; }

    /// <summary>
    /// Gets or sets the list of program components or dependencies.
    /// </summary>
    public List<string>? Components { get; set; }
}