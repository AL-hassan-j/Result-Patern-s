
# English

## Overview

**Gilgamesh.Results** is a lightweight, reusable implementation of the Result Pattern for .NET applications. It represents successful operations, failed operations, returned values, and categorized domain or operation errors without using exceptions for expected failure cases.

The library has no external runtime dependencies and targets:

```xml
<TargetFramework>net10.0</TargetFramework>
```

## Release Status

**Gilgamesh.Results is currently moving from Alpha to Beta.** The test project currently contains 17 tests covering successful and failed results, value access, invalid result states, implicit conversions, and selected `DomainError` behavior. The API may still change before a stable release.

## Installation

Reference the project directly:

```xml
<ProjectReference Include="path\to\Gilgamesh.Results.csproj" />
```

Future NuGet package distribution may provide a package reference based on the project release configuration.

## Namespaces

```csharp
using Gilgamesh.Results.Results;
using Gilgamesh.Results.Errors;
```

## Types

| Type | Namespace | Purpose |
|---|---|---|
| `Result` | `Gilgamesh.Results.Results` | Represents an operation that does not return a value |
| `Result<T>` | `Gilgamesh.Results.Results` | Represents an operation that returns a value of type `T` |
| `ResultBase` | `Gilgamesh.Results.Results` | Shared abstract base type for results |
| `DomainError` | `Gilgamesh.Results.Errors` | Represents a domain or operation error |
| `ErrorKind` | `Gilgamesh.Results.Errors` | Categorizes an error |

---

## `ResultBase`

`ResultBase` is the common abstract base for every result.

```csharp
public abstract class ResultBase
{
    public bool IsSuccess { get; }
    public bool IsFailure { get; }
    public DomainError Error { get; }
}
```

The constructor enforces two invariants:

- A successful result cannot contain an error other than `DomainError.None`.
- A failed result must contain a specific error.

Violating either rule throws `InvalidOperationException`.

---

## `Result`

Use `Result` for operations that do not return a value, such as delete, update, or command operations.

### Create success

```csharp
Result result = Result.Success();
```

### Create failure

```csharp
var error = DomainError.NotFound(
    "Users.NotFound",
    "The requested user was not found.");

Result result = Result.Failure(error);
```

`DomainError` can also be converted implicitly to a failed `Result`:

```csharp
Result DeleteUser(Guid id)
{
    if (id == Guid.Empty)
        return DomainError.Validation(
            "Users.InvalidId",
            "The user ID is invalid.");

    return Result.Success();
}
```

### Members

| Member | Type | Description |
|---|---|---|
| `Success()` | `Result` | Creates a successful result |
| `Failure(error)` | `Result` | Creates a failed result |
| Implicit `DomainError` conversion | `Result` | Converts an error into a failed result |
| `IsSuccess` | `bool` | Indicates success |
| `IsFailure` | `bool` | Indicates failure |
| `Error` | `DomainError` | Contains the associated error |

---

## `Result<T>`

Use `Result<T>` when an operation should return a value of type `T` on success.

### Create success with a value

```csharp
var result = Result<int>.Success(42);

if (result.IsSuccess)
{
    Console.WriteLine(result.Value);
}
```

A value can be converted implicitly to a successful result:

```csharp
Result<string> result = "User created";
Console.WriteLine(result.Value);
```

### Create failure

```csharp
var result = Result<User>.Failure(
    DomainError.NotFound(
        "Users.NotFound",
        "The user was not found."));
```

Or use implicit conversion:

```csharp
Result<User> result =
    DomainError.NotFound(
        "Users.NotFound",
        "The user was not found.");
```

### Access the value

```csharp
T Value { get; }
```

`Value` returns the stored value only when `IsSuccess` is `true`. Reading it from a failed result throws `InvalidOperationException`.

```csharp
var result = Result<int>.Failure(DomainError.NullValue);

if (result.IsFailure)
{
    Console.WriteLine(result.Error.Message);
}
else
{
    int value = result.Value;
}
```

Always verify `IsSuccess` before reading `Value`.

