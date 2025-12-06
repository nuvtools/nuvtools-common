namespace NuvTools.Common.Reflection;

/// <summary>
/// Exception thrown when there is an error retrieving or processing assembly version information.
/// </summary>
public class VersionException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VersionException"/> class.
    /// </summary>
    public VersionException() : base()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VersionException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public VersionException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VersionException"/> class with a specified error message
    /// and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public VersionException(string message, Exception innerException) : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VersionException"/> class with a reference to the inner exception.
    /// </summary>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public VersionException(Exception innerException) : base(null, innerException)
    {
    }
}
