

using Gilgamesh.Results.Results;
using Gilgamesh.Results.Errors;

namespace Result_Pattern_Tests;
public class EmptyResultTests
{
    /// <summary>
    /// Verifies that creating a non-generic successful result sets IsSuccess to true, IsFailure to false, and returns DomainError.None.
    /// </summary>
    [Fact]
    public void Success_WhenCalled_ReturnsSuccessWithNoError()
    {
        // Arrange & Act
        var result = Result.Success();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(DomainError.None, result.Error);
        Assert.True(result.Error.IsNone());
    }

    /// <summary>
    /// Verifies that creating a non-generic failed result sets IsFailure to true, IsSuccess to false, and preserves the DomainError instance.
    /// </summary>
    [Fact]
    public void Failure_WhenDomainErrorProvided_ReturnsFailureWithExpectedError()
    {
        // Arrange
        var expectedError = DomainError.NotFound("user-not-found", "sory this user not found");

        // Act
        var result = Result.Failure(expectedError);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(expectedError, result.Error);
    }

    /// <summary>
    /// Verifies that implicit conversion from DomainError to Result produces a failed result preserving the error instance.
    /// </summary>
    [Fact]
    public void ImplicitConversion_FromDomainError_ReturnsFailureWithExpectedError()
    {
        // Arrange
        var expectedError = DomainError.NotFound("user-not-found", "sory this user not found");

        // Act
        Result result = expectedError;

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(expectedError, result.Error);
    }

    /// <summary>
    /// Verifies that updating data with a non-matching email returns a failed result with the expected error via implicit DomainError conversion.
    /// </summary>
    [Theory]
    [InlineData("email@gmail.com")]
    public void UpdateData_WhenEmailDoesNotMatch_ReturnsFailureWithExpectedError(string email)
    {
        // Arrange & Act
        var result = UpdateData(email);

        // Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(DomainError.NotFound("user-not-found", "sory this user not found"), result.Error);
    }

    /// <summary>
    /// Verifies that updating data with a matching email returns a successful result with DomainError.None.
    /// </summary>
    [Theory]
    [InlineData("abdul.hassan.jamal.abdul.hassan@gmail.com")]
    public void UpdateData_WhenEmailMatches_ReturnsSuccessWithNoError(string email)
    {
        // Arrange & Act
        var result = UpdateData(email);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(DomainError.None, result.Error);
        Assert.True(result.Error.IsNone());
    }

    private  Result UpdateData(string email)
    {
        if (email != "abdul.hassan.jamal.abdul.hassan@gmail.com")
        {
            return DomainError.NotFound("user-not-found", "sory this user not found");
        }

        return Result.Success();
    }

    /// <summary>
    /// Verifies that creating a successful result with a non-None error throws InvalidOperationException.
    /// </summary>
    [Fact]
    public void Constructor_WhenSuccessWithError_ThrowsInvalidOperationException()
    {
        // Arrange
        var error = DomainError.NotFound("user-not-found", "sory this user not found");

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => new Result(true, error));
    }

    /// <summary>
    /// Verifies that creating a failed result with DomainError.None throws InvalidOperationException.
    /// </summary>
    [Fact]
    public void Constructor_WhenFailureWithNoneError_ThrowsInvalidOperationException()
    {
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => new Result(false, DomainError.None));
    }

}