---

## `DomainError`

`DomainError` is an immutable record describing a domain or operation error.

```csharp
public sealed record DomainError(
    string Code,
    string Message,
    ErrorKind Kind);
```

| Member | Description |
|---|---|
| `Code` | Unique, machine-readable reason code |
| `Message` | Human-readable error message |
| `Kind` | General error classification |

### No error

`DomainError.None` represents the absence of an error:

```csharp
public static readonly DomainError None =
    new(string.Empty, string.Empty, ErrorKind.Failure);
```

It is valid for successful results and invalid as the reason for a failed result.

### Null value

`DomainError.NullValue` represents a missing value:

```csharp
public static readonly DomainError NullValue =
    new(
        "General.NullValue",
        "A null value was provided.",
        ErrorKind.Validation);
```

Example:

```csharp
Result<User> CreateUser(User? user)
{
    if (user is null)
        return DomainError.NullValue;

    return user;
}
```

### Error factories

```csharp
DomainError.Failure(code, message);
DomainError.Validation(code, message);
DomainError.NotFound(code, message);
DomainError.Conflict(code, message);
DomainError.Unauthorized(code, message);
DomainError.Forbidden(code, message);
DomainError.Unavailable(code, message);
```

Example:

```csharp
var error = DomainError.Conflict(
    "Users.EmailExists",
    "The email address is already in use.");

if (error.Is(ErrorKind.Conflict))
{
    // Handle conflict
}
```

### Error helpers

```csharp
bool matches = error.Is(ErrorKind.Validation);
bool isEmpty = error.IsNone();
```

- `Is(...)` compares the error with an `ErrorKind`.
- `IsNone()` checks whether the error is `DomainError.None`.

---

## `ErrorKind`

`ErrorKind` categorizes errors for consistent handling.

```csharp
public enum ErrorKind
{
    Failure = 0,
    Validation = 1,
    NotFound = 2,
    Conflict = 3,
    Unauthorized = 4,
    Forbidden = 5,
    Unavailable = 6
}
```

| Value | Purpose |
|---|---|
| `Failure` | General or uncategorized failure |
| `Validation` | Invalid input or violated validation rule |
| `NotFound` | Requested record or resource does not exist |
| `Conflict` | Operation conflicts with existing state or data |
| `Unauthorized` | Identity is missing or invalid |
| `Forbidden` | Identity is valid but lacks required permission |
| `Unavailable` | External service or resource is temporarily unavailable |

Example:

```csharp
void Handle(Result result)
{
    if (result.IsSuccess)
        return;

    switch (result.Error.Kind)
    {
        case ErrorKind.Validation:
            // Show input errors
            break;

        case ErrorKind.NotFound:
            // Show not found response
            break;

        case ErrorKind.Conflict:
            // Handle conflicting state
            break;

        case ErrorKind.Unauthorized:
            // Redirect to authentication
            break;

        case ErrorKind.Forbidden:
            // Show insufficient permissions
            break;

        case ErrorKind.Unavailable:
            // Suggest retry or fallback
            break;

        default:
            // Handle general failure
            break;
    }
}
```

---

## Complete Examples

### Operation with a return value

```csharp
public sealed record User(Guid Id, string Name);

public static class UserService
{
    public static Result<User> GetById(Guid id)
    {
        if (id == Guid.Empty)
        {
            return DomainError.Validation(
                "Users.InvalidId",
                "The user ID is invalid.");
        }

        if (!TryFindUser(id, out var user))
        {
            return DomainError.NotFound(
                "Users.NotFound",
                "The requested user was not found.");
        }

        return user;
    }

    private static bool TryFindUser(Guid id, out User? user)
    {
        user = new User(id, "Hassan");
        return true;
    }
}
```

Usage:

```csharp
Result<User> result = UserService.GetById(Guid.NewGuid());

if (result.IsSuccess)
{
    Console.WriteLine(result.Value.Name);
}
else
{
    Console.WriteLine($"{result.Error.Code}: {result.Error.Message}");
}
```

