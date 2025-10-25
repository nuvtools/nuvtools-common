using Microsoft.Extensions.DependencyInjection;
using NuvTools.Common.Dates.Enumerations;

namespace NuvTools.Common.Dates;

/// <summary>
/// Provides DI extensions for <see cref="IDateTimeService"/>.
/// </summary>
public static class DateTimeServiceCollectionExtensions
{
    /// <summary>
    /// Registers <see cref="IDateTimeService"/> as a singleton with a specified <see cref="TimeZoneRegion"/>.
    /// </summary>
    public static IServiceCollection AddDateTimeService(this IServiceCollection services, TimeZoneRegion region = TimeZoneRegion.Brasilia)
    {
        services.AddSingleton<IDateTimeService>(new SystemDateTimeService(region));
        return services;
    }
}