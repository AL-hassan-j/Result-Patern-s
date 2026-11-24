using Gilgamesh.Results.Errors;

namespace Result_Pattern_Tests;
 

public class DomainErrorTests
{

    /// <summary>
    /// Verifies that DomainError.None is recognized as a no-error state via IsNone().
    /// </summary>
    [Fact]
    public void None_WhenAccessed_IsNoneReturnsTrue()
    {
        // Arrange
        var noneError = DomainError.None;

        // Act
        var isNone = noneError.IsNone();

        // Assert
        Assert.True(isNone);
    }

    /// <summary>
    /// Verifies that DomainError.None matches ErrorKind.Failure when checked via Is().
    /// </summary>
    [Fact]
    public void None_WhenCheckedWithIsFailure_ReturnsTrue()
    {
        // Arrange
        var noneError = DomainError.None;

        // Act
        var isFailure = noneError.Is(ErrorKind.Failure);

        // Assert
        Assert.True(isFailure);
    }

    /// <summary>
    /// Verifies that DomainError.NullValue has the expected code, message, and ErrorKind.Validation kind.
    /// </summary>
    [Fact]
    public void NullValue_WhenAccessed_HasExpectedCodeMessageAndValidationKind()
    {
        // Arrange & Act
        var error = DomainError.NullValue;

        // Assert
        Assert.Equal("General.NullValue", error.Code);
        Assert.Equal("A null value was provided.", error.Message);
        Assert.True(error.Is(ErrorKind.Validation));
        Assert.False(error.IsNone());
    }

}
