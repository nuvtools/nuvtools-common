using NuvTools.Common.Resources;
using NuvTools.Common.Strings;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NuvTools.Common.Serialization.Json.Converters;

/// <summary>
/// JSON converter for <see cref="DateTime"/>, <see cref="DateTimeOffset"/> and <see cref="DateOnly"/>
/// allowing serialization and deserialization using a custom format.
/// </summary>
/// <typeparam name="T">Supported types: DateTime, DateTime?, DateTimeOffset, DateTimeOffset?, DateOnly, DateOnly?</typeparam>
public class DateTimeJsonConverter<T>(string format) : JsonConverter<T> where T : struct
{
    private readonly string _format = format ?? throw new ArgumentNullException(nameof(format));

    /// <inheritdoc />
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            if (Nullable.GetUnderlyingType(typeToConvert) != null)
                return default;

            throw new JsonException(Messages.CannotConvertNullToNonNullableTypeX.Format(typeToConvert));
        }

        var dateString = reader.GetString();
        if (string.IsNullOrWhiteSpace(dateString))
            throw new JsonException(Messages.ExpectedDateString);

        try
        {
            return typeToConvert switch
            {
                _ when typeToConvert == typeof(DateTime) ||
                       typeToConvert == typeof(DateTime?) =>
                    (T)(object)DateTime.ParseExact(dateString, _format, null),

                _ when typeToConvert == typeof(DateTimeOffset) ||
                       typeToConvert == typeof(DateTimeOffset?) =>
                    (T)(object)DateTimeOffset.ParseExact(dateString, _format, null),

                _ when typeToConvert == typeof(DateOnly) ||
                       typeToConvert == typeof(DateOnly?) =>
                    (T)(object)DateOnly.ParseExact(dateString, _format, null),

                _ => throw new NotSupportedException(Messages.UnsupportedTypeX.Format(typeToConvert))
            };
        }
        catch (FormatException)
        {
            throw new JsonException(Messages.UnsupportedTypeX.Format(dateString));
        }
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        if (EqualityComparer<T>.Default.Equals(value, default) &&
            Nullable.GetUnderlyingType(typeof(T)) != null)
        {
            writer.WriteNullValue();
            return;
        }

        switch (value)
        {
            case DateTime dateTimeValue:
                writer.WriteStringValue(dateTimeValue.ToString(_format));
                break;

            case DateTimeOffset dateTimeOffsetValue:
                writer.WriteStringValue(dateTimeOffsetValue.ToString(_format));
                break;

            case DateOnly dateOnlyValue:
                writer.WriteStringValue(dateOnlyValue.ToString(_format));
                break;

            default:
                throw new NotSupportedException(Messages.UnsupportedTypeX.Format(typeof(T)));
        }
    }
}