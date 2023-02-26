using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Text;

namespace NuvTools.Common.Strings;

public static class StringExtensions
{
    public static string Left(this string value, int length)
    {
        if (string.IsNullOrEmpty(value)) throw new ArgumentNullException(nameof(value));

        length = Math.Abs(length);

        return (value.Length <= length
               ? value
               : value.Substring(0, length)
               );
    }

    public static string Right(this string value, int length)
    {
        if (string.IsNullOrEmpty(value)) throw new ArgumentNullException(nameof(value));

        length = Math.Abs(length);

        return (value.Length <= length
               ? value
               : value.Substring(value.Length - length, length)
               );
    }

    public static string Format(this string format, params string[] values)
    {
        if (string.IsNullOrEmpty(format)) throw new ArgumentNullException(nameof(format));

        return string.Format(format, values);
    }


    /// <summary>
    /// Remove a sign, such as an accent or cedill.
    /// <para>By Blair Conrad</para>
    /// See also <a href="https://stackoverflow.com/questions/249087/how-do-i-remove-diacritics-accents-from-a-string-in-net">here</a>.
    /// </summary>
    /// <param name="value">The text with sign, accent or cedill to remove.</param>
    /// <returns></returns>
    public static string RemoveDiacritics(this string value)
    {
        var normalizedString = value.Normalize(NormalizationForm.FormD);
        StringBuilder stringBuilder = new();

        foreach (var c in normalizedString)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }

        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }

    /// <summary>
    /// Removes the special characters from text.
    /// </summary>
    /// <param name="value">Text content.</param>
    /// <returns></returns>
    public static string RemoveSpecialCharacters(this string value)
    {
        StringBuilder sb = new();
        foreach (char c in value)
        {
            if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'))
            {
                sb.Append(c);
            }
        }
        return sb.ToString();
    }

    /// <summary>
    /// Format the string to Json pretty-format.
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
            select lineBreak == null
                        ? openChar.Length > 1
                            ? openChar
                            : closeChar
                        : lineBreak;

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
        if ((value.StartsWith("{") && value.EndsWith("}")) || //For object
            (value.StartsWith("[") && value.EndsWith("]"))) //For array
        {
            try
            {
                var obj = JToken.Parse(value);
                return true;
            }
            catch (JsonReaderException jex)
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
        else
        {
            return false;
        }
    }
}