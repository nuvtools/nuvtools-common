using NuvTools.Common.Serialization;
using System.Collections;
using System.Globalization;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Web;

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
    public static string GetQueryString<T>(this T obj, string uriBase = null) where T : class
    {
        ArgumentNullException.ThrowIfNull(obj);

        if (string.IsNullOrEmpty(uriBase)) uriBase = string.Empty;

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

            if (itemType.IsEnum)
            {
                var enumValue = Convert.ChangeType(itemValue, Enum.GetUnderlyingType(itemType));
                list.Add(new KeyValuePair<string, string>(item.Name, enumValue.ToString()));
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


    /// <summary>
    /// Gets the dictionary from QueryString.
    /// </summary>
    /// <param name="queryString">QueryString to parse</param>
    /// <returns></returns>
    public static Dictionary<string, object> ParseQueryString(this string queryString)
    {
        if (string.IsNullOrEmpty(queryString)) return [];

        var result = new Dictionary<string, object>();

        var parts = queryString.Split('?');

        if (parts.Length == 2)
            result.Add("UriBase", parts[0]);

        var parameters = parts[parts.Length == 2 ? 1 : 0].Split('&');

        var listAux = new List<KeyValuePair<string, string>>();

        foreach (var item in parameters)
        {
            var keyValue = item.Split('=');
            listAux.Add(new KeyValuePair<string, string>(HttpUtility.UrlDecode(keyValue[0]), keyValue.Length == 2 ? HttpUtility.UrlDecode(keyValue[1]) : null));
        }

        foreach (var item in listAux.GroupBy(e => e.Key))
            result.Add(item.Key, item.Count() > 1 ? item.Select(e => e.Value).ToArray() : item.First().Value);

        if (result.Count == 1 && result.Any(e => e.Key.Contains("http")))
        {
            result.Clear();
            result.Add("UriBase", queryString);
        }

        return result;
    }
}