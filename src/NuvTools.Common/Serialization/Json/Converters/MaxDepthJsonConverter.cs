using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections;
using System.Globalization;
using System.Reflection;
using NuvTools.Common.Resources;
using NuvTools.Common.Serialization.Json;

namespace NuvTools.Common.Serialization.Json.Converters;
public class MaxDepthJsonConverter<T> : JsonConverter<T>
{
    private readonly int _maxDepth;

    public MaxDepthJsonConverter(int maxDepth)
    {
        if (maxDepth <= 0)
            throw new ArgumentOutOfRangeException(nameof(maxDepth), Messages.MaxDepthMustBeGreaterThanZero);

        _maxDepth = maxDepth;
    }

    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
        {
            var jsonElement = doc.RootElement;

            if (jsonElement.ValueKind != JsonValueKind.Object)
            {
                throw new JsonException(Messages.TheJSONDataMustBeAnObjectAtTheRoot);
            }

            return jsonElement.Deserialize<T>();
        }
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        Serialize(value, writer, options, 1);
    }

    private void Serialize(object? obj, Utf8JsonWriter writer, JsonSerializerOptions options, int currentDepth, bool isFromArray = false, PropertyInfo? propertyInfo = null)
    {
        if (obj == null)
        {
            writer.WriteNullValue();
            return;
        }

        var objectType = obj.GetType();

        if (objectType.IsSimple())
        {
            if (TryUseCustomConverter(obj, writer, options, objectType, propertyInfo))
                return;

            JsonSerializer.Serialize(writer, obj, options);
            return;
        }

        if (objectType.IsEnum)
        {
            JsonSerializer.Serialize(writer, obj, objectType, options);
            return;
        }

        if (currentDepth > _maxDepth)
        {
            writer.WriteNullValue();
            return; // Reached the maximum depth, stop serialization.
        }

        // Dictionaries must be serialized as JSON objects (key/value pairs), like System.Text.Json.
        // Without this branch they match the IEnumerable path and become an array of KeyValuePair objects.
        if (obj is IDictionary dictionary)
        {
            SerializeDictionary(dictionary, writer, options, currentDepth);
            return;
        }

        if (objectType.IsArray || objectType.IsList())
        {
            writer.WriteStartArray();

            var itemList = objectType.IsArray ? (Array)obj : (IEnumerable)obj;

            foreach (var item in itemList)
                Serialize(item, writer, options, isFromArray ? currentDepth + 1 : currentDepth, true);

            writer.WriteEndArray();
            return;
        }

        writer.WriteStartObject();

        // Public instance properties only: the default GetProperties() also returns static properties
        // (e.g. DateOnly.MinValue/MaxValue), which must not be part of the serialized payload.
        foreach (var property in objectType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (!property.CanRead || property.GetIndexParameters().Length > 0)
                continue;

            writer.WritePropertyName(property.Name);
            var value = property.GetValue(obj);
            Serialize(value, writer, options, currentDepth + 1, propertyInfo: property);
        }

        writer.WriteEndObject();
    }

    private void SerializeDictionary(IDictionary dictionary, Utf8JsonWriter writer, JsonSerializerOptions options, int currentDepth)
    {
        writer.WriteStartObject();

        foreach (DictionaryEntry entry in dictionary)
        {
            writer.WritePropertyName(Convert.ToString(entry.Key, CultureInfo.InvariantCulture) ?? string.Empty);
            Serialize(entry.Value, writer, options, currentDepth + 1);
        }

        writer.WriteEndObject();
    }

    /// <summary>
    /// Attempts to use a custom converter if one is defined on the property.
    /// </summary>
    private bool TryUseCustomConverter(object obj, Utf8JsonWriter writer, JsonSerializerOptions options, Type objectType, PropertyInfo? propertyInfo)
    {
        if (propertyInfo == null)
            return false;

        var customConverterAttribute = propertyInfo.GetCustomAttribute<JsonConverterAttribute>();
        if (customConverterAttribute == null)
            return false;

        var customConverter = customConverterAttribute.CreateConverter(objectType);
        if (customConverter == null)
            return false;

        customConverter.GetType()
            .GetMethod(nameof(JsonConverter<object>.Write))
            ?.Invoke(customConverter, [writer, obj, options]);

        return true;
    }
}

