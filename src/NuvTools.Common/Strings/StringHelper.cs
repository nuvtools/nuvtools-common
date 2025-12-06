namespace NuvTools.Common.Strings;

/// <summary>
/// Provides utility methods for string operations.
/// </summary>
public class StringHelper
{
    private static readonly Random random = new();

    /// <summary>
    /// Generates a random alphanumeric string of the specified length.
    /// </summary>
    /// <param name="length">The desired length of the random string.</param>
    /// <returns>A random string consisting of uppercase letters (A-Z) and digits (0-9).</returns>
    /// <remarks>
    /// This method uses uppercase letters and digits only. The generated string is suitable
    /// for generating simple identifiers or test data.
    /// </remarks>
    public static string RandomAlphaNumericString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
          .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
