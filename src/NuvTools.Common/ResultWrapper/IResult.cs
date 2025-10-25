using NuvTools.Common.ResultWrapper.Enumerations;

namespace NuvTools.Common.ResultWrapper;

/// <summary>
/// Defines the base contract for an operation result, representing
/// the outcome of a business or system process along with contextual messages.
/// </summary>
/// <remarks>
/// This interface provides a consistent structure for handling success and error
/// results throughout the application.  
/// Implementations usually include information such as:
/// <list type="bullet">
/// <item><description>Whether the operation succeeded (<see cref="Succeeded"/>)</description></item>
/// <item><description>The result classification (<see cref="ResultType"/>)</description></item>
/// <item><description>Messages providing feedback or error details (<see cref="Messages"/>)</description></item>
/// </list>
/// It is commonly implemented by <see cref="Result"/> and <see cref="Result{T}"/> classes.
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
public interface IResult
{
    /// <summary>
    /// A collection of messages associated with the operation result.
    /// Each <see cref="MessageDetail"/> contains structured information
    /// about the outcome (e.g., warnings, validation errors, or success messages).
    /// </summary>
    List<MessageDetail> Messages { get; }

    /// <summary>
    /// The primary message detail, typically the first or most relevant message
    /// from the <see cref="Messages"/> collection.
    /// </summary>
    MessageDetail? MessageDetail { get; }

    /// <summary>
    /// A simplified or aggregated version of the main message,
    /// typically derived from <see cref="MessageDetail.Title"/>.
    /// </summary>
    string? Message { get; }

    /// <summary>
    /// Indicates whether the operation completed successfully.
    /// </summary>
    bool Succeeded { get; }

    /// <summary>
    /// Indicates whether the result represents a "Not Found" condition.
    /// Useful for distinguishing between empty results and missing resources.
    /// </summary>
    bool ContainsNotFound { get; }

    /// <summary>
    /// Specifies the type or classification of the result,
    /// such as <see cref="ResultType.Success"/>,
    /// <see cref="ResultType.Error"/>, or <see cref="ResultType.ValidationError"/>.
    /// </summary>
    ResultType ResultType { get; }
}

/// <summary>
/// Defines the generic contract for an operation result that includes a data payload.
/// </summary>
/// <typeparam name="T">
/// The type of the data returned by the operation.
/// </typeparam>
/// <remarks>
/// This interface extends <see cref="IResult"/> by including a strongly-typed
/// <see cref="Data"/> property.  
/// It allows returning both status information and operation results
/// in a unified, consistent structure.
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
///     Console.WriteLine($"Error: {result.Message}");
/// }
/// </code>
/// </example>
public interface IResult<out T> : IResult
{
    /// <summary>
    /// The data returned by the operation, if applicable.
    /// </summary>
    T? Data { get; }
}
