namespace NuvTools.Common.ResultWrapper;

using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

public class Result : IResult
{
    public Result()
    {
    }

    public bool Succeeded { get; set; }

    public ResultType ResultType { get; set; } = ResultType.Success;

    public List<string> Messages { get; set; } = new List<string>();

    protected static void WriteLog(ResultType resultType, List<string> messages, ILogger logger)
    {
        if (messages == null || logger == null) return;

        var message = string.Join("\n", messages);

        if (message.Trim().Length == 0) return;

        switch (resultType)
        {
            case ResultType.Error:
                logger.LogError(message);
                break;
            case ResultType.Validation:
                logger.LogWarning(message);
                break;
            default:
                break;
        }
    }

    private static IResult CreateResult(ResultType resultType, List<string> messages = null, ILogger logger = null)
    {
        WriteLog(resultType, messages, logger);
        return new Result { Succeeded = resultType == ResultType.Success, ResultType = resultType, Messages = messages };
    }

    #region Fail

    public static IResult Fail(List<string> messages, ILogger logger = null)
    {
        return CreateResult(ResultType.Error, messages: messages, logger: logger);
    }

    public static IResult Fail()
    {
        return Fail(new List<string>());
    }

    public static Task<IResult> FailAsync()
    {
        return Task.FromResult(Fail());
    }

    public static IResult Fail(string message, ILogger logger = null)
    {
        return Fail(messages: new List<string> { message }, logger: logger);
    }

    public static Task<IResult> FailAsync(string message, ILogger logger = null)
    {
        return Task.FromResult(Fail(message, logger));
    }

    #endregion

    #region Validation Fail

    public static IResult ValidationFail(List<string> messages, ILogger logger = null)
    {
        return CreateResult(ResultType.Validation, messages: messages, logger: logger);
    }

    public static IResult ValidationFail(string message, ILogger logger = null)
    {
        return ValidationFail(messages: new List<string> { message }, logger: logger);
    }

    public static Task<IResult> ValidationFailAsync(string message, ILogger logger = null)
    {
        return Task.FromResult(ValidationFail(message, logger));
    }

    public static Task<IResult> ValidationFailAsync(List<string> messages, ILogger logger = null)
    {
        return Task.FromResult(ValidationFail(messages, logger));
    }

    #endregion

    #region Success

    public static IResult Success()
    {
        return new Result { Succeeded = true };
    }

    public static IResult Success(string message)
    {
        return new Result { Succeeded = true, Messages = new List<string> { message } };
    }

    public static Task<IResult> SuccessAsync()
    {
        return Task.FromResult(Success());
    }

    public static Task<IResult> SuccessAsync(string message)
    {
        return Task.FromResult(Success(message));
    }

    #endregion
}

public class Result<T> : Result, IResult<T>
{
    public Result()
    {
    }

    public T Data { get; set; }

    private static IResult<T> CreateResult(ResultType resultType, object data = null, List<string> messages = null, ILogger logger = null)
    {
        WriteLog(resultType, messages, logger);
        var result = new Result<T> { Succeeded = resultType == ResultType.Success, ResultType = resultType, Messages = messages };

        if (data != null) result.Data = (T)data;

        return result;
    }

    #region Fail

    public new static IResult<T> Fail(List<string> messages, ILogger logger = null)
    {
        return CreateResult(ResultType.Error, messages: messages, logger: logger);
    }

    public new static IResult<T> Fail()
    {
        return Fail(new List<string>());
    }

    public new static Task<IResult<T>> FailAsync()
    {
        return Task.FromResult(Fail());
    }

    public new static IResult<T> Fail(string message, ILogger logger = null)
    {
        return Fail(messages: new List<string> { message }, logger: logger);
    }

    public new static Task<IResult<T>> FailAsync(string message, ILogger logger = null)
    {
        return Task.FromResult(Fail(message, logger));
    }

