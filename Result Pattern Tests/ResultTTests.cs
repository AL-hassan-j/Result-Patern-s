namespace Result_Pattern_Tests;

using Gilgamesh.Results.Errors;
using Gilgamesh.Results.Results;


public class ResultTTests
{
    /// <summary>
    /// Verifies that creating a successful result with a valid value returns a success state, the expected value, and no error.
    /// </summary>
    [Fact]
    public void Success_WithValidValue_ShouldReturnSuccessWithExpectedValueAndNoError()
    {
        int Num = 10;

        var result = Result<int>.Success(Num);

        Assert.True(result.IsSuccess);

        Assert.False(result.IsFailure);

        Assert.Equal(result.Value, Num);

        Assert.True(result.Error.IsNone());
    }
    /// <summary>
    /// Verifies that creating a failed result with an error returns a failure state, the matching error, and throws when accessing the value.
    /// </summary>
    [Fact]
    public void Failure_WithError_ShoukdReturnFailureWithExpectedErrorAndThrowOnValueAccess()
    {

        var error = DomainError.NotFound("user-not-found", "user Not Found");

        var result = Result<int>.Failure(error);

        Assert.True(result.IsFailure);

        Assert.False(result.IsSuccess);

        Assert.Equal(error, result.Error);

        Assert.Throws<InvalidOperationException>(() => result.Value);

        Assert.False(result.Error.IsNone());


    }
    /// <summary>
    /// Verifies that implicitly converting a valid value to <see cref="Result{T}"/> creates a successful result with the expected value and no error.
    /// </summary>
    [Fact]
    public void ImplicitOperator_WithValidValue_ShouldReturnSuccessWithExpectedValueAndNoError()
    {

        string message = "More C# & Clean Architecture tips on Instagram: @al_hassan.j";

        Result<string> result = message;

        Assert.True(result.IsSuccess);

        Assert.False(result.IsFailure);

        Assert.Equal(message, result.Value);

        Assert.True(result.Error.IsNone());

    }
    /// <summary>
    /// Tests division using <see cref="Result{T}"/> to verify success for valid division and failure when dividing by zero.
    /// </summary>
    [Theory]
    [InlineData(10, 2, 5, true)]
    [InlineData(10, 0, 0, false)]
    public void Divid_WithInputs_ShouldReturnExpectedResultStateAndValue(int a, int b, int expectedValue, bool expectedSuccess)
    {
        var result = Divid(a, b);

        Assert.Equal(expectedSuccess, result.IsSuccess);

        if (expectedSuccess)
        {
            Assert.Equal(expectedValue, result.Value);

        }
        else
        {
            Assert.True(result.IsFailure);
            Assert.Throws<InvalidOperationException>(() => result.Value);
        }


    }

    /// <summary>
    /// Divides two integers and returns a successful result or a domain error when dividing by zero.
    /// </summary>
    private static Result<int> Divid(int a, int b)
    {
        if (b == 0)
        {
            return new DomainError("Math.DivideByZero", "Division by zero is not possible.", ErrorKind.Failure);
        }

        return a / b;
    }

    /// <summary>
    /// Verifies that implicitly converting a <see cref="DomainError"/> to <see cref="Result{T}"/> creates a failed result containing the specified error.
    /// </summary>
    [Fact]
    public void ImplicitOperator_WithDomainError_ShouldReturnFailureWithExpectedError()
    {
        var error = DomainError.Validation("invalid-email", "Invalid email");

        Result<string> result = error;

        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(error, result.Error);
        Assert.Throws<InvalidOperationException>(() => result.Value);
    }

    /// <summary>
    /// Verifies that a successful result can hold a null value for nullable reference types.
    /// </summary>
    [Fact]
    public void Success_WithNullValueForNullableType_ShouldReturnSuccessWithNullValue()
    {

        var result = Result<string?>.Success(null);
    
        Assert.True(result.IsSuccess);

        Assert.Null(result.Value);
    
    }


}

