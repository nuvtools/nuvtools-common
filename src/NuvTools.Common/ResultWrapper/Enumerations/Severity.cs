namespace NuvTools.Common.ResultWrapper.Enumerations;

/// <summary>
/// Represents the severity level of an event or message.
/// </summary>
/// <remarks>This enumeration is commonly used to categorize the importance or urgency of events,  such as log
/// messages or application alerts. The severity levels range from informational  messages to critical errors.</remarks>
public enum Severity
{
    Error,
    Warning,
    Information,
    Critical
}
