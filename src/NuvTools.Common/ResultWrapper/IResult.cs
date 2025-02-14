﻿namespace NuvTools.Common.ResultWrapper;

/// <summary>
/// Specifies the severity of a rule.
/// </summary>
public enum Severity
{
    Error,
    Warning,
    Information,
    Critical
}

public enum ResultType
{
    Success = 0,
    Error = 1,
    ValidationError = 2
}

public record MessageDetail(string Title, string? Detail = null, string? Code = null, Severity? Severity = null);

public interface IResult
{
    List<MessageDetail> Messages { get; }

    MessageDetail? MessageDetail { get; }

    string? Message { get; }

    bool Succeeded { get; }
    bool ContainsNotFound { get; }

    ResultType ResultType { get; }
}

public interface IResult<out T> : IResult
{
    T? Data { get; }
}