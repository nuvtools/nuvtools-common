using Microsoft.Extensions.Logging;
using NuvTools.Common.ResultWrapper.Enumerations;
using System.Text.Json.Serialization;

namespace NuvTools.Common.ResultWrapper;

/// <summary>
/// Base class for operation results, containing shared properties and logic.
/// </summary>
public abstract class ResultBase
{
    /// <summary>
    /// Indicates whether the operation succeeded.
    /// </summary>
    public bool Succeeded { get; set; }

    /// <summary>
    /// Indicates whether the result contains a "not found" message.
    /// </summary>
    public bool ContainsNotFound { get; set; }

    /// <summary>
    /// Represents the type of the result (Success, Error, ValidationError, etc.).
    /// </summary>
    public ResultType ResultType { get; set; } = ResultType.Success;

    /// <summary>
    /// List of message details returned by the operation.
    /// </summary>
    public List<MessageDetail> Messages { get; set; } = [];

    /// <summary>
    /// Returns the first message detail (usually the main one).
    /// Ignored during JSON serialization to avoid redundancy.
    /// </summary>
    [JsonIgnore]
    public MessageDetail? MessageDetail => Messages.FirstOrDefault();

    /// <summary>
    /// Returns a formatted string with the first message (e.g., "Title - Detail").
    /// Ignored in JSON serialization to prevent duplication.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Message => MessageDetail == null
        ? null
        : $"{MessageDetail.Title}{(MessageDetail.Detail != null ? $" - {MessageDetail.Detail}" : string.Empty)}";

    #region Protected helpers

    /// <summary>
    /// Logs all message details using the specified logger.
    /// </summary>
    protected static void Log(List<MessageDetail>? messages, ILogger? logger)
    {
        if (logger == null || messages == null) return;

        foreach (var messageDetail in messages)
        {
            var message = messageDetail.Title +
                          (!string.IsNullOrEmpty(messageDetail.Detail) ? $" - {messageDetail.Detail}" : string.Empty);

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

    /// <summary>
    /// Converts a list of strings into a list of message details.
    /// </summary>
    protected static List<MessageDetail> ConvertToMessageDetail(IEnumerable<string> value)
        => [.. value.Select(e => new MessageDetail(e))];

    /// <summary>
    /// Creates a new instance of a result.
    /// </summary>
    protected static T CreateResult<T>(
        ResultType resultType,
        T instance,
        List<MessageDetail>? messages = null,
        ILogger? logger = null) where T : ResultBase
    {
        Log(messages, logger);

        instance.Succeeded = resultType == ResultType.Success;
        instance.ContainsNotFound = resultType != ResultType.Success &&
                                    messages?.Any(e => e.Code == "404") == true;
        instance.ResultType = resultType;
        instance.Messages = messages ?? [];

        return instance;
    }

    #endregion
}
