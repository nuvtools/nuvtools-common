namespace NuvTools.Common.Reflection;

public class VersionException : Exception
{
    public VersionException() : base()
    {
    }

    public VersionException(string message) : base(message)
    {
    }

    public VersionException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public VersionException(Exception innerException) : base(null, innerException)
    {
    }
}
