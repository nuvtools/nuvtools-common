using NuvTools.Common.Enums;
using System.ComponentModel;

namespace NuvTools.Common.RegularExpressions;

/// <summary>
/// Regex patterns class.
/// </summary>
public static class RegexPattern
{

    public enum Pattern
    {
        [Description(RegexEmailAddress)]
        Email,
        [Description(RegexBase64File)]
        Base64
    }
    /// <summary>
    /// Regex pattern for e-mail address validation.
    /// </summary>
    public const string RegexEmailAddress = @"^([a-z0-9_\-])([a-z0-9_\-\.]*)@(\[((25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9][0-9]|[0-9])\.){3}|((([a-z0-9\-]+)\.)+))([a-z]{2,}|(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9][0-9]|[0-9])\])$";

    /// <summary>
    /// Regex to validate and extract informations (type, extension and content) from base64 file.
    /// </summary>
    public const string RegexBase64File = @"data:(?<type>.+?/(?<extension>.+?));(?<base>.+),(?<content>.+)";
}