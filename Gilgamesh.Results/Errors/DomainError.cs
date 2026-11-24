namespace Gilgamesh.Results.Errors;


/// <summary>
/// Represents a domain or operation error resulting from executing a business operation.
/// </summary>
/// <param name="Code"> Unique code identifying the specific error reason.</param>
/// <param name="Message"> Human-readable message describing the error.</param>
/// <param name="Kind">Error category used to determine how it is handled.</param>
public sealed record DomainError(string Code, string Message, ErrorKind Kind)
{

    /// <summary>
    /// Represents no error (neutral / empty state).
    /// </summary>
    public static readonly DomainError None = new(string.Empty, string.Empty, ErrorKind.Failure);


    /// <summary>
    /// Predefined error when a null value is provided where a valid value is expected.
    /// </summary>
    public static readonly DomainError NullValue = new("General.NullValue", "A null value was provided.", ErrorKind.Validation);


    /// <summary>
    /// Creates a general or uncategorized failure error.
    /// </summary>
    public static DomainError Failure(string code, string message) => new(code, message, ErrorKind.Failure);


    /// <summary>
    /// Creates an input or condition validation error.
    /// </summary>
    public static DomainError Validation(string code, string message) => new(code, message, ErrorKind.Validation);


    /// <summary>
    /// Creates an error when the requested item or record is not found.
    /// </summary>
    public static DomainError NotFound(string code, string message) => new(code, message, ErrorKind.NotFound);


    /// <summary>
    /// Creates an error when there is a conflict with existing state or data.
    /// </summary>
    public static DomainError Conflict(string code, string message) => new(code, message, ErrorKind.Conflict);


    /// <summary>
    /// Creates an unauthorized error when identity is missing or invalid.
    /// </summary>
    public static DomainError Unauthorized(string code, string message) => new(code, message, ErrorKind.Unauthorized);


    /// <summary>
    /// Creates a forbidden error when the user lacks required permissions.
    /// </summary>
    public static DomainError Forbidden(string code, string message) => new(code, message, ErrorKind.Forbidden);


    /// <summary>
    /// Creates an unavailable error when an external service or resource is unreachable.
    /// </summary>
    public static DomainError Unavailable(string code, string message) => new(code, message, ErrorKind.Unavailable);


    /// <summary>
    /// Checks whether this error matches the specified <see cref="ErrorKind"/>.
    /// </summary>
    public bool Is(ErrorKind type) => Kind == type;


    /// <summary>
    /// Checks whether this error represents no error (<see cref="None"/>).
    /// </summary>
    public bool IsNone() => this == None;


}

