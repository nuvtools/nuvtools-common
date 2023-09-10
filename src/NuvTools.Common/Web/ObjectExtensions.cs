using NuvTools.Common.Serialization;
using System.Collections;
using System.Globalization;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace NuvTools.Common.Web;

public static class ObjectExtensions
{
    /// <summary>
    /// Gets the query string url from the object and uriBase. Only simple/primitives/lists types are converted.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj">Object source to generate QueryString</param>
    /// <param name="uriBase">Uri base address</param>
    /// <returns></returns>
    public static string GetQueryString<T>(this T obj, string uriBase) where T : class
    {
        var properties = from p in obj.GetType().GetProperties()
                         where p.GetValue(obj, null) != null
                         select p;

        if (!properties.Any())
            return uriBase;

        var list = new List<KeyValuePair<string, string>>();

        foreach (var item in properties)
        {
            var itemValue = item.GetValue(obj, null);
            var itemType = itemValue.GetType();

            if (itemType.IsSimple())
            {
                var value = itemType == typeof(DateTime) ?
                            ((DateTime)itemValue).ToString("yyyy-MM-ddThh:mm:ss", CultureInfo.InvariantCulture)
                            : itemValue.ToString();

                list.Add(new KeyValuePair<string, string>(item.Name, value));
                continue;
            }

            if (!itemType.IsArray && !itemType.IsList()) continue;

            var listValue = (itemType.IsArray ? (Array)itemValue : (IEnumerable)itemValue).Cast<object>();

            if (!listValue.Any(e => e.GetType().IsSimple())) continue;

            var a = listValue.Select(e => new KeyValuePair<string, string>(item.Name, e.ToString()));

            list.AddRange(a);
        }

        var result = $"{uriBase}?{string.Join('&', list.Select(e => $"{UrlEncoder.Default.Encode(e.Key)}={UrlEncoder.Default.Encode(e.Value)}"))}";

        return result;
    }
}