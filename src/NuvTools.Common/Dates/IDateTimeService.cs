using NuvTools.Common.Dates.Enumerations;

namespace NuvTools.Common.Dates;

/// <summary>
/// Provides a timezone-aware abstraction for system date and time operations.
/// </summary>
public interface IDateTimeService
{
    /// <summary>Current UTC DateTime.</summary>
    DateTime UtcNow { get; }

    /// <summary>Current UTC DateTimeOffset.</summary>
    DateTimeOffset UtcNowOffset { get; }

    /// <summary>Current local DateTime based on configured region.</summary>
    DateTime Now { get; }

    /// <summary>Current local DateTimeOffset based on configured region.</summary>
    DateTimeOffset NowOffset { get; }

    /// <summary>Configured timezone region.</summary>
    TimeZoneRegion Region { get; }

    /// <summary>Converts UTC DateTime to local region time.</summary>
    DateTime ConvertFromUtc(DateTime utcDateTime);

    /// <summary>Converts local DateTime to UTC.</summary>
    DateTime ConvertToUtc(DateTime localDateTime);

    /// <summary>Converts UTC DateTimeOffset to local region time.</summary>
    DateTimeOffset ConvertFromUtc(DateTimeOffset utcDateTime);

    /// <summary>Converts local DateTimeOffset to UTC.</summary>
    DateTimeOffset ConvertToUtc(DateTimeOffset localDateTime);
}