using NuvTools.Common.ResultWrapper.Enumerations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NuvTools.Common.ResultWrapper;

/// <summary>
/// Extension methods to convert <see cref="HttpResponseMessage"/> into <see cref="IResult"/> or <see cref="IResult{T}"/>.
/// Handles both success and error responses that may contain a serialized <see cref="Result"/>.
/// </summary>
public static class HttpResponseMessageExtensions
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        ReferenceHandler = ReferenceHandler.Preserve,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public static async Task<IResult> ToResultAsync(this HttpResponseMessage response, CancellationToken cancellationToken = default)
    {
        var (statusCode, reason, content) = await ReadContentAsync(response, cancellationToken);
        if (content is null)
            return response.IsSuccessStatusCode ? Result.Success() : CreateFallbackResult(statusCode, reason, null);

        if (TryDeserialize(content, out Result? result) && IsValidResult(result))
            return result!;

        return response.IsSuccessStatusCode
            ? Result.Success()
            : CreateFallbackResult(statusCode, reason, content);
    }

    public static async Task<IResult<T>> ToResultAsync<T>(this HttpResponseMessage response, CancellationToken cancellationToken = default)
    {
        var (statusCode, reason, content) = await ReadContentAsync(response, cancellationToken);
        if (content is null)
            return response.IsSuccessStatusCode ? Result<T>.Success() : CreateFallbackResult<T>(statusCode, reason, null);

        // Attempt to deserialize Result<T>
        if (TryDeserialize(content, out Result<T>? result) && IsValidResult(result))
            return result!;

        // Try fallback: plain T + error message
        if (TryDeserialize(content, out T? data))
        {
            return response.IsSuccessStatusCode
                ? Result<T>.Success(data!)
                : Result<T>.Fail(CreateMessage(statusCode, reason, content), data!);
        }

        return CreateFallbackResult<T>(statusCode, reason, content);
    }



    public static async Task<IResult<T, E>> ToResultAsync<T, E>(this HttpResponseMessage response, CancellationToken cancellationToken = default)
    {
        var (statusCode, reason, content) = await ReadContentAsync(response, cancellationToken);
        if (content is null)
        {
            return response.IsSuccessStatusCode
                ? Result<T, E>.Success()
                : Result<T, E>.Fail(CreateMessage(statusCode, reason, "Empty response body"));
        }

        if (TryDeserialize(content, out Result<T>? result) && IsValidResult(result))
        {
            return result!.Succeeded
                ? Result<T, E>.Success(result.Data, result.MessageDetail)
                : Result<T, E>.Fail(result.Messages, result.Data);
        }

        if (response.IsSuccessStatusCode && TryDeserialize(content, out T? data))
        {
            Result<T, E>.Success(data);
        }
        else if (TryDeserialize(content, out E? error))
        {
            return Result<T, E>.Fail(CreateMessage(statusCode, reason), error: error!);
        }

        return Result<T, E>.Fail(CreateMessage(statusCode, reason, content));
    }

    // ---------------------------
    // Helpers
    // ---------------------------

    private static async Task<(int StatusCode, string Reason, string? Content)> ReadContentAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        var statusCode = (int)response.StatusCode;
        var reason = response.ReasonPhrase ?? response.StatusCode.ToString();

        try
        {
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            return (statusCode, reason, string.IsNullOrWhiteSpace(content) ? null : content);
        }
        catch (Exception ex)
        {
            return (statusCode, reason, $"Failed to read response body: {ex.Message}");
        }
    }

    private static bool TryDeserialize<T>(string json, out T? result)
    {
        try
        {
            result = JsonSerializer.Deserialize<T>(json, SerializerOptions);
            return result is not null;
        }
        catch
        {
            result = default;
            return false;
        }
    }

    private static bool IsValidResult(IResult? result)
    {
        return result is not null &&
               (result.ResultType != ResultType.Success ||
                result.Succeeded ||
                result is IResult<object> obj && obj.Data is not null ||
                (result.Messages?.Count ?? 0) > 0);
    }

    private static MessageDetail CreateMessage(int statusCode, string reason, string? content = null)
    {
        return new MessageDetail(
            $"Unexpected response ({statusCode} {reason})",
            Detail: TrimBody(content),
            Code: statusCode.ToString());
    }

    private static IResult<T> CreateFallbackResult<T>(int statusCode, string reason, string? content) =>
        Result<T>.Fail(CreateMessage(statusCode, reason, content));

    private static IResult CreateFallbackResult(int statusCode, string reason, string? content) =>
        Result.Fail(CreateMessage(statusCode, reason, content));

    private static string? TrimBody(string? body)
    {
        const int maxLength = 500;
        return string.IsNullOrWhiteSpace(body)
            ? null
            : (body.Length <= maxLength ? body : body[..maxLength] + "...");
    }
}
