namespace Gilgamesh.Results.Errors;

 /// <summary>
 /// يحدد تصنيف الخطأ لتسهيل فهمه ومعالجته (مثل تحويله إلى رمز حالة مناسب).
 /// Specifies the error category to simplify handling and mapping (e.g., to status codes).
 /// </summary>
 public enum ErrorKind
 {
    /// <summary>
    /// فشل عام أو غير مصنف ضمن حالة محددة؛ يُستخدم كخيار افتراضي عندما لا ينطبق على الخطأ تصنيف أدق مثل التحقق أو عدم الوجود أو التعارض.
    /// General or uncategorized failure; used as a fallback when an error does not fit a more specific category such as validation, not found, or conflict.
    /// </summary>
    Failure = 0,

    /// <summary>
    /// خطأ في صحة المدخلات أو الشروط أو قواعد التحقق؛ يُستخدم عندما تكون البيانات المرسلة ناقصة أو غير صالحة أو مخالفة للقيود المطلوبة.
    /// Input, condition, or validation rule error; used when provided data is missing, invalid, or violates required constraints.
    /// </summary>
    Validation = 1,

    /// <summary>
    /// العنصر أو السجل أو المورد المطلوب غير موجود؛ يُستخدم عندما لا يتم العثور على الكيان بالمعرف أو الشرط المحدد.
    /// 
    /// Requested item, record, or resource not found; used when an entity cannot be located by the given identifier or criteria.
    /// </summary>
    NotFound = 2,

    /// <summary>
    /// تعارض مع حالة أو بيانات موجودة مسبقاً؛ يُستخدم مثل تكرار بريد إلكتروني أو محاولة تنفيذ عملية تتعارض مع الحالة الحالية للمورد.
    /// Conflict with existing state or data; used for cases such as duplicate email or an operation conflicting with the resource's current state.
    /// </summary>
    Conflict = 3,

    /// <summary>
    /// غير مصادق عليه؛ يُستخدم عندما تكون هوية المستخدم غير معروفة أو غير موجودة أو غير صالحة (مثل غياب أو انتهاء صلاحية بيانات المصادقة).
    /// Unauthenticated; used when the caller's identity is unknown, missing, or invalid (such as missing or expired authentication credentials).
    /// </summary>
    Unauthorized = 4,

    /// <summary>
    /// مرفوض؛ يُستخدم عندما تكون هوية المستخدم معروفة ومصادقاً عليها لكنه لا يملك الصلاحية اللازمة لتنفيذ العملية المطلوبة.
    /// Forbidden; used when the caller is authenticated but lacks the required permission or role to perform the requested operation.
    /// </summary>
    Forbidden = 5,

    /// <summary>
    /// خدمة خارجية أو مورد أو اعتماد غير متاح حالياً؛ يُستخدم عند تعذر الوصول إلى خدمة خارجية أو قاعدة بيانات أو مورد مؤقتاً.
    /// External service, resource, or dependency currently unavailable; used when a downstream service, database, or resource cannot be reached temporarily.
    /// </summary>
    Unavailable = 6

}
