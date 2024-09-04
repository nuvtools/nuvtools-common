using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections;

namespace NuvTools.Common.Serialization.Json;
public class MaxDepthJsonConverter<T> : JsonConverter<T>
{
    private readonly int maxDepth;

    public MaxDepthJsonConverter(int maxDepth)
    {
        if (maxDepth <= 0)
            throw new ArgumentOutOfRangeException(nameof(maxDepth), "MaxDepth must be greater than zero.");

        this.maxDepth = maxDepth;
    }

    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
        {
            var jsonElement = doc.RootElement;

            if (jsonElement.ValueKind != JsonValueKind.Object)
            {
                throw new JsonException($"The JSON data must be an object at the root.");
            }

            return JsonSerializer.Deserialize<T>(jsonElement);
        }
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        Serialize(value, writer, options, 1);
    }

    private void Serialize(object? obj, Utf8JsonWriter writer, JsonSerializerOptions options, int currentDepth, bool isFromArray = false)
    {
        if (obj == null)
        {
            writer.WriteNullValue();
            return;
        }

        var objectType = obj.GetType();

        if (objectType.IsSimple())
        {
            JsonSerializer.Serialize(writer, obj, options);
            return;
        }

        if (objectType.IsEnum)
        {
            JsonSerializer.Serialize(writer, obj, objectType, options);
            return;
        }

        if (currentDepth > maxDepth)
        {
            writer.WriteNullValue();
            return; // Reached the maximum depth, stop serialization.
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

        foreach (var property in objectType.GetProperties())
        {
            writer.WritePropertyName(property.Name);
            var value = property.GetValue(obj);
            Serialize(value, writer, options, currentDepth + 1);
        }

        writer.WriteEndObject();
    }
}