### Operation without a return value

```csharp
public static Result DeleteUser(Guid id)
{
    if (id == Guid.Empty)
    {
        return DomainError.Validation(
            "Users.InvalidId",
            "The user ID is invalid.");
    }

    return Result.Success();
}
```

Usage:

```csharp
Result result = DeleteUser(Guid.NewGuid());

if (result.IsFailure)
{
    Console.WriteLine(result.Error.Message);
}
```

---

## When to Use Each Type

| Situation | Recommended type |
|---|---|
| Success/failure operation without a value | `Result` |
| Operation that returns a value on success | `Result<T>` |
| Describing why an operation failed | `DomainError` |
| Categorizing validation, resource, or permission errors | `ErrorKind` |

## Usage Guidelines

1. Use stable and hierarchical error codes such as `Users.NotFound`.
2. Keep messages clear and suitable for the target audience.
3. Use `Validation` for invalid input or violated rules.
4. Use `NotFound` when the requested resource does not exist.
5. Use `Conflict` when the operation conflicts with existing state or data.
6. Do not read `Value` unless `IsSuccess` is `true`.
7. Never create a failed result with `DomainError.None`.

## Build

```powershell
dotnet build
```

## Testing

The repository includes an xUnit test project. Run it from the repository root:

```powershell
dotnet test "Result Pattern Tests\Result Pattern Tests.csproj"
```

The latest test run passed all 17 tests, with 0 failures and 0 skipped tests. Coverage includes:

- Successful and failed non-generic and generic results
- Invalid non-generic result states
- Accessing values in successful and failed generic results
- Implicit conversions between results, values, and `DomainError`
- `DomainError.None` and `DomainError.NullValue`

## Author

Hassan Jamal







# Gilgamesh.Results

> مكتبة خفيفة وقابلة لإعادة الاستخدام لتطبيق نمط `Result` في تطبيقات .NET، وإدارة عمليات النجاح والفشل من دون الاعتماد على الاستثناءات للحالات المتوقعة.

---

## العربية

### نظرة عامة

تعمل **Gilgamesh.Results** على فصل حالة تنفيذ العملية عن تفاصيل نتيجتها:

- نجاح عملية لا تُرجع قيمة.
- نجاح عملية تُرجع قيمة من نوع معيّن.
- فشل العملية مع خطأ منطقي أو تشغيلي موحّد.
- تصنيف الخطأ لتحديد طريقة المعالجة وعرضه للمستخدم أو تحويله إلى حالة HTTP.

المكتبة مصممة لتكون خفيفة، ولا تحتوي حاليًا على تبعيات خارجية. وهي تستهدف:

```xml
<TargetFramework>net10.0</TargetFramework>
```

### حالة الإصدار

**المكتبة تنتقل حاليًا من Alpha إلى Beta.** يحتوي مشروع الاختبارات حاليًا على 17 اختبارًا تغطي نتائج النجاح والفشل، والوصول إلى القيمة، والحالات غير الصالحة، والتحويلات الضمنية، وبعض سلوكيات `DomainError`. قد تتغير الواجهة قبل الإصدار المستقر.

### التثبيت

يمكن تضمين المشروع مباشرةً من خلال مرجع مشروع:

```xml
<ProjectReference Include="path\to\Gilgamesh.Results.csproj" />
```

أو نسخ المكتبة إلى مشروع آخر والحصول على حزم NuGet مستقبلية عند توفرها.

### المساحات الاسمية

```csharp
using Gilgamesh.Results.Results;
using Gilgamesh.Results.Errors;
```

تتضمن المكتبة الأنواع الأساسية التالية:

| النوع | المساحة الاسمية | الوصف |
|---|---|---|
| `Result` | `Gilgamesh.Results.Results` | نتيجة عملية لا تُرجع قيمة |
| `Result<T>` | `Gilgamesh.Results.Results` | نتيجة عملية تُرجع قيمة من نوع `T` |
| `ResultBase` | `Gilgamesh.Results.Results` | الأساس المجرد المشترك بين أنواع النتائج |
| `DomainError` | `Gilgamesh.Results.Errors` | وصف موحّد للخطأ |
| `ErrorKind` | `Gilgamesh.Results.Errors` | تصنيف نوع الخطأ |

