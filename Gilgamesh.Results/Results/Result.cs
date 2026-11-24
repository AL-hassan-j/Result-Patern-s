namespace Gilgamesh.Results.Results;

/// <summary>
/// يمثل نتيجة عملية لا ترجع قيمة محددة (مثل عمليات الحذف أو التحديث أو التنفيذ)، إما بالنجاح أو بخطأ من نوع <see cref="DomainError"/>.
/// Represents the result of an operation that does not return a value (such as delete, update, or command operations), either succeeding or failing with a <see cref="DomainError"/>.
/// </summary>
/// <param name="isSuccess">يشير إلى نجاح العملية أو فشلها | Indicates whether the operation succeeded or failed.</param>
/// <param name="error">الخطأ المرتبط بالنتيجة، أو <see cref="DomainError.None"/> عند النجاح | The error associated with the result, or <see cref="DomainError.None"/> on success.</param>
public class Result(bool isSuccess, DomainError error) : ResultBase(isSuccess, error)
{
    /// <summary>
    /// ينشئ نتيجة ناجحة بدون قيمة مرجعة.
    /// Creates a successful result with no return value.
    /// </summary>
    public static Result Success() => new(true, DomainError.None);


    /// <summary>
    /// ينشئ نتيجة فاشلة تحمل الخطأ المحدد.
    /// Creates a failed result containing the specified error.
    /// </summary>
    /// <param name="error">سبب الفشل | The reason for failure.</param>
    public static Result Failure(DomainError error) => new(false, error);

    /// <summary>
    /// يحوّل <see cref="DomainError"/> ضمنياً إلى <see cref="Result"/> فاشلة.
    /// Implicitly converts a <see cref="DomainError"/> into a failed <see cref="Result"/>.
    /// </summary>
    /// <param name="error">الخطأ المراد تحويله إلى نتيجة فاشلة | The error to convert into a failed result.</param>
    public static implicit operator Result(DomainError error) => Failure(error);


}