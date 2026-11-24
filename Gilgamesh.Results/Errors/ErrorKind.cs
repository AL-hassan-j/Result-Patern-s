namespace Gilgamesh.Results.Errors;

/// <summary>
/// يحدد تصنيف الخطأ لتسهيل فهمه ومعالجته (مثل تحويله إلى رمز حالة مناسب).
/// Specifies the error category to simplify handling and mapping (e.g., to status codes).
/// </summary>
public enum ErrorKind
{
    /// <summary>
    /// General or uncategorized failure; used as a fallback when an error does not fit a more specific category such as validation, not found, or conflict.
    /// </summary>
    Failure = 0,

    /// <summary>
    /// Input, condition, or validation rule error; used when provided data is missing, invalid, or violates required constraints.
    /// </summary>
    Validation = 1,

    /// <summary>
    /// Requested item, record, or resource not found; used when an entity cannot be located by the given identifier or criteria.
    /// </summary>
    NotFound = 2,

    /// <summary>
    /// Conflict with existing state or data; used for cases such as duplicate email or an operation conflicting with the resource's current state.
    /// </summary>
    Conflict = 3,

    /// <summary>
    /// Unauthenticated; used when the caller's identity is unknown, missing, or invalid (such as missing or expired authentication credentials).
    /// </summary>
    Unauthorized = 4,

    /// <summary>
    /// Forbidden; used when the caller is authenticated but lacks the required permission or role to perform the requested operation.
    /// </summary>
    Forbidden = 5,

    /// <summary>
    /// External service, resource, or dependency currently unavailable; used when a downstream service, database, or resource cannot be reached temporarily.
    /// </summary>
    Unavailable = 6
}