using Microsoft.Extensions.Logging;
using NuvTools.Common.ResultWrapper.Enumerations;

namespace NuvTools.Common.ResultWrapper;

/// <summary>
/// Represents an operation result that returns success data (T)
/// and can also capture a typed error payload (E) for APIs that return
/// non-standard error contracts.
/// </summary>
/// <typeparam name="T">Type of the success data.</typeparam>
/// <typeparam name="E">Type of the typed error payload returned on failure.</typeparam>
public class Result<T, E> : Result<T>, IResult<T, E>
{
    /// <inheritdoc />
    public E? ErrorPayload { get; set; }

    #region Factory Methods

    /// <summary>
    /// Creates a successful result with optional success data and message.
    /// </summary>
    public static new IResult<T, E> Success(T? data = default, MessageDetail? message = null)
    {
        var result = new Result<T, E> { Data = data };
        return CreateResult(
            ResultType.Success,
            result,
            message != null ? [message] : []
        );
    }

    /// <summary>
    /// Creates a successful result with data and a simple message.
    /// </summary>
    public static new IResult<T, E> Success(T? data, string message)
        => Success(data, new MessageDetail(message));

    /// <summary>
    /// Creates a failure result with optional success data and typed error payload.
    /// </summary>
    public static IResult<T, E> Fail(
        List<MessageDetail>? messages = null,
        T? data = default,
        E? error = default,
        ILogger? logger = null)
    {
        var result = new Result<T, E>
        {
            Data = data,
            ErrorPayload = error
        };

        return CreateResult(
            ResultType.Error,
            result,
            messages,
            logger
        );
    }

    /// <summary>
    /// Creates a failure result with a single error message.
    /// </summary>
    public static IResult<T, E> Fail(
        MessageDetail message,
        T? data = default,
        E? error = default,
        ILogger? logger = null)
        => Fail([message], data, error, logger);

    /// <summary>
    /// Creates a failure result with a simple string message.
    /// </summary>
    public static IResult<T, E> Fail(
        string message,
        T? data = default,
        E? error = default,
        ILogger? logger = null)
        => Fail([new MessageDetail(message)], data, error, logger);

    /// <summary>
    /// Creates a "Not Found" (404) failure result.
    /// </summary>
    public static IResult<T, E> FailNotFound(
        string message,
        T? data = default,
        E? error = default)
        => Fail(new MessageDetail(message, Code: "404"), data, error);

    #endregion
}
