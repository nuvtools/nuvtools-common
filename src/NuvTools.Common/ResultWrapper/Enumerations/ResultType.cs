namespace NuvTools.Common.ResultWrapper.Enumerations;

/// <summary>
/// Represents the result type of an operation, indicating its outcome.
/// </summary>
/// <remarks>This enumeration is used to classify the result of an operation into one of the predefined
/// categories: <see cref="Success"/>, <see cref="Error"/>, or <see cref="ValidationError"/>.</remarks>
public enum ResultType
{
    Success = 0,
    Error = 1,
    ValidationError = 2
}
