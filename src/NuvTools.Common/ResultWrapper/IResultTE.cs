namespace NuvTools.Common.ResultWrapper;

public interface IResult<T, E> : IResult<T>
{
    /// <summary>
    /// Strongly typed error payload provided by APIs that do not follow the default Result pattern.
    /// Only populated when the request fails and returns a different error model.
    /// </summary>
    E? ErrorPayload { get; }
}