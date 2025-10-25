using NuvTools.Common.ResultWrapper.Enumerations;

namespace NuvTools.Common.ResultWrapper;

/// <summary>
/// Represents a detailed message related to an operation result,
/// such as informational, success, warning, or error messages.
/// </summary>
/// <remarks>
/// The <see cref="MessageDetail"/> record is typically used as part of a
/// <see cref="Result"/> or <see cref="Result{T}"/> object to provide
/// structured feedback about an operation.
/// It allows developers to capture human-readable messages along with
/// metadata such as message code and severity.
/// </remarks>
/// <example>
/// Example usage:
/// <code>
/// var message = new MessageDetail(
///     Title: "Validation failed",
///     Detail: "The email address format is invalid.",
///     Code: "VAL001",
///     Severity: Severity.Warning
/// );
/// </code>
/// </example>
/// <param name="Title">
/// The short, human-readable message summarizing the event or result.
/// </param>
/// <param name="Detail">
/// An optional detailed description that provides more context or
/// technical information about the message.
/// </param>
/// <param name="Code">
/// An optional identifier that uniquely represents the message type or
/// category, useful for localization, logging, or client-side handling.
/// </param>
/// <param name="Severity">
/// The severity level of the message (e.g., <see cref="Severity.Information"/>,
/// <see cref="Severity.Warning"/>, <see cref="Severity.Error"/>).
/// </param>
public record MessageDetail(
    string Title,
    string? Detail = null,
    string? Code = null,
    Severity? Severity = null
);