---

## `ResultBase`

الفئة `ResultBase` هي الفئة الأساسية لكل النتائج وتتعرف على:

- نجاح العملية من خلال `IsSuccess`.
- فشل العملية من خلال `IsFailure`.
- تفاصيل الخطأ من خلال `Error`.

```csharp
public abstract class ResultBase
{
    public bool IsSuccess { get; }
    public bool IsFailure { get; }
    public DomainError Error { get; }
}
```

تفرض الفئة القواعد التالية:

- لا يجوز أن تكون النتيجة ناجحة وتحمل خطأً مختلفًا عن `DomainError.None`.
- لا يجوز أن تكون النتيجة فاشلة من دون تحديد خطأ.
- عند خرق أي من القاعدتين، يرمي النوع استثناءً من نوع `InvalidOperationException`.

---

## `Result`

تمثل `Result` نتيجة عملية لا تُرجع قيمة؛ مثل الحذف أو التحديث أو تنفيذ أمر.

### إنشاء نتيجة ناجحة

```csharp
Result result = Result.Success();

if (result.IsSuccess)
{
    Console.WriteLine("تمت العملية بنجاح.");
}
```

### إنشاء نتيجة فاشلة

```csharp
var error = DomainError.NotFound(
    "Users.NotFound",
    "المستخدم المطلوب غير موجود.");

Result result = Result.Failure(error);

if (result.IsFailure)
{
    Console.WriteLine(result.Error.Message);
}
```

يمكن أيضًا تمرير `DomainError` ضمنيًا إلى `Result`:

```csharp
Result DeleteUser(Guid id)
{
    if (id == Guid.Empty)
    {
        return DomainError.Validation(
            "Users.InvalidId",
            "معرّف المستخدم غير صالح.");
    }

    return Result.Success();
}
```

### الخواص والطرق

| العضو | النوع | الوصف |
|---|---|---|
| `Success()` | `Result` | ينشئ نتيجة ناجحة |
| `Failure(error)` | `Result` | ينشئ نتيجة فاشلة تحمل خطأً |
| التحويل ضمني من `DomainError` | `Result` | يحوّل الخطأ إلى نتيجة فاشلة |
| `IsSuccess` | `bool` | يحدد هل العملية ناجحة |
| `IsFailure` | `bool` | يحدد هل العملية فاشلة |
| `Error` | `DomainError` | يحمل تفاصيل الخطأ |

---

## `Result<T>`

تمثل `Result<T>` نتيجة عملية تُرجع قيمة من النوع `T` عند النجاح أو خطأً عند الفشل.

### إنشاء نتيجة ناجحة بقيمة

```csharp
var result = Result<int>.Success(42);

if (result.IsSuccess)
{
    Console.WriteLine(result.Value);
}
```

يمكن أيضًا تحويل القيم ضمنيًا:

```csharp
Result<string> result = "تم إنشاء المستخدم";
Console.WriteLine(result.Value);
```

### إنشاء نتيجة فاشلة

```csharp
var result = Result<User>.Failure(
    DomainError.NotFound(
        "Users.NotFound",
        "لا يوجد مستخدم بهذا المعرّف."));
```

أو باستخدام التحويل ضمني:

```csharp
Result<User> result =
    DomainError.NotFound(
        "Users.NotFound",
        "لا يوجد مستخدم بهذا المعرّف.");
```

### الوصول إلى القيمة

توفر الخاصية `Value` القيمة فقط عند نجاح النتيجة:

```csharp
T Value { get; }
```

إذا حاولت قراءة `Value` من نتيجة فاشلة، سيرمي الرمز استثناءً من نوع `InvalidOperationException`.

```csharp
var result = Result<int>.Failure(DomainError.NullValue);

if (result.IsFailure)
{
    Console.WriteLine(result.Error.Message);
}
else
{
    int value = result.Value;
}
```

