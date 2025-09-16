
using System.Net.Http.Json;
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

    public static async Task<IResult<T>?> ToResult<T>(this HttpResponseMessage response, CancellationToken cancellationToken = default)
    {
        try
        {
            return await response.Content.ReadFromJsonAsync<Result<T>>(serializerOptions, cancellationToken);
        }
        catch
        {
            return null;
        }
    }

    public static async Task<IResult?> ToResult(this HttpResponseMessage response, CancellationToken cancellationToken = default)
    {
        try
        {
            return await response.Content.ReadFromJsonAsync<Result>(serializerOptions, cancellationToken);
        }
        catch
        {
            return null;
        }
    }
}