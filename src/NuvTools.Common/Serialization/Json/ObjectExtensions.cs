using System.Text.Json;

namespace NuvTools.Common.Serialization.Json;

public static class ObjectExtensions
{
    /// <summary>
    /// Clone the value using serialization and deserialization aproach.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value">Value to be copied.</param>
    /// <param name="totalLevel">Quantity of levels to be copied.</param>
    /// <returns></returns>
    public static T? Clone<T>(this T value, int totalLevel = 1) => value.Serialize(totalLevel).Deserialize<T>();

    private static JsonSerializerOptions GetOptions<T>(int totalLevel = 1) => new()
    {
        Converters = { new MaxDepthJsonConverter<T>(totalLevel) }
    };

    /// <summary>
    /// Serialize the value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value">Value to be serialized.</param>
    /// <param name="totalLevel">Quantity of levels to be serialized.</param>
    /// <returns></returns>
    public static string Serialize<T>(this T value, int totalLevel = 1)
    {
        return JsonSerializer.Serialize(value, GetOptions<T>(totalLevel));
    }

    /// <summary>
    /// Deserializes the Json string.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="serializedValue">Serialized string (Json) to be deserialized.</param>
    /// <param name="totalLevel">Quantity of levels to be deserialized. Default: 0 - Will returns all levels.</param>
    /// <returns></returns>
    public static T? Deserialize<T>(this string serializedValue, int totalLevel = 0)
    {
        var obj = JsonSerializer.Deserialize<T>(serializedValue);
        if (totalLevel == 0) return obj;

        return obj.Serialize(totalLevel).Deserialize<T>();
    }
}