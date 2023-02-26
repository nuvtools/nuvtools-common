namespace NuvTools.Common.ResultWrapper;
public enum ResultType
{
    Success = 0,
    Error = 1,
    Validation = 2
}

public interface IResult
{
    List<string> Messages { get; set; }

    bool Succeeded { get; set; }

    ResultType ResultType { get; set; }
}

public interface IResult<out T> : IResult
{
    T Data { get; }
}