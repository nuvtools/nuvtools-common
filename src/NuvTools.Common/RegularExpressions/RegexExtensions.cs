using System.Text.RegularExpressions;
using NuvTools.Common.Resources;

namespace NuvTools.Common.RegularExpressions;

/// <summary>
/// Provides extension methods for simplifying common <see cref="Regex"/> operations on <see cref="string"/> values.
/// </summary>
public static class RegexExtensions
{
    /// <summary>
    /// Searches the specified input string for the first occurrence of a regular expression pattern.
    /// </summary>
    /// <param name="value">The input string to search. If <see langword="null"/>, returns an empty <see cref="Match"/>.</param>
    /// <param name="pattern">The regular expression pattern to match.</param>
    /// <param name="options">Optional <see cref="RegexOptions"/> to modify the search behavior. Default is <see cref="RegexOptions.None"/>.</param>
    /// <returns>
    /// A <see cref="Match"/> object containing information about the first match found in <paramref name="value"/>.
    /// If <paramref name="value"/> is <see langword="null"/>, returns an empty match.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="pattern"/> is <see langword="null"/> or empty.</exception>
    public static Match Match(this string? value, string pattern, RegexOptions options = RegexOptions.None)
    {
        if (string.IsNullOrEmpty(pattern))
            throw new ArgumentException(Messages.RegexPatternCannotBeNullOrEmpty, nameof(pattern));

        if (value is null)
            return Regex.Match(string.Empty, pattern);

        return Regex.Match(value, pattern, options);
    }

    /// <summary>
    /// Determines whether the specified regular expression finds a match in the input string.
    /// </summary>
    /// <param name="value">The input string to search. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="pattern">The regular expression pattern to match.</param>
    /// <param name="options">
    /// Optional <see cref="RegexOptions"/> to modify the search behavior.
    /// Default is <see cref="RegexOptions.IgnoreCase"/>.
    /// </param>
    /// <returns><see langword="true"/> if the regular expression finds a match; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="pattern"/> is <see langword="null"/> or empty.</exception>
    public static bool IsMatch(this string? value, string pattern, RegexOptions options = RegexOptions.IgnoreCase)
    {
        if (string.IsNullOrEmpty(pattern))
            throw new ArgumentException(Messages.RegexPatternCannotBeNullOrEmpty, nameof(pattern));

        if (string.IsNullOrEmpty(value))
            return false;

        return Regex.IsMatch(value, pattern, options);
    }

    /// <summary>
    /// Replaces all occurrences of a specified regular expression pattern in the input string with a replacement string.
    /// </summary>
    /// <param name="value">The input string to modify. If <see langword="null"/>, returns an empty string.</param>
    /// <param name="pattern">The regular expression pattern to match.</param>
    /// <param name="newValue">The replacement string. If <see langword="null"/>, it is treated as an empty string.</param>
    /// <param name="options">Optional <see cref="RegexOptions"/> to modify the search behavior. Default is <see cref="RegexOptions.None"/>.</param>
    /// <returns>
    /// A new string that is identical to the input string except that all instances of <paramref name="pattern"/>
    /// have been replaced with <paramref name="newValue"/>.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="pattern"/> is <see langword="null"/> or empty.</exception>
    public static string? ReplacePattern(this string? value, string pattern, string newValue, RegexOptions options = RegexOptions.None)
    {
        if (value is null) return null;

        if (string.IsNullOrEmpty(pattern))
            throw new ArgumentException(Messages.RegexPatternCannotBeNullOrEmpty, nameof(pattern));

        return Regex.Replace(value, pattern, newValue ?? string.Empty, options);
    }
}