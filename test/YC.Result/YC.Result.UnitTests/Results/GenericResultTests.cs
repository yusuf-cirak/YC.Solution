namespace YC.Result.UnitTests.Results;

public class GenericResultTests
{
    [Fact]
    public void Success_ShouldSetIsSuccessToTrueAndSetTheValue()
    {
        // Arrange
        var expectedValue = "Test Value";

        // Act
        var result = Result<string>.Success(expectedValue);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(expectedValue, result.Value);
        Assert.Equal(ErrorsCache.None, result.Error);
    }

    [Fact]
    public void Failure_ShouldSetIsSuccessToFalseAndSetTheError()
    {
        // Arrange
        var error = Error.Create("Test Error");

        // Act
        var result = Result<string>.Failure(error);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(default, result.Value);
        Assert.Equal(error, result.Error);
    }

    [Fact]
    public void ImplicitConversion_FromValue_ShouldSetIsSuccessToTrueAndSetTheValue()
    {
        // Arrange
        var expectedValue = "Test Value";

        // Act
        Result<string> result = expectedValue;

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(expectedValue, result.Value);
        Assert.Equal(ErrorsCache.None, result.Error);
    }

    [Fact]
    public void ImplicitConversion_FromError_ShouldSetIsSuccessToFalseAndSetTheError()
    {
        // Arrange
        var error = Error.Create("Test Error");

        // Act
        Result<string> result = error;

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(default, result.Value);
        Assert.Equal(error, result.Error);
    }

    [Fact]
    public void Match_ShouldCallCorrectDelegateBasedOnIsSuccess()
    {
        // Arrange
        var successResult = Result<string>.Success("Success");
        var failureResult = Result<string>.Failure(Error.Create("Failure"));

        // Act & Assert
        Assert.Equal("Success", successResult.Match(value => value, error => error.Detail));
        Assert.Equal("Failure", failureResult.Match(value => value, error => error.Detail));
    }

    [Fact]
    public void Match_Success_ShouldInvokeSuccessAction()
    {
        // Arrange
        var result = Result<string>.Success("Test Value");
        bool successCalled = false;
        bool failureCalled = false;

        // Act
        result.Match(
            success: _ => successCalled = true,
            failure: _ => failureCalled = true);

        // Assert
        Assert.True(successCalled);
        Assert.False(failureCalled);
    }

    [Fact]
    public void Match_Failure_ShouldInvokeFailureAction()
    {
        // Arrange
        var error = Error.Create("Failure");
        var result = Result<string>.Failure(error);
        bool successCalled = false;
        bool failureCalled = false;

        // Act
        result.Match(
            success: _ => successCalled = true,
            failure: _ => failureCalled = true);

        // Assert
        Assert.False(successCalled);
        Assert.True(failureCalled);
    }

    [Fact]
    public void Match_OnlyFailureAction_ShouldInvokeFailureAction()
    {
        // Arrange
        var error = Error.Create("Failure");
        var result = Result<string>.Failure(error);
        bool failureCalled = false;

        // Act
        result.Match(failure: _ => failureCalled = true);

        // Assert
        Assert.True(failureCalled);
    }

    [Fact]
    public void Match_OnlySuccessAction_ShouldInvokeSuccessAction()
    {
        // Arrange
        var result = Result<string>.Success("Test Value");
        bool successCalled = false;

        // Act
        result.Match(success: _ => successCalled = true);

        // Assert
        Assert.True(successCalled);
    }
}