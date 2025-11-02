using NuvTools.Common.ResultWrapper.Enumerations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NuvTools.Common.ResultWrapper;

/// <summary>
/// Extension methods to convert <see cref="HttpResponseMessage"/> into <see cref="IResult"/> or <see cref="IResult{T}"/>.
/// Handles both success and error responses that may contain a serialized <see cref="Result"/>.
/// </summary>
public static class ResultExtensions
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        ReferenceHandler = ReferenceHandler.Preserve,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

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
        }

        return CreateFallbackResult<T>(response, content);
    }

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
        }

        return CreateFallbackResult(response, content);
    }

    /// <summary>
    /// Converts an <see cref="HttpResponseMessage"/> into a strongly typed <see cref="IResult{T,E}"/>.
    /// Supports APIs that return non-standard payloads in case of error.
    /// </summary>
    public static async Task<IResult<T, E>> ToResultAsync<T, E>(
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
            return Result<T, E>.Fail($"Failed to read response body: {ex.Message}");
        }

        if (string.IsNullOrWhiteSpace(content))
        {
            return Result<T, E>.Fail(
                new MessageDetail($"Empty response body ({statusCode} {reason})", Code: statusCode.ToString()));
        }

        // Try to deserialize as Result<T>
        try
        {
            var resultT = JsonSerializer.Deserialize<Result<T>>(content, SerializerOptions);
            if (resultT is not null)
            {
                var isReal =
                    resultT.ResultType != ResultType.Success ||
                    resultT.Succeeded ||
                    resultT.Data is not null ||
                    (resultT.Messages?.Count ?? 0) > 0;

                if (isReal)
                {
                    return resultT.Succeeded
                        ? Result<T, E>.Success(resultT.Data, resultT.MessageDetail)
                        : Result<T, E>.Fail(resultT.Messages, resultT.Data);
                }
            }
        }
        catch
        {
        }

        // Try to deserialize as error payload E
        try
        {
            var errorPayload = JsonSerializer.Deserialize<E>(content, SerializerOptions);
            if (errorPayload is not null)
            {
                return Result<T, E>.Fail(
                    new MessageDetail($"Error response ({statusCode} {reason})", Code: statusCode.ToString()),
                    error: errorPayload);
            }
        }
        catch
        {
            // ignore and fallback
        }

        // Fallback if both deserializations failed
        return Result<T, E>.Fail(
            new MessageDetail(
                $"Unexpected response ({statusCode} {reason})",
                Detail: TrimBody(content),
                Code: statusCode.ToString()));
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
