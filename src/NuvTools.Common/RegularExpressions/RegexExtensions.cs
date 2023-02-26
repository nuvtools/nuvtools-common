using NuvTools.Common.Enums;
using System.Text.RegularExpressions;
using static NuvTools.Common.RegularExpressions.RegexPattern;

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

    public static Match Match(this string value, Pattern pattern)
    {
        return value.Match(pattern.GetDescription());
    }

    public static bool IsMatch(this string value, string pattern, RegexOptions options = RegexOptions.IgnoreCase)
    {
        return Regex.IsMatch(value, pattern, options);
    }

    public static bool IsMatch(this string value, Pattern pattern)
    {
        return value.IsMatch(pattern.GetDescription());
    }

    public static string Replace(this string value, string pattern, string newValue, RegexOptions options = RegexOptions.None)
    {
        return Regex.Replace(value, pattern, newValue, options);
    }

}