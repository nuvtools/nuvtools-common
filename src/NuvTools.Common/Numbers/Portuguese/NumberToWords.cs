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
                                                      string.Empty, "dez", "vinte", "trinta", "quarenta", "cinqüenta", "sessenta"
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
    private const string centesimo = " centavo ";
    private const string centesimoPlural = " centavos ";

    private static string Converter(long value)
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
            return dozen[result] + " e " + Converter(remainder);

        result = NumbersHelper.DivRem(value, 100, out remainder);

        if (value == 100 || value == 200 || value == 300 ||
            value == 400 || value == 500 || value == 600 ||
            value == 700 || value == 800 || value == 900)
            return hundreds[result] + ", ";

        if (value >= 101 && value <= 199)
            return " cento e " + Converter(remainder);

        if (value >= 201 && value <= 299 ||
            value >= 301 && value <= 399 ||
            value >= 401 && value <= 499 ||
            value >= 501 && value <= 599 ||
            value >= 601 && value <= 699 ||
            value >= 701 && value <= 799 ||
            value >= 801 && value <= 899 ||
            value >= 901 && value <= 999)
            return hundreds[result] + " e " +
                   Converter(remainder);

        result = NumbersHelper.DivRem(value, 1000, out remainder);

        if (value >= 1000 && value <= 999999)
            return Converter(result) + " mil " +
                   Converter(remainder);

        result = NumbersHelper.DivRem(value, 1000000, out remainder);

        if (value >= 1000000 && value <= 1999999)
            return Converter(result) + " milhão " +
                   Converter(remainder);

        if (value >= 2000000 && value <= 999999999)
            return Converter(result) + " milhões " +
                   Converter(remainder);

        result = NumbersHelper.DivRem(value, 1000000000, out remainder);

        if (value >= 1000000000 && value <= 1999999999)
            return Converter(result) + " bilhão " +
                   Converter(remainder);

        if (value >= 2000000000 && value <= 999999999999)
            return Converter(result) + " bilhões " +
                   Converter(remainder);

        result = NumbersHelper.DivRem(value, 1000000000000, out remainder);

        if (value >= 1000000000000 && value <= 1999999999999)
            return Converter(result) + " trilhão " +
                   Converter(remainder);

        if (value >= 2000000000000 && value <= 999999999999999)
            return Converter(result) + " trilhões " +
                   Converter(remainder);

        return string.Empty;
    }

    public static string ToWords(decimal value)
    {
        string texto = string.Empty;

        if (value >= minValue && value <= maxValue)
        {
            long inteiro = Convert.ToInt64(Math.Truncate(value));
            long centavos = Convert.ToInt64(Math.Truncate((value - Math.Truncate(value)) * 100));

            texto += Converter(inteiro) + (inteiro <= 1 ? currency : currencyPlural);

            if (centavos > 0)
                texto += " e " + Converter(centavos) + (centavos == 1 ? centesimo : centesimoPlural);
        }
        else
            throw new Exception(Messages.ResourceManager.GetString(nameof(Messages.ValueOutsideRange), CultureInfo.GetCultureInfo("pt-BR")));

        return NumbersHelper.CleanupSpaces(texto);
    }
}