namespace Gilgamesh.Results.Results;



/// <summary>
/// يمثل نتيجة عملية ترجع قيمة من النوع <typeparamref name="T"/> في حال النجاح، أو خطأً من نوع <see cref="DomainError"/> في حال الفشل.
/// Represents the result of an operation returning a value of type <typeparamref name="T"/> on success, or a <see cref="DomainError"/> on failure.
/// </summary>
/// <typeparam name="T">نوع القيمة المرجعة عند نجاح العملية | Type of the value returned on success.</typeparam>
public class Result<T> : ResultBase
{

    private readonly T? _value;


    /// <summary>
    /// ينشئ نسخة جديدة من <see cref="Result{T}"/>.
    /// Initializes a new instance of <see cref="Result{T}"/>.
    /// </summary>
    public Result(T? value , bool isSuccess, DomainError error) : base (isSuccess, error)
    {

        _value = value; 

    }

    /// <summary>
    /// ينشئ نتيجة ناجحة تحمل القيمة المحددة.
    /// Creates a successful result containing the specified value.
    /// </summary>
    public static Result<T> Success(T value) => new(value, true, DomainError.None);


    /// <summary>
    /// القيمة الناتجة عند نجاح العملية؛ يرمي استثناءً إذا حاولت قراءتها والنتيجة فاشلة.
    /// The value produced when the operation succeeds; throws if accessed on a failed result.
    /// </summary>
    public T Value => IsSuccess ? _value! : throw new InvalidOperationException("Cannot access Value of a failed result.");




    /// <summary>
    /// ينشئ نتيجة فاشلة تحمل الخطأ المحدد.
    /// Creates a failed result containing the specified error.
    /// </summary>
    public static new Result<T> Failure(DomainError error) => new(default, false, error);




    /// <summary>
    /// يحوّل قيمة من النوع <typeparamref name="T"/> ضمنياً إلى <see cref="Result{T}"/> ناجحة.
    /// Implicitly converts a value of type <typeparamref name="T"/> into a successful <see cref="Result{T}"/>.
    /// </summary>
    public static implicit operator Result<T>(T value) => Success(value);


    /// <summary>
    /// يحوّل <see cref="DomainError"/> ضمنياً إلى <see cref="Result{T}"/> فاشلة.
    /// Implicitly converts a <see cref="DomainError"/> into a failed <see cref="Result{T}"/>.
    /// </summary>
    public static implicit operator Result<T>(DomainError error) => Failure(error);



}
