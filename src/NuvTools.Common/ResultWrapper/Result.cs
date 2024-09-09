using Microsoft.Extensions.Logging;

namespace NuvTools.Common.ResultWrapper;

public class Result : IResult
{
    public bool Succeeded { get; set; }
    public ResultType ResultType { get; set; } = ResultType.Success;
    public List<MessageDetail> Messages { get; set; } = [];

    protected static void Log(List<MessageDetail>? messages, ILogger? logger)
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

    protected static List<MessageDetail> ConvertToMessageDetail(IEnumerable<string> value)
    {
        return value.Select(e => new MessageDetail(e)).ToList();
    }

    private static Result CreateResult(ResultType resultType, List<MessageDetail>? messages = null, ILogger? logger = null)
    {
        Log(messages, logger);
        return new Result { Succeeded = resultType == ResultType.Success, ResultType = resultType, Messages = messages ?? [] };
    }

    public static IResult Fail(List<MessageDetail>? messages = null, ILogger? logger = null) => CreateResult(ResultType.Error, messages, logger);
    public static IResult Fail(List<string> messages, ILogger? logger = null) => Fail(ConvertToMessageDetail(messages), logger);
    public static IResult Fail(string message, ILogger? logger = null) => Fail([message], logger);
    public static IResult Fail(MessageDetail message, ILogger? logger = null) => Fail([message], logger);

    public static IResult ValidationFail(List<MessageDetail> messages, ILogger? logger = null) => CreateResult(ResultType.ValidationError, messages, logger);
    public static IResult ValidationFail(List<string> messages, ILogger? logger = null) => ValidationFail(ConvertToMessageDetail(messages), logger);
    public static IResult ValidationFail(MessageDetail message, ILogger? logger = null) => ValidationFail([message], logger);
    public static IResult ValidationFail(string message, ILogger? logger = null) => ValidationFail([message], logger);

    public static IResult Success(MessageDetail? message = null) => new Result { Succeeded = true, Messages = [message] };
    public static IResult Success(string message) => Success(new MessageDetail(message));
}

public class Result<T> : Result, IResult<T>
{
    public T? Data { get; set; }

    private static Result<T> CreateResult(ResultType resultType, T? data = default, List<MessageDetail>? messages = null, ILogger? logger = null)
    {
        Log(messages, logger);
        return new Result<T> { Succeeded = resultType == ResultType.Success, ResultType = resultType, Data = data, Messages = messages ?? [] };
    }

    public static IResult<T> Fail(List<MessageDetail>? messages = null, T? data = default, ILogger? logger = null) => CreateResult(ResultType.Error, data, messages, logger);
    public static IResult<T> Fail(List<string> messages, T? data = default, ILogger? logger = null) => Fail(ConvertToMessageDetail(messages), data, logger);
    public static IResult<T> Fail(MessageDetail message, T? data = default, ILogger? logger = null) => Fail([message], data, logger);
    public static IResult<T> Fail(string message, T? data = default, ILogger? logger = null) => Fail([new MessageDetail(message)], data, logger);

    public static IResult<T> ValidationFail(List<MessageDetail> messages, T? data = default, ILogger? logger = null) => CreateResult(ResultType.ValidationError, data, messages, logger);
    public static IResult<T> ValidationFail(List<string> messages, T? data = default, ILogger? logger = null) => ValidationFail(ConvertToMessageDetail(messages), data, logger);
    public static IResult<T> ValidationFail(MessageDetail message, T? data = default, ILogger? logger = null) => ValidationFail([message], data, logger);
    public static IResult<T> ValidationFail(string message, T? data = default, ILogger? logger = null) => ValidationFail([message], data, logger);

    public static IResult<T> Success(T? data = default, MessageDetail? message = null) => CreateResult(ResultType.Success, data, [message]);
    public static IResult<T> Success(T? data, string message) => Success(data, new MessageDetail(message));
    public static new IResult<T> Success(MessageDetail? message) => Success(default, message);
    public static new IResult<T> Success(string message) => Success(default, new MessageDetail(message));
}