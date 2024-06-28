namespace YC.Result.UnitTests.Results;

public class ResultTests
{
    [Fact]
    public void Success_ShouldSetIsSuccessToTrue()
    {
        // Arrange
        var result = Result.Success();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
    }

    [Fact]
    public void Fail_ShouldSetIsSuccessToFalse()
    {
        // Arrange
        var result = Result.Failure();

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(ErrorsCache.None, result.Error);
    }

    [Fact]
    public void Failure_WithError_ShouldSetErrorAndIsSuccessToFalse()
    {
        // Arrange
        var error = Error.Create("Test Error");
        var result = Result.Failure(error);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(error, result.Error);
    }

    [Fact]
    public void ImplicitConversion_FromError_ShouldSetErrorAndIsSuccessToFalse()
    {
        // Arrange
        var error = Error.Create("Test Error");

        // Act
        Result result = error;

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(error, result.Error);
    }

    [Fact]
    public void ImplicitConversion_FromBool_ShouldSetIsSuccess()
    {
        // Arrange
        bool isSuccess = true;

        // Act
        Result result = isSuccess;

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(ErrorsCache.None, result.Error);

        // Arrange
        isSuccess = false;

        // Act
        result = isSuccess;

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(ErrorsCache.None, result.Error);
    }

    [Fact]
    public void Match_ShouldCallCorrectDelegateBasedOnIsSuccess()
    {
        // Arrange
        var successResult = Result.Success();
        var failureResult = Result.Failure(Error.Create("Failure"));

        // Act & Assert
        Assert.Equal("Success", successResult.Match(() => "Success", error => "Failure"));
        Assert.Equal("Failure", failureResult.Match(() => "Success", error => "Failure"));
    }
}