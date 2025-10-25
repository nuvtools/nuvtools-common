using NuvTools.Common.Dates.Enumerations;
using System.Globalization;
using System.Runtime.InteropServices;

namespace NuvTools.Common.Dates;

/// <summary>
/// Provides helper methods for timezone conversions.
/// </summary>
public static class TimeZoneExtensions
{
    /// <summary>
    /// Converts a <see cref="DateTime"/> to the specified timezone or offset.
    /// </summary>
    /// <param name="value">The input datetime.</param>
    /// <param name="timeZoneRegion">The target region (optional if using offset).</param>
    /// <param name="offsetMinutes">Manual offset in minutes (optional).</param>
    /// <param name="utcDirection">Whether to convert to or from UTC.</param>
    public static DateTime ToTimeZone(
        this DateTime value,
        TimeZoneRegion timeZoneRegion = TimeZoneRegion.Brasilia,
        UtcDirection utcDirection = UtcDirection.None, int? offsetMinutes = null)
    {
        // Determine timezone info: region → offset → UTC fallback
        TimeZoneInfo timeZoneInfo = offsetMinutes.HasValue
            ? CreateTimeZoneFromOffset(offsetMinutes.Value)
            : timeZoneRegion.GetTimeZoneInfo();

        return utcDirection switch
        {
            UtcDirection.FromUtc => value.Kind == DateTimeKind.Local
                ? value.ToTimeZone(timeZoneRegion, offsetMinutes: offsetMinutes)
                : TimeZoneInfo.ConvertTimeFromUtc(value, timeZoneInfo),

            UtcDirection.ToUtc => TimeZoneInfo.ConvertTimeToUtc(value, timeZoneInfo),

            _ => TimeZoneInfo.ConvertTime(value, timeZoneInfo)
        };
    }

    /// <summary>
    /// Converts a string date to UTC <see cref="DateTime"/> using either a region or a custom offset.
    /// </summary>
    public static DateTime DateParseToUtc(
        this string value,
        string format = "dd-MM-yyyy HH:mm:ss",
        TimeZoneRegion fromTimeZoneRegion = TimeZoneRegion.Brasilia,
        int? offsetMinutes = null)
    {
        var parsed = DateTime.ParseExact(value, format, CultureInfo.InvariantCulture);
        return parsed.ToTimeZone(fromTimeZoneRegion, UtcDirection.ToUtc, offsetMinutes);
    }

    /// <summary>
    /// Converts a DateTimeOffset to the specified timezone region or offset.
    /// </summary>
    public static DateTimeOffset ToTimeZoneOffset(
        this DateTimeOffset value,
        TimeZoneRegion timeZoneRegion = TimeZoneRegion.Brasilia,
        int? offsetMinutes = null)
    {
        var targetZone = offsetMinutes.HasValue
            ? CreateTimeZoneFromOffset(offsetMinutes.Value)
            : timeZoneRegion.GetTimeZoneInfo();

        return TimeZoneInfo.ConvertTime(value, targetZone);
    }

    private static readonly Dictionary<TimeZoneRegion, (string WindowsId, string IanaId)> _timeZoneMap = new()
    {
        { TimeZoneRegion.Utc, ("UTC", "Etc/UTC") },
        { TimeZoneRegion.PacificTimeUSCanada, ("Pacific Standard Time", "America/Los_Angeles") },
        { TimeZoneRegion.MountainTimeUSCanada, ("Mountain Standard Time", "America/Denver") },
        { TimeZoneRegion.CentralStandardTime, ("Central Standard Time", "America/Chicago") },
        { TimeZoneRegion.EasternTimeUSCanada, ("Eastern Standard Time", "America/New_York") },
        { TimeZoneRegion.Santiago, ("Pacific SA Standard Time", "America/Santiago") },
        { TimeZoneRegion.Brasilia, ("E. South America Standard Time", "America/Sao_Paulo") },
        { TimeZoneRegion.MidAtlantic, ("Mid-Atlantic Standard Time", "Atlantic/South_Georgia") },
        { TimeZoneRegion.Azores, ("Azores Standard Time", "Atlantic/Azores") },
        { TimeZoneRegion.London, ("GMT Standard Time", "Europe/London") },
        { TimeZoneRegion.CentralEurope, ("W. Europe Standard Time", "Europe/Berlin") },
        { TimeZoneRegion.EasternEurope, ("GTB Standard Time", "Europe/Athens") },
        { TimeZoneRegion.Moscow, ("Russian Standard Time", "Europe/Moscow") },
        { TimeZoneRegion.Dubai, ("Arabian Standard Time", "Asia/Dubai") },
        { TimeZoneRegion.India, ("India Standard Time", "Asia/Kolkata") },
        { TimeZoneRegion.SoutheastAsia, ("SE Asia Standard Time", "Asia/Bangkok") },
        { TimeZoneRegion.China, ("China Standard Time", "Asia/Shanghai") },
        { TimeZoneRegion.Japan, ("Tokyo Standard Time", "Asia/Tokyo") },
        { TimeZoneRegion.Sydney, ("AUS Eastern Standard Time", "Australia/Sydney") },
        { TimeZoneRegion.NewZealand, ("New Zealand Standard Time", "Pacific/Auckland") }
    };

    /// <summary>
    /// Returns the <see cref="TimeZoneInfo"/> for a given region, respecting the OS platform.
    /// </summary>
    public static TimeZoneInfo GetTimeZoneInfo(this TimeZoneRegion region)
    {
        if (!_timeZoneMap.TryGetValue(region, out var ids))
            throw new ArgumentOutOfRangeException(nameof(region), region, "Unsupported timezone region.");

        var timeZoneId = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? ids.WindowsId
            : ids.IanaId;

        return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
    }

    /// <summary>
    /// Creates a dynamic timezone using a UTC offset in minutes.
    /// </summary>
    private static TimeZoneInfo CreateTimeZoneFromOffset(int offsetMinutes)
    {
        var offset = TimeSpan.FromMinutes(offsetMinutes);
        var displayName = $"UTC{(offset >= TimeSpan.Zero ? "+" : "")}{offset:hh\\:mm}";
        return TimeZoneInfo.CreateCustomTimeZone(displayName, offset, displayName, displayName);
    }
}