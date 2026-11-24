namespace Gilgamesh.Results.Results;

/// <summary>
/// Represents the result of an operation that does not return a value (such as delete, update, or command operations), either succeeding or failing with a <see cref="DomainError"/>.
/// </summary>
/// <param name="isSuccess"> Indicates whether the operation succeeded or failed.</param>
/// <param name="error"> The error associated with the result, or <see cref="DomainError.None"/> on success.</param>
public class Result(bool isSuccess, DomainError error) : ResultBase(isSuccess, error)
{
    /// <summary>
    /// Creates a successful result with no return value.
    /// </summary>
    public static Result Success() => new(true, DomainError.None);


    /// <summary>
    /// Creates a failed result containing the specified error.
    /// </summary>
    public static Result Failure(DomainError error) => new(false, error);

    /// <summary>
    /// Implicitly converts a <see cref="DomainError"/> into a failed <see cref="Result"/>.
    /// </summary>
    public static implicit operator Result(DomainError error) => Failure(error);


}