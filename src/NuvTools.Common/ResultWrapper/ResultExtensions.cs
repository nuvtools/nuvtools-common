using System.Text.Json;
using System.Text.Json.Serialization;

namespace NuvTools.Common.ResultWrapper;

/// <summary>
/// Extension methods to convert <see cref="HttpResponseMessage"/> into <see cref="IResult"/> or <see cref="IResult{T}"/>.
/// Handles both success and error responses (e.g., 404, 500) that may contain a serialized <see cref="Result"/>.
/// </summary>
public static class ResultExtensions
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        ReferenceHandler = ReferenceHandler.Preserve,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    /// <summary>
    /// Converts an <see cref="HttpResponseMessage"/> into a strongly typed <see cref="IResult{T}"/>.
    /// Always returns a valid <see cref="IResult{T}"/> (never null), even for non-successful responses or invalid bodies.
    /// </summary>
    public static async Task<IResult<T>> ToResultAsync<T>(
        this HttpResponseMessage response,
        CancellationToken cancellationToken = default)
    {
        var statusCode = (int)response.StatusCode;
        var reason = response.ReasonPhrase ?? response.StatusCode.ToString();

        string? content;
        try
        {
            content = await response.Content.ReadAsStringAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return Result<T>.Fail($"Failed to read response body: {ex.Message}");
        }

        if (string.IsNullOrWhiteSpace(content))
        {
            return Result<T>.Fail(
                new MessageDetail($"Empty response body ({statusCode} {reason})", Code: statusCode.ToString()));
        }

        try
        {
            var result = JsonSerializer.Deserialize<Result<T>>(content, SerializerOptions);
            if (result is not null)
                return result;
        }
        catch
        {
            // Ignore and fallback to generic failure
        }

        return CreateFallbackResult<T>(response, content);
    }

    /// <summary>
    /// Converts an <see cref="HttpResponseMessage"/> into a non-generic <see cref="IResult"/>.
    /// Always returns a valid <see cref="IResult"/> (never null), even for non-successful responses or invalid bodies.
    /// </summary>
    public static async Task<IResult> ToResultAsync(
        this HttpResponseMessage response,
        CancellationToken cancellationToken = default)
    {
        var statusCode = (int)response.StatusCode;
        var reason = response.ReasonPhrase ?? response.StatusCode.ToString();

        string? content;
        try
        {
            content = await response.Content.ReadAsStringAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return Result.Fail($"Failed to read response body: {ex.Message}");
        }

        if (string.IsNullOrWhiteSpace(content))
        {
            return Result.Fail(
                new MessageDetail($"Empty response body ({statusCode} {reason})", Code: statusCode.ToString()));
        }

        try
        {
            var result = JsonSerializer.Deserialize<Result>(content, SerializerOptions);
            if (result is not null)
                return result;
        }
        catch
        {
            // Ignore and fallback to generic failure
        }

        return CreateFallbackResult(response, content);
    }

    #region Private Helpers

    private static IResult<T> CreateFallbackResult<T>(HttpResponseMessage response, string? body)
    {
        var statusCode = (int)response.StatusCode;
        var reason = response.ReasonPhrase ?? response.StatusCode.ToString();
        var detail = TrimBody(body);

        return Result<T>.Fail(
            new MessageDetail(
                $"Unexpected response ({statusCode} {reason})",
                Detail: detail,
                Code: statusCode.ToString()));
    }

    private static IResult CreateFallbackResult(HttpResponseMessage response, string? body)
    {
        var statusCode = (int)response.StatusCode;
        var reason = response.ReasonPhrase ?? response.StatusCode.ToString();
        var detail = TrimBody(body);

        return Result.Fail(
            new MessageDetail(
                $"Unexpected response ({statusCode} {reason})",
                Detail: detail,
                Code: statusCode.ToString()));
    }

    private static string? TrimBody(string? body)
    {
        if (string.IsNullOrWhiteSpace(body))
            return null;

        const int maxLength = 500;
        return body.Length <= maxLength ? body : body[..maxLength] + "...";
    }

    #endregion
}
