
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NuvTools.Common.ResultWrapper;
public static class ResultExtensions
{
    private static readonly JsonSerializerOptions serializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        ReferenceHandler = ReferenceHandler.Preserve
    };

    public static async Task<IResult<T>?> ToResult<T>(this HttpResponseMessage response)
    {
        var responseAsString = await response.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<Result<T>>(responseAsString, serializerOptions);
        return responseObject;
    }

    public static async Task<IResult?> ToResult(this HttpResponseMessage response)
    {
        var responseAsString = await response.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<Result>(responseAsString, serializerOptions);
        return responseObject;
    }
}