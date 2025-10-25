namespace NuvTools.Common.Dates.Enumerations;

/// <summary>
/// Represents well-known regional time zones supported by NuvTools for
/// cross-platform date and time conversions.
/// <para>
/// Each value maps to both a Windows and an IANA time zone identifier,
/// ensuring consistent behavior across Windows, macOS, and Linux.
/// </para>
/// </summary>
/// <remarks>
/// This enumeration is primarily used by <see cref="IDateTimeService"/> and
/// <see cref="TimeZoneExtensions"/> to standardize time zone conversions.
/// </remarks>
public enum TimeZoneRegion
{
    /// <summary>
    /// Coordinated Universal Time.
    /// <para>
    /// Windows ID: <c>UTC</c><br/>
    /// IANA ID: <c>Etc/UTC</c>
    /// </para>
    /// </summary>
    Utc,

    /// <summary>
    /// (UTC−08:00) Pacific Time (US and Canada).
    /// <para>
    /// Windows ID: <c>Pacific Standard Time</c><br/>
    /// IANA ID: <c>America/Los_Angeles</c>
    /// </para>
    /// </summary>
    PacificTimeUSCanada,

    /// <summary>
    /// (UTC−07:00) Mountain Time (US and Canada).
    /// <para>
    /// Windows ID: <c>Mountain Standard Time</c><br/>
    /// IANA ID: <c>America/Denver</c>
    /// </para>
    /// </summary>
    MountainTimeUSCanada,

    /// <summary>
    /// (UTC−06:00) Central Time (US and Canada), Mexico City, Monterrey.
    /// <para>
    /// Windows ID: <c>Central Standard Time</c><br/>
    /// IANA ID: <c>America/Chicago</c>
    /// </para>
    /// </summary>
    CentralStandardTime,

    /// <summary>
    /// (UTC−05:00) Eastern Time (US and Canada), Bogotá, Lima, Quito.
    /// <para>
    /// Windows ID: <c>Eastern Standard Time</c><br/>
    /// IANA ID: <c>America/New_York</c>
    /// </para>
    /// </summary>
    EasternTimeUSCanada,

    /// <summary>
    /// (UTC−04:00) Santiago, Manaus, La Paz.
    /// <para>
    /// Windows ID: <c>Pacific SA Standard Time</c><br/>
    /// IANA ID: <c>America/Santiago</c>
    /// </para>
    /// </summary>
    Santiago,

    /// <summary>
    /// (UTC−03:00) Brasília, Buenos Aires, Montevideo.
    /// <para>
    /// Windows ID: <c>E. South America Standard Time</c><br/>
    /// IANA ID: <c>America/Sao_Paulo</c>
    /// </para>
    /// </summary>
    Brasilia,

    /// <summary>
    /// (UTC−02:00) Mid-Atlantic, South Georgia.
    /// <para>
    /// Windows ID: <c>Mid-Atlantic Standard Time</c><br/>
    /// IANA ID: <c>Atlantic/South_Georgia</c>
    /// </para>
    /// </summary>
    MidAtlantic,

    /// <summary>
    /// (UTC−01:00) Azores, Cape Verde Islands.
    /// <para>
    /// Windows ID: <c>Azores Standard Time</c><br/>
    /// IANA ID: <c>Atlantic/Azores</c>
    /// </para>
    /// </summary>
    Azores,


    /// <summary>
    /// (UTC+00:00) Dublin, Edinburgh, Lisbon, London.
    /// <para>
    /// Windows ID: <c>GMT Standard Time</c><br/>
    /// IANA ID: <c>Europe/London</c>
    /// </para>
    /// </summary>
    London,

    /// <summary>
    /// (UTC+01:00) Amsterdam, Berlin, Rome, Paris, Madrid.
    /// <para>
    /// Windows ID: <c>W. Europe Standard Time</c><br/>
    /// IANA ID: <c>Europe/Berlin</c>
    /// </para>
    /// </summary>
    CentralEurope,

    /// <summary>
    /// (UTC+02:00) Athens, Bucharest, Istanbul, Cairo.
    /// <para>
    /// Windows ID: <c>GTB Standard Time</c><br/>
    /// IANA ID: <c>Europe/Athens</c>
    /// </para>
    /// </summary>
    EasternEurope,

    /// <summary>
    /// (UTC+03:00) Moscow, Nairobi, Riyadh.
    /// <para>
    /// Windows ID: <c>Russian Standard Time</c><br/>
    /// IANA ID: <c>Europe/Moscow</c>
    /// </para>
    /// </summary>
    Moscow,

    /// <summary>
    /// (UTC+04:00) Dubai, Baku, Samara.
    /// <para>
    /// Windows ID: <c>Arabian Standard Time</c><br/>
    /// IANA ID: <c>Asia/Dubai</c>
    /// </para>
    /// </summary>
    Dubai,

    /// <summary>
    /// (UTC+05:30) Chennai, Kolkata, Mumbai, New Delhi.
    /// <para>
    /// Windows ID: <c>India Standard Time</c><br/>
    /// IANA ID: <c>Asia/Kolkata</c>
    /// </para>
    /// </summary>
    India,

    /// <summary>
    /// (UTC+07:00) Bangkok, Hanoi, Jakarta.
    /// <para>
    /// Windows ID: <c>SE Asia Standard Time</c><br/>
    /// IANA ID: <c>Asia/Bangkok</c>
    /// </para>
    /// </summary>
    SoutheastAsia,

    /// <summary>
    /// (UTC+08:00) Beijing, Hong Kong, Kuala Lumpur, Singapore, Taipei, Perth.
    /// <para>
    /// Windows ID: <c>China Standard Time</c><br/>
    /// IANA ID: <c>Asia/Shanghai</c>
    /// </para>
    /// </summary>
    China,

    /// <summary>
    /// (UTC+09:00) Osaka, Sapporo, Tokyo, Seoul.
    /// <para>
    /// Windows ID: <c>Tokyo Standard Time</c><br/>
    /// IANA ID: <c>Asia/Tokyo</c>
    /// </para>
    /// </summary>
    Japan,

    /// <summary>
    /// (UTC+10:00) Sydney, Canberra, Melbourne.
    /// <para>
    /// Windows ID: <c>AUS Eastern Standard Time</c><br/>
    /// IANA ID: <c>Australia/Sydney</c>
    /// </para>
    /// </summary>
    Sydney,

    /// <summary>
    /// (UTC+12:00) Auckland, Wellington, Fiji.
    /// <para>
    /// Windows ID: <c>New Zealand Standard Time</c><br/>
    /// IANA ID: <c>Pacific/Auckland</c>
    /// </para>
    /// </summary>
    NewZealand
}