    public static IResult<T> Fail(T data)
    {
        return new Result<T> { ResultType = ResultType.Error, Succeeded = false, Data = data };
    }

    public static IResult<T> Fail(T data, string message)
    {
        return new Result<T> { ResultType = ResultType.Error, Succeeded = false, Data = data, Messages = new List<string> { message } };
    }

    public static IResult<T> Fail(T data, List<string> messages)
    {
        return new Result<T> { ResultType = ResultType.Error, Succeeded = false, Data = data, Messages = messages };
    }

    public static Task<IResult<T>> FailAsync(T data)
    {
        return Task.FromResult(Fail(data));
    }

    public static IResult<T> Fail(string message, T data)
    {
        return new Result<T> { ResultType = ResultType.Error, Succeeded = false, Data = data, Messages = new List<string> { message } };
    }

    public static Task<IResult<T>> FailAsync(T data, string message)
    {
        return Task.FromResult(Fail(message, data));
    }

    #endregion

    #region Validation Fail

    public new static IResult<T> ValidationFail(List<string> messages, ILogger logger = null)
    {
        return CreateResult(ResultType.Validation, messages: messages, logger: logger);
    }

    public new static IResult<T> ValidationFail(string message, ILogger logger = null)
    {
        return ValidationFail(messages: new List<string> { message }, logger: logger);
    }

    public new static Task<IResult<T>> ValidationFailAsync(string message, ILogger logger = null)
    {
        return Task.FromResult(ValidationFail(message, logger));
    }

    public new static Task<IResult<T>> ValidationFailAsync(List<string> messages, ILogger logger = null)
    {
        return Task.FromResult(ValidationFail(messages, logger));
    }

    public static IResult<T> ValidationFail(T data)
    {
        return new Result<T> { ResultType = ResultType.Validation, Succeeded = false, Data = data };
    }

    public static IResult<T> ValidationFail(T data, string message)
    {
        return new Result<T> { ResultType = ResultType.Validation, Succeeded = false, Data = data, Messages = new List<string> { message } };
    }

    public static IResult<T> ValidationFail(T data, List<string> messages)
    {
        return new Result<T> { ResultType = ResultType.Validation, Succeeded = false, Data = data, Messages = messages };
    }

    public static Task<IResult<T>> ValidationFailAsync(T data)
    {
        return Task.FromResult(ValidationFail(data));
    }

    public static IResult<T> ValidationFail(string message, T data)
    {
        return new Result<T> { ResultType = ResultType.Validation, Succeeded = false, Data = data, Messages = new List<string> { message } };
    }

    public static Task<IResult<T>> ValidationFailAsync(T data, string message)
    {
        return Task.FromResult(ValidationFail(message, data));
    }

    #endregion

    #region Success

    public static new IResult<T> Success()
    {
        return new Result<T> { Succeeded = true };
    }

    public static new IResult<T> Success(string message)
    {
        return new Result<T> { Succeeded = true, Messages = new List<string> { message } };
    }

    public static IResult<T> Success(T data)
    {
        return new Result<T> { Succeeded = true, Data = data };
    }

    public static IResult<T> Success(T data, string message)
    {
        return new Result<T> { Succeeded = true, Data = data, Messages = new List<string> { message } };
    }

    public static IResult<T> Success(T data, List<string> messages)
    {
        return new Result<T> { Succeeded = true, Data = data, Messages = messages };
    }

    public static new Task<IResult<T>> SuccessAsync()
    {
        return Task.FromResult(Success());
    }

    public static new Task<IResult<T>> SuccessAsync(string message)
    {
        return Task.FromResult(Success(message));
    }

    public static Task<IResult<T>> SuccessAsync(T data)
    {
        return Task.FromResult(Success(data));
    }

    public static Task<IResult<T>> SuccessAsync(T data, string message)
    {
        return Task.FromResult(Success(data, message));
    }

    #endregion
}