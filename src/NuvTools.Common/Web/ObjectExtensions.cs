using NuvTools.Common.Serialization;
using System.Collections;
using System.Text.Encodings.Web;
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
    public static string GetQueryString<T>(this T obj, string? uriBase = null) where T : class
    {
        ArgumentNullException.ThrowIfNull(obj, nameof(obj));
        uriBase ??= string.Empty;

        var properties = obj.GetType()
            .GetProperties()
            .Where(p => p.GetValue(obj, null) != null)
            .ToList();

        if (!properties.Any())
            return uriBase;

        var queryParams = new List<KeyValuePair<string, string>>();

        foreach (var property in properties)
        {
            var value = property.GetValue(obj, null);
            if (value == null)
                continue;

            var type = value.GetType();

            if (type == typeof(DateTimeOffset))
            {
                var dateTimeOffset = (DateTimeOffset)value;
                queryParams.Add(new KeyValuePair<string, string>(property.Name, dateTimeOffset.UtcDateTime.ToString("yyyy-MM-ddTHH:mm:ss")));
                queryParams.Add(new KeyValuePair<string, string>($"{property.Name}Offset", dateTimeOffset.Offset.ToString(@"hh\:mm")));
            }
            else if (type == typeof(DateTime))
            {
                var dateTime = (DateTime)value;
                queryParams.Add(new KeyValuePair<string, string>(property.Name, dateTime.ToString("yyyy-MM-ddTHH:mm:ss")));
            }
            else if (type.IsSimple() || type.IsEnum)
            {
                var simpleValue = type.IsEnum
                    ? Convert.ChangeType(value, Enum.GetUnderlyingType(type)).ToString()
                    : value.ToString();

                queryParams.Add(new KeyValuePair<string, string>(property.Name, simpleValue!));
            }
            else if (type.IsArray || type.IsList())
            {
                var enumerable = ((IEnumerable)value).Cast<object>();
                foreach (var item in enumerable)
                {
                    if (item.GetType().IsSimple())
                    {
                        queryParams.Add(new KeyValuePair<string, string>(property.Name, item.ToString()!));
                    }
                }
            }
        }

        return uriBase + "?" + string.Join('&', queryParams.Select(e => UrlEncoder.Default.Encode(e.Key) + "=" + UrlEncoder.Default.Encode(e.Value)));
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
            listAux.Add(new KeyValuePair<string, string>(HttpUtility.UrlDecode(keyValue[0]), keyValue.Length == 2 ? HttpUtility.UrlDecode(keyValue[1]) : string.Empty));
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