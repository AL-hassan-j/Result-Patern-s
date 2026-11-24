namespace Gilgamesh.Results.Results;



/// <summary>
/// Represents the result of an operation returning a value of type <typeparamref name="T"/> on success, or a <see cref="DomainError"/> on failure.
/// </summary>
/// <typeparam name="T"> Type of the value returned on success.</typeparam>
/// <remarks>
/// Initializes a new instance of <see cref="Result{T}"/>.
/// </remarks>
public class Result<T>(T? value, bool isSuccess, DomainError error) : ResultBase(isSuccess, error)
{

    private readonly T? _value = value;

    /// <summary>
    /// Creates a successful result containing the specified value.
    /// </summary>
    public static Result<T> Success(T value) => new(value, true, DomainError.None);


    /// <summary>
    /// The value produced when the operation succeeds; throws if accessed on a failed result.
    /// </summary>
    public T Value => IsSuccess ? _value! : throw new InvalidOperationException("Cannot access Value of a failed result.");






    /// <summary>
    /// Creates a failed result containing the specified error.
    /// </summary>
    public static Result<T> Failure(DomainError error) => new(default, false, error);




    /// <summary>
    /// Implicitly converts a value of type <typeparamref name="T"/> into a successful <see cref="Result{T}"/>.
    /// </summary>
    public static implicit operator Result<T>(T value) => Success(value);


    /// <summary>
    /// Implicitly converts a <see cref="DomainError"/> into a failed <see cref="Result{T}"/>.
    /// </summary>
    public static implicit operator Result<T>(DomainError error) => Failure(error);


}
