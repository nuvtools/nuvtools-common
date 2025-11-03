using NuvTools.Common.Resources;
using NuvTools.Common.Strings;
using System.Text.Json.Serialization;

namespace NuvTools.Common.Serialization.Json.Converters;

/// <summary>
/// Allows applying a <see cref="DateTimeJsonConverter{T}"/> directly on a property,
/// specifying the exact serialization/deserialization date format.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public sealed class DateTimeJsonConverterAttribute(string format) : JsonConverterAttribute
{
    /// <summary>
    /// Creates a typed <see cref="DateTimeJsonConverter{T}"/> for supported date types.
    /// </summary>
    public override JsonConverter CreateConverter(Type typeToConvert)
    {
        if (string.IsNullOrWhiteSpace(format))
            throw new ArgumentNullException(nameof(format), Messages.ExpectedDateString);

        return typeToConvert switch
        {
            Type t when t == typeof(DateTime) || t == typeof(DateTime?) =>
                new DateTimeJsonConverter<DateTime>(format),

            Type t when t == typeof(DateTimeOffset) || t == typeof(DateTimeOffset?) =>
                new DateTimeJsonConverter<DateTimeOffset>(format),

            Type t when t == typeof(DateOnly) || t == typeof(DateOnly?) =>
                new DateTimeJsonConverter<DateOnly>(format),

            _ => throw new NotSupportedException(Messages.UnsupportedTypeX.Format(typeToConvert))
        };
    }
}
