using System.Text.RegularExpressions;

namespace NuvTools.Common.RegularExpressions;

/// <summary>
/// Regex extension class.
/// </summary>
public static class RegexExtensions
{

    public static Match Match(this string value, string pattern, RegexOptions options = RegexOptions.None)
    {
        return Regex.Match(value, pattern, options);
    }

    public static bool IsMatch(this string value, string pattern, RegexOptions options = RegexOptions.IgnoreCase)
    {
        return Regex.IsMatch(value, pattern, options);
    }

    public static string Replace(this string value, string pattern, string newValue, RegexOptions options = RegexOptions.None)
    {
        return Regex.Replace(value, pattern, newValue, options);
    }

}