namespace Gilgamesh.Results.Errors;


/// <summary>
/// يمثل خطأً منطقياً أو تشغيلياً ناتجاً عن تنفيذ عملية ضمن النطاق.
/// Represents a domain or operation error resulting from executing a business operation.
/// </summary>
/// <param name="Code">رمز فريد يحدد سبب الخطأ بدقة (مثال: Users.NotFound) | Unique code identifying the specific error reason.</param>
/// <param name="Message">رسالة توضح تفاصيل الخطأ للمطور أو المستخدم | Human-readable message describing the error.</param>
/// <param name="Kind">تصنيف الخطأ العام لتحديد طريقة معالجته | Error category used to determine how it is handled.</param>
public sealed record DomainError(string Code, string Message, ErrorKind Kind)
{

    /// <summary>
    /// يمثل عدم وجود خطأ (قيمة محايدة / فارغة).
    /// Represents no error (neutral / empty state).
    /// </summary>
    public static readonly DomainError None = new(string.Empty, string.Empty, ErrorKind.Failure);


    /// <summary>
    /// خطأ شائع عند تمرير قيمة null في موضع يتوقع قيمة صالحة.
    /// Predefined error when a null value is provided where a valid value is expected.
    /// </summary>
    public static readonly DomainError NullValue = new("General.NullValue", "A null value was provided.", ErrorKind.Validation);


    /// <summary>
    /// ينشئ خطأ فشل عام أو غير مصنف.
    /// Creates a general or uncategorized failure error.
    /// </summary>
    public static DomainError Failure(string code, string message) => new(code, message, ErrorKind.Failure);


    /// <summary>
    /// ينشئ خطأ ناتجاً عن فشل التحقق من صحة المدخلات أو الشروط.
    /// Creates an input or condition validation error.
    /// </summary>
    public static DomainError Validation(string code, string message) => new(code, message, ErrorKind.Validation);


    /// <summary>
    /// ينشئ خطأ عندما لا يتم العثور على العنصر أو السجل المطلوب.
    /// Creates an error when the requested item or record is not found.
    /// </summary>
    public static DomainError NotFound(string code, string message) => new(code, message, ErrorKind.NotFound);


    /// <summary>
    /// ينشئ خطأ عند وجود تعارض مع حالة أو بيانات موجودة مسبقاً.
    /// Creates an error when there is a conflict with existing state or data.
    /// </summary>
    public static DomainError Conflict(string code, string message) => new(code, message, ErrorKind.Conflict);


    /// <summary>
    /// ينشئ خطأ عندما تكون هوية المستخدم غير مصادق عليها أو غير صالحة.
    /// Creates an unauthorized error when identity is missing or invalid.
    /// </summary>
    public static DomainError Unauthorized(string code, string message) => new(code, message, ErrorKind.Unauthorized);


    /// <summary>
    /// ينشئ خطأ عندما لا يملك المستخدم الصلاحية لتنفيذ العملية.
    /// Creates a forbidden error when the user lacks required permissions.
    /// </summary>
    public static DomainError Forbidden(string code, string message) => new(code, message, ErrorKind.Forbidden);


    /// <summary>
    /// ينشئ خطأ عندما تكون الخدمة الخارجية أو المورد غير متاح حالياً.
    /// Creates an unavailable error when an external service or resource is unreachable.
    /// </summary>
    public static DomainError Unavailable(string code, string message) => new(code, message, ErrorKind.Unavailable);


    /// <summary>
    /// يتحقق مما إذا كان هذا الخطأ يطابق تصنيف <see cref="ErrorKind"/> المحدد.
    /// Checks whether this error matches the specified <see cref="ErrorKind"/>.
    /// </summary>
    public bool Is(ErrorKind type) => Kind == type;


    /// <summary>
    /// يتحقق مما إذا كان هذا الخطأ يمثل غياب الخطأ (<see cref="None"/>).
    /// Checks whether this error represents no error (<see cref="None"/>).
    /// </summary>
    public bool IsNone() => this == None;


}

