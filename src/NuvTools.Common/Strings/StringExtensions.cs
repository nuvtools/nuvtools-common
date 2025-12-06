using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace NuvTools.Common.Strings;

public static partial class StringExtensions
{
    /// <summary>
    /// Gets the first characters from string.
    /// </summary>
    /// <param name="value">String that contains the characters to be extracted.</param>
    /// <param name="length">The number of characters to be extracted.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static string? Left(this string? value, int length)
    {
        if (string.IsNullOrEmpty(value)) return value;

        length = Math.Abs(length);

        return value.Length <= length ? value : value[..length];
    }

    /// <summary>
    /// Gets the last characters from string.
    /// </summary>
    /// <param name="value">String that contains the characters to be extracted.</param>
    /// <param name="length">The number of characters to be extracted.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static string? Right(this string? value, int length)
    {
        if (string.IsNullOrEmpty(value)) return value;

        length = Math.Abs(length);

        return value.Length <= length ? value : value.Substring(value.Length - length, length);
    }

    /// <summary>
    /// Replaces the format item in a specified string with the string representation of a corresponding object by the key in the Dictionary.
    /// </summary>
    /// <param name="template">A composite format string.</param>
    /// <param name="args">A string array that contains zero or more objects to format.</param>
    /// <returns>A copy of format in which the format items have been replaced by the string representation of the corresponding strings in args.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static string Format(this string template, params object?[] args)
    {
        ArgumentException.ThrowIfNullOrEmpty(template, nameof(template));
        return template.Format(null, args);
    }

    /// <summary>
    /// Replaces the format item in a specified string with the string representation of a corresponding object by the key in the Dictionary.
    /// </summary>
    /// <param name="template">A composite format string.</param>
    /// <param name="args">A string array that contains zero or more objects to format.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>A copy of format in which the format items have been replaced by the string representation of the corresponding strings in args.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static string Format(this string template, IFormatProvider? provider, params object?[] args)
    {
        ArgumentException.ThrowIfNullOrEmpty(template, nameof(template));
        if (args == null || args.Length == 0) throw new ArgumentNullException(nameof(args));

        var dictionary = new Dictionary<string, object?>();

        for (int i = 0; i < args.Length; i++)
            dictionary.Add(i.ToString(), args[i]);

        return template.Format(dictionary, provider);
    }

    /// <summary>
    /// Replaces the format item in a specified string with the string representation of a corresponding object by the key in the Dictionary.
    /// </summary>
    /// <param name="template">A composite format string.</param>
    /// <param name="args">A Dictionary that contains zero or more values to format by the key.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>A copy of format in which the format items have been replaced by the string representation of the corresponding values in Dictionary.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static string Format(this string template, Dictionary<string, object?> args, IFormatProvider? provider = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(template, nameof(template));

        if (args == null || args.Count == 0) throw new ArgumentNullException(nameof(args));

        var regex = new Regex("{(?<variable>\\w+)(:(?<format>[\\w\\/]+))?\\}");
        var templateTokens = regex.Matches(template).Select(e => e.Groups["variable"]).Select(e => e.Value).Distinct().ToList();

        if (templateTokens.Count == 0) return template;

        var dicIndex = new Dictionary<int, object?>();
        var index = -1;
        foreach (var token in templateTokens)
        {
            index++;
            args.TryGetValue(token, out var valueDic);

            if (valueDic is null) args.TryGetValue(index.ToString(), out valueDic);

            dicIndex.Add(index, valueDic);

            template = regex.Replace(template, m =>
            {
                string variable = m.Groups["variable"].Value;
                string format = m.Groups["format"].Value;

                return $"{{{(variable == token ? index : variable)}{(!string.IsNullOrEmpty(format) ? ":" + format : string.Empty)}}}";
            });
        }

        return string.Format(provider, template, [.. dicIndex.Values]);
    }

    /// <summary>
    /// Remove a sign, such as an accent or cedill.
    /// <para>By Blair Conrad</para>
    /// See also <a href="https://stackoverflow.com/questions/249087/how-do-i-remove-diacritics-accents-from-a-string-in-net">here</a>.
    /// </summary>
    /// <param name="value">The text with sign, accent or cedill to remove.</param>
    /// <returns></returns>
    public static string? RemoveDiacritics(this string? value)
    {
        if (string.IsNullOrEmpty(value)) return value;

        var normalizedString = value.Normalize(NormalizationForm.FormD);
        StringBuilder stringBuilder = new();

        foreach (var c in normalizedString)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                stringBuilder.Append(c);
        }

        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }

    /// <summary>
    /// Removes the special characters from text.
    /// </summary>
    /// <param name="value">Text content.</param>
    /// <returns></returns>
    public static string? RemoveSpecialCharacters(this string? value)
    {
        if (string.IsNullOrEmpty(value)) return value;

        StringBuilder sb = new();
        foreach (char c in value)
        {
            if ((c >= '0' && c <= '9')
                || (c >= 'A' && c <= 'Z')
                || (c >= 'a' && c <= 'z'))
                sb.Append(c);
        }
        return sb.ToString();
    }

    /// <summary>
    /// Format the string to pretty-format Json.
    /// </summary>
    /// <param name="value">Json content.</param>
    /// <returns></returns>
    public static string FormatJson(this string value)
    {
        const string INDENT_STRING = "    ";

        int indentation = 0;
        int quoteCount = 0;
        var result =
            from ch in value
            let quotes = ch == '"' ? quoteCount++ : quoteCount
            let lineBreak = ch == ',' && quotes % 2 == 0 ? ch + Environment.NewLine + String.Concat(Enumerable.Repeat(INDENT_STRING, indentation)) : null
            let openChar = ch == '{' || ch == '[' ? ch + Environment.NewLine + String.Concat(Enumerable.Repeat(INDENT_STRING, ++indentation)) : ch.ToString()
            let closeChar = ch == '}' || ch == ']' ? Environment.NewLine + String.Concat(Enumerable.Repeat(INDENT_STRING, --indentation)) + ch : ch.ToString()
            select lineBreak ?? (openChar.Length > 1 ? openChar : closeChar);

        return string.Concat(result);
    }

    /// <summary>
    /// Checks whether the text is a valid Json.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsValidJson(this string value)
    {
        if (string.IsNullOrWhiteSpace(value)) { return false; }
        value = value.Trim();

        if ((!value.StartsWith("{") || !value.EndsWith("}")) && //For object
            (!value.StartsWith("[") || !value.EndsWith("]"))) //For array
            return false;

        try
        {
            var obj = JsonDocument.Parse(value);
            return true;
        }
        catch (JsonException jex)
        {
            //Exception in parsing json
            Console.WriteLine(jex.Message);
            return false;
        }
        catch (Exception ex) //some other exception
        {
            Console.WriteLine(ex.ToString());
            return false;
        }
    }

    /// <summary>
    /// Extracts only the numeric digits from a string, removing all non-digit characters.
    /// </summary>
    /// <param name="value">The string to process.</param>
    /// <returns>A string containing only the numeric digits from the input.</returns>
    public static string GetNumbersOnly(this string value)
    {
        return NonDigitRegex().Replace(value, string.Empty);
    }

    [GeneratedRegex(@"\D", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex NonDigitRegex();
}