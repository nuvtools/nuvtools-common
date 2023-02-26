namespace NuvTools.Common.Numbers.Portuguese;


/// <summary>
/// Extensions's class to convert numbers to words.
/// </summary>
public static class NumberToWordsExtensions
{

    public static string ToWords(this decimal value)
    {
        return NumberToWords.ToWords(value);
    }

    public static string ToWords(this int value)
    {
        return NumberToWords.ToWords(value);
    }

    

}