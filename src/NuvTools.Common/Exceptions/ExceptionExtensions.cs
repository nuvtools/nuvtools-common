namespace NuvTools.Common.Exceptions;

/// <summary>
/// Provides extension methods for <see cref="Exception"/> objects.
/// </summary>
public static class ExceptionExtensions
{
    /// <summary>
    /// Aggregates exception messages including inner exceptions up to a specified level.
    /// </summary>
    /// <param name="exception">The exception to process.</param>
    /// <param name="level">The number of inner exception levels to include. Use 0 to include only the top-level exception message.</param>
    /// <returns>A formatted string containing the exception message(s) with level indicators.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="exception"/> is null.</exception>
    /// <remarks>
    /// When level is 0, only the top-level exception message is returned.
    /// When level is greater than 0, inner exceptions are included up to the specified depth,
    /// with each level clearly marked (e.g., "Level 0: ...", "Level 1: ...").
    /// </remarks>
    public static string AggregateExceptionMessages(this Exception exception, short level = 0)
    {
        ArgumentNullException.ThrowIfNull(exception);

        if (level <= 0)
            return $"Exception: {exception.Message}";

        var messages = new List<string>();
        var currentException = exception;
        int currentLevel = 0;

        while (currentException != null && currentLevel <= level)
        {
            messages.Add($"Level {currentLevel}: {currentException.Message}");
            currentException = currentException.InnerException;
            currentLevel++;
        }

        return string.Join(" -> ", messages);
    }

}
