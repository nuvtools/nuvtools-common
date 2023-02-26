using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO;

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
    public static T Clone<T>(this T value, int totalLevel = 1) => value.Serialize(totalLevel).Deserialize<T>(totalLevel);

    private static JsonSerializerSettings CreateConfiguration()
    {
        var configuration = new JsonSerializerSettings
        {
            Formatting = Formatting.None,
            NullValueHandling = NullValueHandling.Ignore,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            PreserveReferencesHandling = PreserveReferencesHandling.None
        };

        return configuration;
    }

    /// <summary>
    /// Serialize the value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value">Value to be serialized.</param>
    /// <param name="totalLevel">Quantity of levels to be copied.</param>
    /// <returns></returns>
    public static string Serialize<T>(this T value, int totalLevel = 1)
    {
        var configuracao = CreateConfiguration();

        using (var strWriter = new StringWriter())
        {
            using (var jsonWriter = new MaxDepthJsonTextWriter(strWriter))
            {
                int levelRemaining() => totalLevel - jsonWriter.CurrentDepth;
                var resolver = new CondicionalContractResolver(levelRemaining) { NamingStrategy = new CamelCaseNamingStrategy() };

                var serializer = new JsonSerializer
                {
                    ContractResolver = resolver,
                    Formatting = configuracao.Formatting,
                    NullValueHandling = configuracao.NullValueHandling,
                    DateFormatHandling = configuracao.DateFormatHandling,
                    ReferenceLoopHandling = configuracao.ReferenceLoopHandling,
                    PreserveReferencesHandling = configuracao.PreserveReferencesHandling,
                };
                serializer.Serialize(jsonWriter, value);
            }
            return strWriter.ToString();
        }
    }

    /// <summary>
    /// Deserializes the Json string.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="serializedValue">Serialized string (Json) to be deserialized.</param>
    /// <param name="totalLevel">Quantity of levels to be retrieved.</param>
    /// <returns></returns>
    public static T Deserialize<T>(this string serializedValue, int totalLevel = 1) =>
        JsonConvert.DeserializeObject<T>(serializedValue, CreateConfiguration());
}