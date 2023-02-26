namespace NuvTools.Common.Numbers;

internal static class NumbersHelper
{
    public static long DivRem(long number, long divisor, out long remainder)
    {
        var result = number / divisor;
        remainder = number % divisor;
        return result;
    }

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
