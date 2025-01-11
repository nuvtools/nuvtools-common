using NuvTools.Common.Resources;
using NuvTools.Common.Strings;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NuvTools.Common.Serialization.Json.Converters;

public class DateTimeConverter<T>(string format) : JsonConverter<T> where T : struct
{
    private readonly string _format = format ?? throw new ArgumentNullException(nameof(format));

    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            if (Nullable.GetUnderlyingType(typeToConvert) != null)
            {
                return default; // Return null for nullable types
            }

            throw new JsonException(Messages.CannotConvertNullToNonNullableTypeX.Format(typeToConvert));
        }

        var dateString = reader.GetString() ?? throw new JsonException(Messages.ExpectedDateString);

        return typeToConvert switch
        {
            _ when typeToConvert == typeof(DateTime) || typeToConvert == typeof(DateTime?) =>
                (T)(object)DateTime.ParseExact(dateString, _format, null),
            _ when typeToConvert == typeof(DateTimeOffset) || typeToConvert == typeof(DateTimeOffset?) =>
                (T)(object)DateTimeOffset.ParseExact(dateString, _format, null),
            _ => throw new NotSupportedException(Messages.UnsupportedTypeX.Format(typeToConvert))
        };
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        if (EqualityComparer<T>.Default.Equals(value, default))
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
            default:
                throw new NotSupportedException(Messages.UnsupportedTypeX.Format(typeof(T)));
        }
    }
}
