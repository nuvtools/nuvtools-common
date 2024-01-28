using NuvTools.Common.Resources;
using System.Globalization;

namespace NuvTools.Common.Numbers.Portuguese;


/// <summary>
/// Extensions's class to convert numbers to words.
/// </summary>
public static class NumberToWords
{
    private static readonly string[] units = {
                                                       string.Empty, "um", "dois", "três", "quatro", "cinco", "seis", "sete",
                                                       "oito", "nove", "dez", "onze", "doze", "treze", "quatorze",
                                                       "quinze", "dezesseis", "dezessete", "dezoito", "dezenove"
                                                   };

    private static readonly string[] dozen = {
                                                      string.Empty, "dez", "vinte", "trinta", "quarenta", "cinquenta", "sessenta"
                                                      , "setenta", "oitenta", "noventa"
                                                  };


    private static readonly string[] hundreds = {
                                                       string.Empty, "cem", "duzentos", "trezentos", "quatrocentos", "quinhentos"
                                                       , "seiscentos", "setecentos", "oitocentos", "novecentos"
                                                   };


    private const decimal minValue = 0.01M;
    private const decimal maxValue = 999999999999999.99M;
    private const string currency = " real ";
    private const string currencyPlural = " reais ";
    private const string hundredth = " centavo ";
    private const string hundredthPlural = " centavos ";

    private static string Convert(long value)
    {
        long result;

        if (value >= 1 && value <= 19)
            return units[value];

        result = NumbersHelper.DivRem(value, 10, out long remainder);

        if (value == 20 || value == 30 || value == 40 ||
            value == 50 || value == 60 || value == 70 ||
            value == 80 || value == 90)
            return dozen[result] + " ";

        if (value >= 21 && value <= 29 ||
            value >= 31 && value <= 39 ||
            value >= 41 && value <= 49 ||
            value >= 51 && value <= 59 ||
            value >= 61 && value <= 69 ||
            value >= 71 && value <= 79 ||
            value >= 81 && value <= 89 ||
            value >= 91 && value <= 99)
            return dozen[result] + " e " + Convert(remainder);

        result = NumbersHelper.DivRem(value, 100, out remainder);

        if (value == 100 || value == 200 || value == 300 ||
            value == 400 || value == 500 || value == 600 ||
            value == 700 || value == 800 || value == 900)
            return hundreds[result] + ", ";

        if (value >= 101 && value <= 199)
            return " cento e " + Convert(remainder);

        if (value >= 201 && value <= 299 ||
            value >= 301 && value <= 399 ||
            value >= 401 && value <= 499 ||
            value >= 501 && value <= 599 ||
            value >= 601 && value <= 699 ||
            value >= 701 && value <= 799 ||
            value >= 801 && value <= 899 ||
            value >= 901 && value <= 999)
            return hundreds[result] + " e " +
                   Convert(remainder);

        result = NumbersHelper.DivRem(value, 1000, out remainder);

        if (value >= 1000 && value <= 999999)
            return Convert(result) + " mil " +
                   Convert(remainder);

        result = NumbersHelper.DivRem(value, 1000000, out remainder);

        if (value >= 1000000 && value <= 1999999)
            return Convert(result) + " milhão " +
                   Convert(remainder);

        if (value >= 2000000 && value <= 999999999)
            return Convert(result) + " milhões " +
                   Convert(remainder);

        result = NumbersHelper.DivRem(value, 1000000000, out remainder);

        if (value >= 1000000000 && value <= 1999999999)
            return Convert(result) + " bilhão " +
                   Convert(remainder);

        if (value >= 2000000000 && value <= 999999999999)
            return Convert(result) + " bilhões " +
                   Convert(remainder);

        result = NumbersHelper.DivRem(value, 1000000000000, out remainder);

        if (value >= 1000000000000 && value <= 1999999999999)
            return Convert(result) + " trilhão " +
                   Convert(remainder);

        if (value >= 2000000000000 && value <= 999999999999999)
            return Convert(result) + " trilhões " +
                   Convert(remainder);

        return string.Empty;
    }

    public static string ToWords(decimal value)
    {
        string result = string.Empty;

        if (value >= minValue && value <= maxValue)
        {
            long intPart = System.Convert.ToInt64(Math.Truncate(value));
            long decimalPart = System.Convert.ToInt64(Math.Truncate((value - Math.Truncate(value)) * 100));

            result += Convert(intPart) + (intPart <= 1 ? currency : currencyPlural);

            if (decimalPart > 0)
                result += " e " + Convert(decimalPart) + (decimalPart == 1 ? hundredth : hundredthPlural);
        }
        else
            throw new Exception(Messages.ResourceManager.GetString(nameof(Messages.ValueOutsideRange), CultureInfo.GetCultureInfo("pt-BR")));

        return NumbersHelper.CleanupSpaces(result);
    }
}