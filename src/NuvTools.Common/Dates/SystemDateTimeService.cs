using NuvTools.Common.Dates.Enumerations;

namespace NuvTools.Common.Dates;

/// <summary>
/// Default implementation of <see cref="IDateTimeService"/> using <see cref="TimeZoneExtensions"/>.
/// </summary>
public class SystemDateTimeService(TimeZoneRegion region = TimeZoneRegion.Brasilia) : IDateTimeService
{
    public TimeZoneRegion Region { get; } = region;

    public DateTimeOffset UtcNowOffset => DateTimeOffset.UtcNow;
    public DateTimeOffset NowOffset => new(Now, UtcNowOffset.Offset);

    public DateTime UtcNow => DateTime.UtcNow;

    public DateTime Now => DateTime.UtcNow.ToTimeZone(Region, UtcDirection.FromUtc);

    public DateTime ConvertFromUtc(DateTime utcDateTime)
        => utcDateTime.ToTimeZone(Region, UtcDirection.FromUtc);

    public DateTime ConvertToUtc(DateTime localDateTime)
        => localDateTime.ToTimeZone(Region, UtcDirection.ToUtc);

    public DateTimeOffset ConvertFromUtc(DateTimeOffset utcDateTime)
        => new(ConvertFromUtc(utcDateTime.UtcDateTime));

    public DateTimeOffset ConvertToUtc(DateTimeOffset localDateTime)
        => new(ConvertToUtc(localDateTime.DateTime));
}