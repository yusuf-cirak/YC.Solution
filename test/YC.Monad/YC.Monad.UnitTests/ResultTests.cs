using Xunit;

namespace YC.Monad.UnitTests
{
    public class ResultTests
    {
        [Fact]
        public void Result_Success_ReturnsSuccessInstance()
        {
            // Act
            var result = Result.Success();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.False(result.IsFailure);
        }

        [Fact]
        public void Result_Failure_ReturnsFailureInstanceWithError()
        {
            // Arrange
            var error = Error.Create("Error.Test", "Error occurred");

            // Act
            var result = Result.Failure(error);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailure);
            Assert.Equal(error, result.Error);
        }

        [Fact]
        public void Result_Match_ReturnsSuccessOrFailureBasedOnResult()
        {
            // Arrange
            var successResult = Result.Success();
            var error = Error.Create("Error.Test", "Error occurred");

            var failureResult = Result.Failure(error);

            // Act
            var successMessage = successResult.Match(
                () => "Success",
                error => $"Failure: {error.Message}");
            var failureMessage = failureResult.Match(
                () => "Success",
                error => $"Failure: {error.Message}");

            // Assert
            Assert.Equal("Success", successMessage);
            Assert.Equal("Failure: Error occurred", failureMessage);
        }

        [Fact]
        public void Result_ImplicitConversionFromError_ReturnsFailure()
        {
            // Arrange
            Error error = Error.Create("Error.Test", "Implicit error");

            // Act
            Result result = error;

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(error, result.Error);
        }
    }

    public class ResultGenericTests
    {
        [Fact]
        public void Result_SuccessWithValue_ReturnsSuccessInstance()
        {
            // Arrange
            var value = 100;

            // Act
            var result = Result<int>.Success(value);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(value, result.Value);
        }

        [Fact]
        public void Result_FailureWithError_ReturnsFailureInstance()
        {
            // Arrange
            Error error = Error.Create("Error.Test", "Implicit error");

            // Act
            var result = Result<int>.Failure(error);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailure);
            Assert.Equal(error, result.Error);
        }

        [Fact]
        public void Result_Match_ReturnsSuccessOrFailureBasedOnGenericResult()
        {
            // Arrange
            var successResult = Result<int>.Success(200);
            Error error = Error.Create("Error.Test", "Error occurred");

            var failureResult = Result<int>.Failure(error);

            // Act
            var successMessage = successResult.Match(
                value => $"Success: {value}",
                error => $"Failure: {error.Message}");
            var failureMessage = failureResult.Match(
                value => $"Success: {value}",
                error => $"Failure: {error.Message}");

            // Assert
            Assert.Equal("Success: 200", successMessage);
            Assert.Equal("Failure: Error occurred", failureMessage);
        }

        [Fact]
        public void Result_ImplicitConversionFromValue_ReturnsSuccess()
        {
            // Arrange
            int value = 300;

            // Act
            Result<int> result = value;

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(value, result.Value);
        }

        [Fact]
        public void Result_ImplicitConversionFromError_ReturnsFailure()
        {
            // Arrange
            var error = Error.Create("Error.Test", "Implicit error");
            // Act
            Result<int> result = error;

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(error, result.Error);
        }
    }

    public class ResultCacheTests
    {
        [Fact]
        public void ResultCache_ReturnsPredefinedFailureInstances()
        {
            // Act & Assert
            var unauthorizedResult = ResultCache.Unauthorized;
            var badRequestResult = ResultCache.BadRequest;
            var notFoundResult = ResultCache.NotFound;
            var forbiddenResult = ResultCache.Forbidden;

            Assert.False(unauthorizedResult.IsSuccess);
            Assert.Equal(ErrorCache.Unauthorized, ((Result)unauthorizedResult).Error);

            Assert.False(badRequestResult.IsSuccess);
            Assert.Equal(ErrorCache.BadRequest, ((Result)badRequestResult).Error);

            Assert.False(notFoundResult.IsSuccess);
            Assert.Equal(ErrorCache.NotFound, ((Result)notFoundResult).Error);

            Assert.False(forbiddenResult.IsSuccess);
            Assert.Equal(ErrorCache.Forbidden, ((Result)forbiddenResult).Error);
        }
    }

    public class ResultExtensionsTests
    {
        [Fact]
        public void Create_WithError_ReturnsFailure()
        {
            // Arrange
            var error = Error.Create("Error.Test", "Implicit error");

            // Act
            var result = ResultExtensions.Create(error);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(error, ((Result)result).Error);
        }

        [Fact]
        public void Create_WithValue_ReturnsSuccess()
        {
            // Arrange
            var value = "Success value";

            // Act
            var result = ResultExtensions.Create<string>(value);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(value, ((Result<string>)result).Value);
        }


            [Fact]
    public void Success_WithValue_CreatesSuccessResult()
    {
        // Arrange
        var value = "test";

        // Act
        var result = Result<string>.Success(value);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(value, result.Value);
        Assert.Equal(ErrorCache.None, result.Error);
    }

    [Fact]
    public void Failure_WithError_CreatesFailureResult()
    {
        // Arrange
        var error = Error.Create("TEST", "Test error");

        // Act
        var result = Result<string>.Failure(error);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(error, result.Error);
    }

    [Fact]
    public void ImplicitConversion_FromValue_CreatesSuccessResult()
    {
        // Arrange
        string value = "test";

        // Act
        Result<string> result = value;

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(value, result.Value);
    }

    [Fact]
    public void ImplicitConversion_FromError_CreatesFailureResult()
    {
        // Arrange
        var error = Error.Create("TEST", "Test error");

        // Act
        Result<string> result = error;

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(error, result.Error);
    }

    [Fact]
    public void Match_OnSuccess_CallsSuccessFunc()
    {
        // Arrange
        var value = "test";
        var result = Result<string>.Success(value);
        string? capturedValue = null;
        Error? capturedError = null;

        // Act
        result.Match(
            v => capturedValue = v,
            e => capturedError = e);

        // Assert
        Assert.Equal(value, capturedValue);
        Assert.Null(capturedError);
    }

    [Fact]
    public void Match_OnFailure_CallsFailureFunc()
    {
        // Arrange
        var error = Error.Create("TEST", "Test error");
        var result = Result<string>.Failure(error);
        string? capturedValue = null;
        Error? capturedError = null;

        // Act
        result.Match(
            v => capturedValue = v,
            e => capturedError = e);

        // Assert
        Assert.Null(capturedValue);
        Assert.Equal(error, capturedError);
    }
    }
}
