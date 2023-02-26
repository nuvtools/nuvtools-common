namespace NuvTools.Common.Numbers;

public static class NumbersExtensions
{
    /// <summary>
    /// Parses the string to long or null.
    /// </summary>
    /// <param name="value">Value to parsed.</param>
    /// <param name="returnZeroIsNull">Returns zero whether the value is null.</param>
    /// <returns>Returns long or null.</returns>
    public static long? ParseToLongOrNull(this string value, bool returnZeroIsNull = false)
    {
        if (string.IsNullOrEmpty(value)
            || !long.TryParse(value, out long result)) return returnZeroIsNull ? 0 : null;
        return result;
    }

    /// <summary>
    /// Parses the string to int or null.
    /// </summary>
    /// <param name="value">Value to parsed.</param>
    /// <param name="returnZeroIsNull">Returns zero whether the value is null.</param>
    /// <returns>Returns int or null.</returns>
    public static int? ParseToIntOrNull(this string value, bool returnZeroIsNull = false)
    {
        if (string.IsNullOrEmpty(value)
            || !int.TryParse(value, out int result)) return returnZeroIsNull ? 0 : null;
        return result;
    }

    /// <summary>
    /// Parses the string to short or null.
    /// </summary>
    /// <param name="value">Value to parsed.</param>
    /// <param name="returnZeroIsNull">Returns zero whether the value is null.</param>
    /// <returns>Returns long or null.</returns>
    public static short? ParseToShortOrNull(this string value, bool returnZeroIsNull = false)
    {
        if (string.IsNullOrEmpty(value)
            || !short.TryParse(value, out short result)) return returnZeroIsNull ? 0 : null;
        return result;
    }

    /// <summary>
    /// Parses the string to decimal or null.
    /// </summary>
    /// <param name="value">Value to parsed.</param>
    /// <param name="returnZeroIsNull">Returns zero whether the value is null.</param>
    /// <returns>Returns decimal or null.</returns>
    public static decimal? ParseToDecimalOrNull(this string value, bool returnZeroIsNull = false)
    {
        if (string.IsNullOrEmpty(value)
            || !decimal.TryParse(value, out decimal result)) return returnZeroIsNull ? 0 : null;
        return result;
    }
}
