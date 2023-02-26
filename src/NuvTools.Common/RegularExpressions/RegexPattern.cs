namespace NuvTools.Common.RegularExpressions;

/// <summary>
/// Regex patterns class.
/// </summary>
public static class RegexPattern
{
    /// <summary>
    /// Regex to validate and extract informations (type, extension and content) from base64 file.
    /// </summary>
    public const string Base64File = @"data:(?<type>.+?/(?<extension>.+?));(?<base>.+),(?<content>.+)";
}