يُنصح دائمًا بالتحقق من `IsSuccess` قبل قراءة `Value`.

---

## `DomainError`

يمثل `DomainError` معلومات الخطأ الناتج عن تنفيذ عملية منطقية أو تشغيليّة.

```csharp
public sealed record DomainError(
    string Code,
    string Message,
    ErrorKind Kind);
```

### الأجزاء

| العضو | الوصف |
|---|---|
| `Code` | رمز فريد يصف سبب الخطأ، مثل `Users.NotFound` |
| `Message` | رسالة بشرية توضح تفاصيل الخطأ |
| `Kind` | التصنيف العام للخطأ |

لا يزال سجل الأخطاء قابلًا للمقارنة كنوع `record`، ولذلك يمكن استخدامه عند المقارنة بحسب جميع عناصره.

### الخطأ الفارغ

يمثل `DomainError.None` عدم وجود خطأ ويُستخدم فقط مع النتائج الناجحة:

```csharp
public static readonly DomainError None =
    new(string.Empty, string.Empty, ErrorKind.Failure);
```

لا يُعدّ `None` سببًا صالحًا لإنشاء نتيجة فاشلة.

### خطأ القيمة الصفرية

يوفر النوع خطأًا جاهزًا للقيم المرسلة من دون قيمة:

```csharp
public static readonly DomainError NullValue =
    new(
        "General.NullValue",
        "A null value was provided.",
        ErrorKind.Validation);
```

مثال:

```csharp
Result<User> CreateUser(User? user)
{
    if (user is null)
        return DomainError.NullValue;

    // تنفيذ العملية
    return user;
}
```

### إنشاء أنواع محددة من الأخطاء

توفر المكتبة طرقًا ثابتة لإنشاء الأخطاء:

```csharp
DomainError.Failure(code, message);
DomainError.Validation(code, message);
DomainError.NotFound(code, message);
DomainError.Conflict(code, message);
DomainError.Unauthorized(code, message);
DomainError.Forbidden(code, message);
DomainError.Unavailable(code, message);
```

مثال كامل:

```csharp
var error = DomainError.Conflict(
    "Users.EmailExists",
    "البريد الإلكتروني مستخدم بالفعل.");

if (error.Is(ErrorKind.Conflict))
{
    // معالجة التعارض
}
```

### التحقق من نوع الخطأ

```csharp
bool matches = error.Is(ErrorKind.Validation);
bool isEmpty = error.IsNone();
```

- `Is(...)` يقارن الخطأ مع تصنيف `ErrorKind`.
- `IsNone()` يتحقق هل الخطأ هو `DomainError.None`.

---

## `ErrorKind`

يمثل `ErrorKind` تصنيفًا عامًا يساعد على معالجة الأخطاء بطرق مختلفة.

```csharp
public enum ErrorKind
{
    Failure = 0,
    Validation = 1,
    NotFound = 2,
    Conflict = 3,
    Unauthorized = 4,
    Forbidden = 5,
    Unavailable = 6
}
```

### التصنيفات

| القيمة | الاستخدام المقترح |
|---|---|
| `Failure` | فشل عام أو غير مصنّف |
| `Validation` | بيانات أو شروط إدخال غير صالحة |
| `NotFound` | عدم وجود سجل أو مورد مطلوب |
| `Conflict` | تعارض مع حالة أو بيانات موجودة |
| `Unauthorized` | هوية غير مصادَق عليها أو غير صالحة |
| `Forbidden` | هوية مصادَق عليها من دون صلاحية كافية |
| `Unavailable` | مورد أو خدمة خارجية غير متاحة مؤقتًا |

### مثال على معالجة التصنيف

