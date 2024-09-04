
using Microsoft.Extensions.Logging;

namespace NuvTools.Common.ResultWrapper;

public abstract class ResultBase : IResult
{
    public bool Succeeded { get; protected set; }
    public ResultType ResultType { get; protected set; }
    public List<MessageDetail> Messages { get; protected set; } = [];

    protected static void Log(ResultType resultType, IEnumerable<MessageDetail>? messages, ILogger? logger)
    {
        if (logger == null || messages == null) return;

        foreach (var messageDetail in messages)
        {
            var message = messageDetail.Title + (!string.IsNullOrEmpty(messageDetail.Detail) ? " - " + messageDetail.Detail : string.Empty);
            switch (messageDetail.Severity)
            {
                case Severity.Error:
                    logger.LogError(message);
                    break;
                case Severity.Warning:
                    logger.LogWarning(message);
                    break;
                case Severity.Information:
                    logger.LogInformation(message);
                    break;
                case Severity.Critical:
                    logger.LogCritical(message);
                    break;
            }
        }
    }
}

public class Result : ResultBase, IResult
{
    private Result(bool succeeded, ResultType resultType, List<MessageDetail>? messages)
    {
        Succeeded = succeeded;
        ResultType = resultType;
        Messages = messages ?? [];
    }

    public static IResult Create(bool succeeded, ResultType resultType, List<MessageDetail>? messages = null, ILogger? logger = null)
    {
        Log(resultType, messages, logger);
        return new Result(succeeded, resultType, messages);
    }

    public static IResult Success(List<MessageDetail>? messages = null) => Create(true, ResultType.Success, messages);
    public static IResult Fail(List<MessageDetail>? messages = null, ILogger? logger = null) => Create(false, ResultType.Error, messages, logger);
    public static IResult ValidationFail(List<MessageDetail>? messages = null, ILogger? logger = null) => Create(false, ResultType.ValidationError, messages, logger);

    public static Task<IResult> SuccessAsync(List<MessageDetail>? messages = null) => Task.FromResult(Success(messages));
    public static Task<IResult> FailAsync(List<MessageDetail>? messages = null, ILogger? logger = null) => Task.FromResult(Fail(messages, logger));
    public static Task<IResult> ValidationFailAsync(List<MessageDetail>? messages = null, ILogger? logger = null) => Task.FromResult(ValidationFail(messages, logger));
}

public class Result<T> : ResultBase, IResult<T>
{
    public T Data { get; private set; }

    private Result(bool succeeded, ResultType resultType, T data, List<MessageDetail>? messages) : base()
    {
        Succeeded = succeeded;
        ResultType = resultType;
        Data = data;
        Messages = messages ?? [];
    }

    public static IResult<T> Create(bool succeeded, ResultType resultType, T data, List<MessageDetail>? messages = null, ILogger? logger = null)
    {
        Log(resultType, messages, logger);
        return new Result<T>(succeeded, resultType, data, messages);
    }

    public static IResult<T> Success(T data, List<MessageDetail>? messages = null) => Create(true, ResultType.Success, data, messages);
    public static IResult<T> Fail(T data, List<MessageDetail>? messages = null, ILogger? logger = null) => Create(false, ResultType.Error, data, messages, logger);
    public static IResult<T> ValidationFail(T data, List<MessageDetail>? messages = null, ILogger? logger = null) => Create(false, ResultType.ValidationError, data, messages, logger);

    public static Task<IResult<T>> SuccessAsync(T data, List<MessageDetail>? messages = null) => Task.FromResult(Success(data, messages));
    public static Task<IResult<T>> FailAsync(T data, List<MessageDetail>? messages = null, ILogger? logger = null) => Task.FromResult(Fail(data, messages, logger));
    public static Task<IResult<T>> ValidationFailAsync(T data, List<MessageDetail>? messages = null, ILogger? logger = null) => Task.FromResult(ValidationFail(data, messages, logger));
}