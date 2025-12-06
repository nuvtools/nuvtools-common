namespace NuvTools.Common.Numbers.Portuguese;


/// <summary>
/// Extension methods to convert numbers to words in Portuguese.
/// </summary>
public static class NumberToWordsExtensions
{
    /// <summary>
    /// Converts a decimal value to its written form in Portuguese (Brazil).
    /// </summary>
    /// <param name="value">The decimal value to convert.</param>
    /// <returns>The value written out in Portuguese with currency (reais/centavos).</returns>
    public static string ToWords(this decimal value)
    {
        return NumberToWords.ToWords(value);
    }

    /// <summary>
    /// Converts an integer value to its written form in Portuguese (Brazil).
    /// </summary>
    /// <param name="value">The integer value to convert.</param>
    /// <returns>The value written out in Portuguese with currency (reais).</returns>
    public static string ToWords(this int value)
    {
        return NumberToWords.ToWords(value);
    }

    

}