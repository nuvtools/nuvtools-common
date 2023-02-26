using NuvTools.Common.Enums;
using System.ComponentModel;
using System.Globalization;

namespace NuvTools.Common.Dates;

public enum TimeZoneRegion
{
    /// <summary>
    /// Time zone GMT-03:00.
    /// <list type="bullet">
    /// <item>Brasilia (E. South America Standard Time)</item>
    /// <item>Buenos Aires (Argentina Standard Time)</item>
    /// <item>Georgetown (SA Eastern Standard Time)</item>
    /// <item>Greenland (Greenland Standard Time)</item>
    /// <item>Montevideo (Montevideo Standard Time)</item>
    /// </list>
    /// </summary>
    [Description("E. South America Standard Time")]
    Brasilia,

    /// <summary>
    /// Time zone GMT-04:00.
    /// <list type="bullet">
    /// <item>Atlantic Time - Canada (Atlantic Standard Time)</item>
    /// <item>La Paz (SA Western Standard Time)</item>
    /// <item>Manaus (Central Brazilian Standard Time)</item>
    /// <item>Greenland (Greenland Standard Time)</item>
    /// <item>Santiago (Pacific SA Standard Time)</item>
    /// </list>
    /// </summary>
    [Description("Pacific SA Standard Time")]
    Santiago,

    /// <summary>
    /// Time zone GMT-05:00.
    /// <list type="bullet">
    /// <item>Bogota, Lima, Quito, Rio Branco (SA Pacific Standard Time)</item>
    /// <item>Eastern Time (US and Canada) (Eastern Standard Time)</item>
    /// <item>Indiana (East) (Central Standard Time (Mexico))</item>
    /// </list>
    /// </summary>
    [Description("Eastern Standard Time")]
    EasternTimeUSCanada,

    /// <summary>
    /// Time zone GMT-06:00.
    /// <list type="bullet">
    /// <item>Central America (Central America Standard Time)</item>
    /// <item>Central Time (US and Canada) (Central Standard Time)</item>
    /// <item>Guadalajara, Mexico City, Monterrey (Central Standard Time (Mexico))</item>
    /// <item>Saskatchewan (Canada Central Standard Time)</item>
    /// </list>
    /// </summary>
    [Description("Central Standard Time")]
    CentralStandardTime
}

public enum UtcDirection
{
    None,
    FromUtc,
    ToUtc
}

public static class TimeZoneExtensions
{

    /// <summary>
    /// Converts a date and time to the specified time zone.
    /// </summary>
    /// <param name="value">Date and time to be converted.</param>
    /// <param name="timeZoneRegion">Region which value will be converted.</param>
    /// <param name="utcDirection">Conversion direction.</param>
    /// <returns></returns>
    public static DateTime ToTimeZone(this DateTime value,
                                                TimeZoneRegion timeZoneRegion = TimeZoneRegion.Brasilia,
                                                UtcDirection utcDirection = UtcDirection.None)
    {
        TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneRegion.GetDescription());

        switch (utcDirection)
        {
            case UtcDirection.FromUtc:
                if (value.Kind == DateTimeKind.Local)
                    return value.ToTimeZone(timeZoneRegion);

                return TimeZoneInfo.ConvertTimeFromUtc(value, timeZoneInfo);
            case UtcDirection.ToUtc:
                return TimeZoneInfo.ConvertTimeToUtc(value, timeZoneInfo);
            default:
                return TimeZoneInfo.ConvertTime(value, timeZoneInfo);
        }
    }

    /// <summary>
    /// Parses the string to DateTime using TimeZoneRegion options
    /// </summary>
    /// <param name="value">Date and time string to be converted into DateTIme.</param>
    /// <param name="format">Format that the value is being passed.</param>
    /// <param name="fromTimeZoneRegion">Region which value will be converted.</param>
    /// <returns></returns>
    public static DateTime DateParseToUtc(this string value, string format = "dd-MM-yyyy HH:mm:ss", TimeZoneRegion fromTimeZoneRegion = TimeZoneRegion.Brasilia)
    {
        return DateTime.ParseExact(value, format, CultureInfo.InvariantCulture).ToTimeZone(fromTimeZoneRegion, UtcDirection.ToUtc);
    }
}