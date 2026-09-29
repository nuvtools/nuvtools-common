using System.Globalization;
using System.Numerics;

namespace NuvTools.Common.Numbers;

/// <summary>
/// Provides extension methods for parsing strings to numeric types with null-safe handling.
/// </summary>
/// <remarks>
/// The overloads without an <see cref="IFormatProvider"/> read the text in the <b>current culture</b>.
/// That decides the decimal separator: on a pt-BR machine <c>"3.14"</c> is read as 314. Text that
/// does not come from the user — a file, a database column, an API — is written in a fixed format,
/// so parse it with the overload that takes a provider, usually <see cref="CultureInfo.InvariantCulture"/>.
/// </remarks>
public static class NumbersExtensions
{
    /// <summary>
    /// Parses the string to long or null, in the current culture.
    /// </summary>
    /// <param name="value">Value to parsed.</param>
    /// <param name="returnZeroIsNull">Returns zero instead of null when the value is empty or not a number.</param>
    /// <returns>Returns long or null.</returns>
    public static long? ParseToLongOrNull(this string? value, bool returnZeroIsNull = false) =>
        Parse<long>(value, NumberStyles.Integer, null, returnZeroIsNull);

    /// <summary>
    /// Parses the string to long or null, in the format <paramref name="provider"/> describes.
    /// </summary>
    /// <param name="value">Value to parsed.</param>
    /// <param name="provider">The culture the text is written in, for example <see cref="CultureInfo.InvariantCulture"/>.</param>
    /// <param name="returnZeroIsNull">Returns zero instead of null when the value is empty or not a number.</param>
    /// <returns>Returns long or null.</returns>
    public static long? ParseToLongOrNull(this string? value, IFormatProvider? provider, bool returnZeroIsNull = false) =>
        Parse<long>(value, NumberStyles.Integer, provider, returnZeroIsNull);

    /// <summary>
    /// Parses the string to int or null, in the current culture.
    /// </summary>
    /// <param name="value">Value to parsed.</param>
    /// <param name="returnZeroIsNull">Returns zero instead of null when the value is empty or not a number.</param>
    /// <returns>Returns int or null.</returns>
    public static int? ParseToIntOrNull(this string? value, bool returnZeroIsNull = false) =>
        Parse<int>(value, NumberStyles.Integer, null, returnZeroIsNull);

    /// <summary>
    /// Parses the string to int or null, in the format <paramref name="provider"/> describes.
    /// </summary>
    /// <param name="value">Value to parsed.</param>
    /// <param name="provider">The culture the text is written in, for example <see cref="CultureInfo.InvariantCulture"/>.</param>
    /// <param name="returnZeroIsNull">Returns zero instead of null when the value is empty or not a number.</param>
    /// <returns>Returns int or null.</returns>
    public static int? ParseToIntOrNull(this string? value, IFormatProvider? provider, bool returnZeroIsNull = false) =>
        Parse<int>(value, NumberStyles.Integer, provider, returnZeroIsNull);

    /// <summary>
    /// Parses the string to short or null, in the current culture.
    /// </summary>
    /// <param name="value">Value to parsed.</param>
    /// <param name="returnZeroIsNull">Returns zero instead of null when the value is empty or not a number.</param>
    /// <returns>Returns short or null.</returns>
    public static short? ParseToShortOrNull(this string? value, bool returnZeroIsNull = false) =>
        Parse<short>(value, NumberStyles.Integer, null, returnZeroIsNull);

    /// <summary>
    /// Parses the string to short or null, in the format <paramref name="provider"/> describes.
    /// </summary>
    /// <param name="value">Value to parsed.</param>
    /// <param name="provider">The culture the text is written in, for example <see cref="CultureInfo.InvariantCulture"/>.</param>
    /// <param name="returnZeroIsNull">Returns zero instead of null when the value is empty or not a number.</param>
    /// <returns>Returns short or null.</returns>
    public static short? ParseToShortOrNull(this string? value, IFormatProvider? provider, bool returnZeroIsNull = false) =>
        Parse<short>(value, NumberStyles.Integer, provider, returnZeroIsNull);

    /// <summary>
    /// Parses the string to decimal or null, in the current culture — see the class remarks before
    /// using it on text that did not come from the user.
    /// </summary>
    /// <param name="value">Value to parsed.</param>
    /// <param name="returnZeroIsNull">Returns zero instead of null when the value is empty or not a number.</param>
    /// <returns>Returns decimal or null.</returns>
    public static decimal? ParseToDecimalOrNull(this string? value, bool returnZeroIsNull = false) =>
        Parse<decimal>(value, NumberStyles.Number, null, returnZeroIsNull);

    /// <summary>
    /// Parses the string to decimal or null, in the format <paramref name="provider"/> describes.
    /// </summary>
    /// <param name="value">Value to parsed.</param>
    /// <param name="provider">The culture the text is written in, for example <see cref="CultureInfo.InvariantCulture"/>.</param>
    /// <param name="returnZeroIsNull">Returns zero instead of null when the value is empty or not a number.</param>
    /// <returns>Returns decimal or null.</returns>
    public static decimal? ParseToDecimalOrNull(this string? value, IFormatProvider? provider, bool returnZeroIsNull = false) =>
        Parse<decimal>(value, NumberStyles.Number, provider, returnZeroIsNull);

    /// <summary>
    /// Parses the string to double or null, in the current culture — see the class remarks before
    /// using it on text that did not come from the user.
    /// </summary>
    /// <param name="value">Value to parsed.</param>
    /// <param name="returnZeroIsNull">Returns zero instead of null when the value is empty or not a number.</param>
    /// <returns>Returns double or null.</returns>
    public static double? ParseToDoubleOrNull(this string? value, bool returnZeroIsNull = false) =>
        Parse<double>(value, NumberStyles.Float | NumberStyles.AllowThousands, null, returnZeroIsNull);

    /// <summary>
    /// Parses the string to double or null, in the format <paramref name="provider"/> describes.
    /// </summary>
    /// <param name="value">Value to parsed.</param>
    /// <param name="provider">The culture the text is written in, for example <see cref="CultureInfo.InvariantCulture"/>.</param>
    /// <param name="returnZeroIsNull">Returns zero instead of null when the value is empty or not a number.</param>
    /// <returns>Returns double or null.</returns>
    public static double? ParseToDoubleOrNull(this string? value, IFormatProvider? provider, bool returnZeroIsNull = false) =>
        Parse<double>(value, NumberStyles.Float | NumberStyles.AllowThousands, provider, returnZeroIsNull);

    /// <remarks>
    /// Each caller passes the styles its type's own <c>TryParse(string, out T)</c> uses, so the
    /// overloads without a provider behave exactly as they did before the provider was added.
    /// </remarks>
    private static T? Parse<T>(string? value, NumberStyles style, IFormatProvider? provider, bool returnZeroIsNull)
        where T : struct, INumberBase<T>
    {
        if (string.IsNullOrEmpty(value)
            || !T.TryParse(value, style, provider, out var result)) return returnZeroIsNull ? T.Zero : null;
        return result;
    }
}
