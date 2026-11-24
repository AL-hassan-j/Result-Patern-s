
namespace Gilgamesh.Results.Results;


/// <summary>
/// Base class for operation results, holding success/failure state and error details.
/// </summary>
public abstract class ResultBase
{

    /// <summary>
    /// Initializes a new instance of <see cref="ResultBase"/> while enforcing state invariants between success status and error.
    /// </summary>
    /// <param name="isSuccess">Indicates whether the operation succeeded.</param>
    /// <param name="error">The associated domain error, which must be <see cref="DomainError.None"/> on success and non-none on failure.</param>
    /// <exception cref="InvalidOperationException">
    /// Thrown when <paramref name="isSuccess"/> is <see langword="true"/> with a non-none <paramref name="error"/>,
    /// or when <paramref name="isSuccess"/> is <see langword="false"/> and <paramref name="error"/> is <see cref="DomainError.None"/>.
    /// </exception>
    protected ResultBase(bool isSuccess, DomainError error)
    {

        if (isSuccess && error != DomainError.None)
            throw new InvalidOperationException("A successful result cannot contain an error.");

        if (!isSuccess && error == DomainError.None)
            throw new InvalidOperationException("A failed result must specify an error.");

        IsSuccess = isSuccess;
        Error = error;

    }

    /// <summary>
    /// Indicates whether the operation succeeded.
    /// </summary>
    public bool IsSuccess { get; }


    /// <summary>
    /// Indicates whether the operation failed.
    /// </summary>
    public bool IsFailure => !IsSuccess;


    /// <summary>
    /// Represents the error associated with the result, or <see cref="DomainError.None"/> on success.
    /// </summary>
    public DomainError Error { get; }


}
