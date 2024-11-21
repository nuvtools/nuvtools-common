namespace NuvTools.Common.Exceptions;

public static class ExceptionExtensions
{
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
