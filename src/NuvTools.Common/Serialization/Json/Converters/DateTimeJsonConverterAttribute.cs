using NuvTools.Common.Resources;
using NuvTools.Common.Strings;
using System.Text.Json.Serialization;

namespace NuvTools.Common.Serialization.Json.Converters;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class DateTimeJsonConverterAttribute(string format) : JsonConverterAttribute
{
    public override JsonConverter CreateConverter(Type typeToConvert)
    {
        return typeToConvert switch
        {
            { } when typeToConvert == typeof(DateTime) || typeToConvert == typeof(DateTime?)
                => new DateTimeJsonConverter<DateTime>(format),
            { } when typeToConvert == typeof(DateTimeOffset) || typeToConvert == typeof(DateTimeOffset?)
                => new DateTimeJsonConverter<DateTimeOffset>(format),
            _ => throw new NotSupportedException(Messages.UnsupportedTypeX.Format(typeToConvert!))
        };
    }
}