```csharp
void Handle(Result result)
{
    if (result.IsSuccess)
        return;

    switch (result.Error.Kind)
    {
        case ErrorKind.Validation:
            // عرض أخطاء الإدخال
            break;

        case ErrorKind.NotFound:
            // إظهار أن المورد غير موجود
            break;

        case ErrorKind.Conflict:
            // معالجة التعارض، مثل تكرار البيانات
            break;

        case ErrorKind.Unauthorized:
            // طلب المصادقة أو تحديث جلسة الدخول
            break;

        case ErrorKind.Forbidden:
            // رفض العملية لعدم وجود صلاحية
            break;

        case ErrorKind.Unavailable:
            // اقتراح إعادة المحاولة أو استخدام بديل
            break;

        default:
            // معالجة الفشل العام
            break;
    }
}
```

---

## أمثلة متكاملة

### عمليات ترجع قيمة

```csharp
public sealed record User(Guid Id, string Name);

public static class UserService
{
    public static Result<User> GetById(Guid id)
    {
        if (id == Guid.Empty)
        {
            return DomainError.Validation(
                "Users.InvalidId",
                "معرّف المستخدم غير صالح.");
        }

        if (!TryFindUser(id, out var user))
        {
            return DomainError.NotFound(
                "Users.NotFound",
                "المستخدم المطلوب غير موجود.");
        }

        return user;
    }

    private static bool TryFindUser(Guid id, out User? user)
    {
        user = new User(id, "Hassan");
        return true;
    }
}
```

الاستخدام:

```csharp
Result<User> result = UserService.GetById(Guid.NewGuid());

if (result.IsSuccess)
{
    Console.WriteLine(result.Value.Name);
}
else
{
    Console.WriteLine($"{result.Error.Code}: {result.Error.Message}");
}
```

### عمليات لا ترجع قيمة

```csharp
public static Result DeleteUser(Guid id)
{
    if (id == Guid.Empty)
    {
        return DomainError.Validation(
            "Users.InvalidId",
            "معرّف المستخدم غير صالح.");
    }

    // حذف المستخدم
    return Result.Success();
}
```

الاستخدام:

```csharp
Result result = DeleteUser(Guid.NewGuid());

if (result.IsFailure)
{
    Console.WriteLine(result.Error.Message);
}
```

---

## متى تستخدم كل نوع؟

| الحالة | النوع المقترح |
|---|---|
| عملية نجاح/فشل فقط | `Result` |
| عملية تُرجع كائنًا عند النجاح | `Result<T>` |
| وصف سبب الفشل | `DomainError` |
| تصنيف خطأ الإدخال أو المورد أو الصلاحية | `ErrorKind` |

---

## إرشادات الاستخدام

1. استخدم الرموز بشكل ثابت ومنظم، مثل `Users.NotFound`.
2. اجعل رسائل الخطأ واضحة ومناسبة للجمهور المستهدف.
3. استخدم `Validation` لأخطاء المدخلات قبل تنفيذ أي منطق عمل.
4. استخدم `NotFound` عندما يكون المورد المطلوب غير موجود.
5. استخدم `Conflict` عند تعارض العملية مع بيانات أو حالة موجودة.
6. لا تقرأ `Value` إلا بعد التحقق من `IsSuccess`.
7. لا تنشئ نتيجة فاشلة باستخدام `DomainError.None`.

---

## البناء والتطوير

يبنى المشروع باستخدام:

```powershell
dotnet build
```

يمتدح المشروع حاليًا بنجاح البناء ضمن بيئة هدف `net10.0`.

### الاختبارات

يحتوي المستودع على مشروع اختبارات باستخدام xUnit. شغّله من مجلد المستودع الرئيسي:

```powershell
dotnet test "Result Pattern Tests\Result Pattern Tests.csproj"
```

نجحت آخر عملية تشغيل لجميع الاختبارات الـ17، دون أي إخفاق أو اختبار متخطّى. وتشمل التغطية:

- نجاح وفشل النتائج العامة وغير العامة.
- الحالات غير الصالحة للنتائج غير العامة.
- قراءة القيم من النتائج العامة الناجحة والفاشلة.
- التحويلات الضمنية بين النتائج والقيم و`DomainError`.
- `DomainError.None` و`DomainError.NullValue`.

---
