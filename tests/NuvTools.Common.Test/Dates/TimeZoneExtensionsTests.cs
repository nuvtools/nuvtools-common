using NuvTools.Common.Dates;
using NUnit.Framework;
using NuvTools.Common.Dates.Enumerations;

namespace NuvTools.Common.Tests.Dates;

[TestFixture]
public class TimeZoneExtensionsTests
{
    [Test]
    public void ToTimeZone_FromUtc_ShouldConvertToRegionCorrectly()
    {
        // Arrange
        var utcTime = new DateTime(2025, 10, 25, 12, 0, 0, DateTimeKind.Utc);

        // Act
        var brasiliaTime = utcTime.ToTimeZone(TimeZoneRegion.Brasilia, utcDirection: UtcDirection.FromUtc);

        // Assert
        var expectedOffset = TimeSpan.FromHours(-3);
        var diff = brasiliaTime - utcTime;
        Assert.That(diff.TotalHours, Is.EqualTo(expectedOffset.TotalHours).Within(0.1),
            "Conversion from UTC to Brasília should result in -3h difference.");
    }

    [Test]
    public void ToTimeZone_ToUtc_ShouldConvertFromRegionToUtcCorrectly()
    {
        // Arrange
        var localBrasilia = new DateTime(2025, 10, 25, 9, 0, 0, DateTimeKind.Unspecified);

        // Act
        var utc = localBrasilia.ToTimeZone(TimeZoneRegion.Brasilia, utcDirection: UtcDirection.ToUtc);

        // Assert
        Assert.That(utc.Hour, Is.EqualTo(12),
            "Brasília (UTC-3) at 09:00 should convert to 12:00 UTC.");
    }

    [Test]
    public void ToTimeZone_FromUtc_WithOffset_ShouldApplyCustomOffset()
    {
        // Arrange
        var utcTime = new DateTime(2025, 10, 25, 12, 0, 0, DateTimeKind.Utc);
        int offsetMinutes = -180; // Brasília (-03:00)

        // Act
        var offsetTime = utcTime.ToTimeZone(offsetMinutes: offsetMinutes, utcDirection: UtcDirection.FromUtc);

        // Assert
        Assert.That(offsetTime.Hour, Is.EqualTo(9));
    }

    [Test]
    public void ToTimeZone_ToUtc_WithOffset_ShouldApplyCustomOffset()
    {
        // Arrange
        var localTime = new DateTime(2025, 10, 25, 9, 0, 0, DateTimeKind.Unspecified);
        int offsetMinutes = -180; // UTC-3

        // Act
        var utcTime = localTime.ToTimeZone(offsetMinutes: offsetMinutes, utcDirection: UtcDirection.ToUtc);

        // Assert
        Assert.That(utcTime.Hour, Is.EqualTo(12),
            "Offset -180 (UTC-3) at 09:00 should convert to 12:00 UTC.");
    }

    [Test]
    public void DateParseToUtc_ShouldParseStringAndConvertToUtc()
    {
        // Arrange
        var dateString = "25-10-2025 09:00:00";

        // Act
        var utcTime = dateString.DateParseToUtc(fromTimeZoneRegion: TimeZoneRegion.Brasilia);

        // Assert
        Assert.That(utcTime.Kind, Is.EqualTo(DateTimeKind.Utc));
        Assert.That(utcTime.Hour, Is.EqualTo(12));
    }

    [Test]
    public void DateParseToUtc_WithOffset_ShouldUseOffsetInsteadOfRegion()
    {
        // Arrange
        var dateString = "25-10-2025 09:00:00";

        // Act
        var utcTime = dateString.DateParseToUtc(offsetMinutes: -180);

        // Assert
        Assert.That(utcTime.Kind, Is.EqualTo(DateTimeKind.Utc));
        Assert.That(utcTime.Hour, Is.EqualTo(12));
    }

    [Test]
    public void ToTimeZoneOffset_ShouldConvertDateTimeOffsetBetweenZones()
    {
        // Arrange
        var utcOffset = new DateTimeOffset(2025, 10, 25, 12, 0, 0, TimeSpan.Zero);

        // Act
        var brasiliaOffset = utcOffset.ToTimeZoneOffset(TimeZoneRegion.Brasilia);

        // Assert
        Assert.That(brasiliaOffset.Offset.TotalHours, Is.EqualTo(-3).Within(0.1));
        Assert.That(brasiliaOffset.DateTime.Hour, Is.EqualTo(9));
    }

    [Test]
    public void ToTimeZoneOffset_WithManualOffset_ShouldApplyCustomOffset()
    {
        // Arrange
        var utcOffset = new DateTimeOffset(2025, 10, 25, 12, 0, 0, TimeSpan.Zero);

        // Act
        var customOffset = utcOffset.ToTimeZoneOffset(offsetMinutes: -180);

        // Assert
        Assert.That(customOffset.Offset, Is.EqualTo(TimeSpan.FromHours(-3)));
        Assert.That(customOffset.DateTime.Hour, Is.EqualTo(9));
    }

    [Test]
    public void GetTimeZoneInfo_ShouldReturnValidTimeZone()
    {
        // Act
        var tz = TimeZoneRegion.Brasilia.GetTimeZoneInfo();

        // Assert
        Assert.That(tz, Is.Not.Null);
        Assert.That(tz.StandardName, Does.Contain("E. South America").Or.Contain("Sao_Paulo").IgnoreCase);
    }

    [Test]
    public void InvalidRegion_ShouldThrowArgumentOutOfRange()
    {
        // Arrange
        var invalidRegion = (TimeZoneRegion)999;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => invalidRegion.GetTimeZoneInfo());
    }

    [Test]
    public void ToTimeZone_ShouldConvertDateTimeWithoutChangingDateWhenSameZone()
    {
        // Arrange
        var now = new DateTime(2025, 10, 25, 10, 0, 0);

        // Act
        var converted = now.ToTimeZone(TimeZoneRegion.Brasilia, utcDirection: UtcDirection.None);

        // Assert
        Assert.That(converted.Date, Is.EqualTo(now.Date));
    }
}