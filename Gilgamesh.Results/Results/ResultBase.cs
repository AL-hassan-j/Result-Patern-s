using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Gilgamesh.Results.Results;


/// <summary>
/// الكلاس الأساسي المشترك لنتائج العمليات، ويحمل حالة النجاح أو الفشل وتفاصيل الخطأ.
/// Base class for operation results, holding success/failure state and error details.
/// </summary>
public abstract class ResultBase
{


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
    /// يشير إلى ما إذا كانت العملية قد نجحت.
    /// Indicates whether the operation succeeded.
    /// </summary>
    public bool IsSuccess { get; }


    /// <summary>
    /// يشير إلى ما إذا كانت العملية قد فشلت.
    /// Indicates whether the operation failed.
    /// </summary>
    public bool IsFailure => !IsSuccess;


    /// <summary>
    /// يمثل الخطأ المرتبط بالنتيجة، أو <see cref="DomainError.None"/> في حال النجاح.
    /// Represents the error associated with the result, or <see cref="DomainError.None"/> on success.
    /// </summary>
    public DomainError Error { get; }


}
