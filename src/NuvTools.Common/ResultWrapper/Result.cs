using Microsoft.Extensions.Logging;
using NuvTools.Common.Exceptions;
using NuvTools.Common.ResultWrapper.Enumerations;

namespace NuvTools.Common.ResultWrapper;

/// <summary>
/// Represents a non-generic operation result, containing the outcome of an operation,
/// associated messages, and status information.
/// </summary>
/// <remarks>
/// This class is used to represent the result of operations that do not return data.
/// It provides static factory methods to easily create success, validation failure,
/// or error results, optionally with message details and logging support.
/// </remarks>
/// <example>
/// Example usage:
/// <code>
/// IResult result = Result.Success("Operation completed successfully.");
/// 
/// if (!result.Succeeded)
/// {
///     logger.LogError(result.Message);
/// }
/// </code>
/// </example>
public class Result : ResultBase, IResult
{
    #region Factory Methods

    /// <summary>
    /// Creates an error result with optional message details and logging.
    /// </summary>
    /// <param name="messages">A list of <see cref="MessageDetail"/> objects providing error information.</param>
    /// <param name="logger">An optional <see cref="ILogger"/> to log the failure.</param>
    public static IResult Fail(List<MessageDetail>? messages = null, ILogger? logger = null)
        => CreateResult(ResultType.Error, new Result(), messages, logger);

    /// <summary>
    /// Creates an error result from a list of simple message strings.
    /// </summary>
    /// <param name="messages">A list of message strings describing the failure.</param>
    /// <param name="logger">An optional <see cref="ILogger"/> to log the failure.</param>
    public static IResult Fail(List<string> messages, ILogger? logger = null)
        => Fail(ConvertToMessageDetail(messages), logger);

    /// <summary>
    /// Creates an error result from a single message string.
    /// </summary>
    /// <param name="message">A short message describing the failure.</param>
    /// <param name="logger">An optional <see cref="ILogger"/> to log the failure.</param>
    public static IResult Fail(string message, ILogger? logger = null)
        => Fail([new MessageDetail(message)], logger);

    /// <summary>
    /// Creates an error result from a single <see cref="MessageDetail"/> instance.
    /// </summary>
    /// <param name="message">The message describing the failure.</param>
    /// <param name="logger">An optional <see cref="ILogger"/> to log the failure.</param>
    public static IResult Fail(MessageDetail message, ILogger? logger = null)
        => Fail([message], logger);

    /// <summary>
    /// Creates an error result based on an exception, optionally logging it.
    /// </summary>
    /// <param name="exception">The exception that caused the failure.</param>
    /// <param name="level">Specifies how many inner exception levels should be aggregated.</param>
    /// <param name="logger">An optional <see cref="ILogger"/> to log the exception.</param>
    public static IResult Fail(Exception exception, short level = 1, ILogger? logger = null)
        => Fail([new MessageDetail(exception.AggregateExceptionMessages(level))], logger);

    /// <summary>
    /// Creates a failure result representing a "Not Found" condition (HTTP 404).
    /// </summary>
    /// <param name="message">The message describing the missing resource.</param>
    public static IResult FailNotFound(string message)
        => Fail(new MessageDetail(message, Code: "404"));

    /// <summary>
    /// Creates a validation failure result with detailed validation messages.
    /// </summary>
    /// <param name="messages">A list of validation error messages.</param>
    /// <param name="logger">An optional <see cref="ILogger"/> for logging the validation errors.</param>
    public static IResult ValidationFail(List<MessageDetail> messages, ILogger? logger = null)
        => CreateResult(ResultType.ValidationError, new Result(), messages, logger);

    /// <summary>
    /// Creates a validation failure result from a list of message strings.
    /// </summary>
    public static IResult ValidationFail(List<string> messages, ILogger? logger = null)
        => ValidationFail(ConvertToMessageDetail(messages), logger);

    /// <summary>
    /// Creates a validation failure result from a single <see cref="MessageDetail"/>.
    /// </summary>
    public static IResult ValidationFail(MessageDetail message, ILogger? logger = null)
        => ValidationFail([message], logger);

    /// <summary>
    /// Creates a validation failure result from a single message string.
    /// </summary>
    public static IResult ValidationFail(string message, ILogger? logger = null)
        => ValidationFail([new MessageDetail(message)], logger);

    /// <summary>
    /// Creates a successful operation result, optionally with a message.
    /// </summary>
    /// <param name="message">An optional message describing the success.</param>
    public static IResult Success(MessageDetail? message = null)
        => CreateResult(ResultType.Success, new Result(), message != null ? [message] : []);

    /// <summary>
    /// Creates a successful operation result with a simple message string.
    /// </summary>
    public static IResult Success(string message)
        => Success(new MessageDetail(message));

    #endregion
}

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
