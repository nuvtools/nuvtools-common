namespace NuvTools.Common.Numbers;

/// <summary>
/// Provides internal helper methods for numeric operations.
/// </summary>
internal static class NumbersHelper
{
    /// <summary>
    /// Performs division and returns both the quotient and remainder.
    /// </summary>
    /// <param name="number">The dividend.</param>
    /// <param name="divisor">The divisor.</param>
    /// <param name="remainder">The remainder of the division operation.</param>
    /// <returns>The quotient of the division operation.</returns>
    public static long DivRem(long number, long divisor, out long remainder)
    {
        var result = number / divisor;
        remainder = number % divisor;
        return result;
    }

    /// <summary>
    /// Removes consecutive spaces from a string, replacing them with a single space.
    /// </summary>
    /// <param name="value">The string to clean up.</param>
    /// <returns>A string with consecutive spaces removed and leading/trailing spaces trimmed.</returns>
    public static string CleanupSpaces(string value)
    {
        string result = string.Empty;
        bool booUltIs32 = false;

        foreach (char character in value)
        {
            if (character != 32)
            {
                result += character;
                booUltIs32 = false;
            }
            else if (!booUltIs32)
            {
                result += character;
                booUltIs32 = true;
            }
        }

        return result.Trim();
    }
}
