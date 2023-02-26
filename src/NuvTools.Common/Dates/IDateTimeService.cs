using System;

namespace NuvTools.Common.Dates;

public interface IDateTimeService
{
    DateTime UtcNow { get; }

    DateTime Now { get; }
}