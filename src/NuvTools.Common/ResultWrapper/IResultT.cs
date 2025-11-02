namespace NuvTools.Common.ResultWrapper;

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