using Microsoft.Extensions.Logging;
using NuvTools.Common.Exceptions;
using NuvTools.Common.ResultWrapper.Enumerations;

namespace NuvTools.Common.ResultWrapper;

/// <summary>
/// Represents a generic operation result that carries a data payload along with
/// success status and message details.
/// </summary>
/// <typeparam name="T">The type of data returned by the operation.</typeparam>
/// <remarks>
/// This class provides a unified and type-safe way to return both data and
/// status information from service or repository operations.
/// </remarks>
/// <example>
/// Example usage:
/// <code>
/// IResult&lt;UserDto&gt; result = await _userService.GetUserByIdAsync(1);
/// 
/// if (result.Succeeded)
/// {
///     Console.WriteLine($"User: {result.Data!.Name}");
/// }
/// else
/// {
///     logger.LogWarning(result.Message);
/// }
/// </code>
/// </example>
public class Result<T> : ResultBase, IResult<T>
{
    /// <inheritdoc />
    public T? Data { get; set; }

    #region Factory Methods

    /// <summary>
    /// Creates an error result with optional messages and data.
    /// </summary>
    public static IResult<T> Fail(List<MessageDetail>? messages = null, T? data = default, ILogger? logger = null)
        => CreateResult(ResultType.Error, new Result<T> { Data = data }, messages, logger);

    /// <summary>
    /// Creates an error result from a list of simple message strings.
    /// </summary>
    public static IResult<T> Fail(List<string> messages, T? data = default, ILogger? logger = null)
        => Fail(ConvertToMessageDetail(messages), data, logger);

    /// <summary>
    /// Creates an error result from a single <see cref="MessageDetail"/>.
    /// </summary>
    public static IResult<T> Fail(MessageDetail message, T? data = default, ILogger? logger = null)
        => Fail([message], data, logger);

    /// <summary>
    /// Creates an error result from a single message string.
    /// </summary>
    public static IResult<T> Fail(string message, T? data = default, ILogger? logger = null)
        => Fail([new MessageDetail(message)], data, logger);

    /// <summary>
    /// Creates an error result from an exception, with optional data and logging.
    /// </summary>
    public static IResult<T> Fail(Exception exception, short level = 1, T? data = default, ILogger? logger = null)
        => Fail([new MessageDetail(exception.AggregateExceptionMessages(level))], data, logger);

    /// <summary>
    /// Creates a "Not Found" failure result with code "404".
    /// </summary>
    public static IResult<T> FailNotFound(string message)
        => Fail(new MessageDetail(message, Code: "404"));

    /// <summary>
    /// Creates a validation failure result with message details.
    /// </summary>
    public static IResult<T> ValidationFail(List<MessageDetail> messages, T? data = default, ILogger? logger = null)
        => CreateResult(ResultType.ValidationError, new Result<T> { Data = data }, messages, logger);

    /// <summary>
    /// Creates a validation failure result from a list of message strings.
    /// </summary>
    public static IResult<T> ValidationFail(List<string> messages, T? data = default, ILogger? logger = null)
        => ValidationFail(ConvertToMessageDetail(messages), data, logger);

    /// <summary>
    /// Creates a validation failure result from a single <see cref="MessageDetail"/>.
    /// </summary>
    public static IResult<T> ValidationFail(MessageDetail message, T? data = default, ILogger? logger = null)
        => ValidationFail([message], data, logger);

    /// <summary>
    /// Creates a validation failure result from a simple message string.
    /// </summary>
    public static IResult<T> ValidationFail(string message, T? data = default, ILogger? logger = null)
        => ValidationFail([new MessageDetail(message)], data, logger);

    /// <summary>
    /// Creates a successful operation result containing optional data and message.
    /// </summary>
    public static IResult<T> Success(T? data = default, MessageDetail? message = null)
        => CreateResult(ResultType.Success, new Result<T> { Data = data }, message != null ? [message] : []);

    /// <summary>
    /// Creates a successful operation result containing data and a message string.
    /// </summary>
    public static IResult<T> Success(T? data, string message)
        => Success(data, new MessageDetail(message));

    /// <summary>
    /// Creates a successful operation result containing only a message.
    /// </summary>
    public static IResult<T> Success(MessageDetail? message)
        => Success(default, message);

    /// <summary>
    /// Creates a successful operation result with a simple message string.
    /// </summary>
    public static IResult<T> Success(string message)
        => Success(default, new MessageDetail(message));

    #endregion
}