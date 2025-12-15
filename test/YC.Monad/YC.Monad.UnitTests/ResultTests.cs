﻿using Xunit;

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

    public class ResultLinqTests
    {
        [Fact]
        public void Select_OnSuccess_TransformsValue()
        {
            // Arrange
            var result = Result<int>.Success(5);

            // Act
            var mapped = result.Select(x => x * 2);

            // Assert
            Assert.True(mapped.IsSuccess);
            Assert.Equal(10, mapped.Value);
        }

        [Fact]
        public void Select_OnFailure_PropagatesError()
        {
            // Arrange
            var error = Error.Create("TEST", "Test error");
            var result = Result<int>.Failure(error);

            // Act
            var mapped = result.Select(x => x * 2);

            // Assert
            Assert.False(mapped.IsSuccess);
            Assert.Equal(error, mapped.Error);
        }

        [Fact]
        public void SelectMany_WithTwoSuccessResults_CombinesValues()
        {
            // Arrange
            var result1 = Result<int>.Success(5);
            var result2 = Result<int>.Success(10);

            // Act
            var combined = result1.SelectMany(
                x => result2,
                (x, y) => x + y);

            // Assert
            Assert.True(combined.IsSuccess);
            Assert.Equal(15, combined.Value);
        }

        [Fact]
        public void SelectMany_WithFirstFailure_ReturnsFirstError()
        {
            // Arrange
            var error = Error.Create("FIRST_ERROR", "First error");
            var result1 = Result<int>.Failure(error);
            var result2 = Result<int>.Success(10);

            // Act
            var combined = result1.SelectMany(
                x => result2,
                (x, y) => x + y);

            // Assert
            Assert.False(combined.IsSuccess);
            Assert.Equal(error, combined.Error);
        }

        [Fact]
        public void SelectMany_WithSecondFailure_ReturnsSecondError()
        {
            // Arrange
            var result1 = Result<int>.Success(5);
            var error = Error.Create("SECOND_ERROR", "Second error");
            var result2 = Result<int>.Failure(error);

            // Act
            var combined = result1.SelectMany(
                x => result2,
                (x, y) => x + y);

            // Assert
            Assert.False(combined.IsSuccess);
            Assert.Equal(error, combined.Error);
        }

        [Fact]
        public void LinqQuery_WithSuccessResults_CombinesValues()
        {
            // Arrange
            var result1 = Result<int>.Success(5);
            var result2 = Result<int>.Success(10);

            // Act
            var combined =
                from x in result1
                from y in result2
                select x + y;

            // Assert
            Assert.True(combined.IsSuccess);
            Assert.Equal(15, combined.Value);
        }

        [Fact]
        public void LinqQuery_WithFailure_PropagatesError()
        {
            // Arrange
            var error = Error.Create("TEST_ERROR", "Test error");
            var result1 = Result<int>.Success(5);
            var result2 = Result<int>.Failure(error);

            // Act
            var combined =
                from x in result1
                from y in result2
                select x + y;

            // Assert
            Assert.False(combined.IsSuccess);
            Assert.Equal(error, combined.Error);
        }

        [Fact]
        public void Where_WithSuccessAndTruePredicate_ReturnsSuccess()
        {
            // Arrange
            var result = Result<int>.Success(10);

            // Act
            var filtered = result.Where(x => x > 5);

            // Assert
            Assert.True(filtered.IsSuccess);
            Assert.Equal(10, filtered.Value);
        }

        [Fact]
        public void Where_WithSuccessAndFalsePredicate_ReturnsFailure()
        {
            // Arrange
            var result = Result<int>.Success(3);

            // Act
            var filtered = result.Where(x => x > 5);

            // Assert
            Assert.False(filtered.IsSuccess);
            Assert.Equal("PREDICATE_FAILED", filtered.Error.Code);
        }

        [Fact]
        public void Where_WithCustomError_ReturnsCustomError()
        {
            // Arrange
            var result = Result<int>.Success(3);
            var customError = Error.Create("CUSTOM", "Custom error");

            // Act
            var filtered = result.Where(x => x > 5, customError);

            // Assert
            Assert.False(filtered.IsSuccess);
            Assert.Equal(customError, filtered.Error);
        }

        [Fact]
        public void Where_WithFailure_PropagatesOriginalError()
        {
            // Arrange
            var error = Error.Create("ORIGINAL", "Original error");
            var result = Result<int>.Failure(error);

            // Act
            var filtered = result.Where(x => x > 5);

            // Assert
            Assert.False(filtered.IsSuccess);
            Assert.Equal(error, filtered.Error);
        }

        [Fact]
        public void ComplexLinqQuery_WithMultipleOperations_WorksCorrectly()
        {
            // Arrange
            var result1 = Result<int>.Success(5);
            var result2 = Result<int>.Success(10);

            // Act
            var combined =
                from x in result1
                from y in result2
                let sum = x + y
                where sum > 10
                select sum * 2;

            // Assert
            Assert.True(combined.IsSuccess);
            Assert.Equal(30, combined.Value);
        }

        [Fact]
        public void ComplexLinqQuery_WithFailingWhere_ReturnsFailure()
        {
            // Arrange
            var result1 = Result<int>.Success(2);
            var result2 = Result<int>.Success(3);

            // Act
            var combined =
                from x in result1
                from y in result2
                let sum = x + y
                where sum > 10
                select sum * 2;

            // Assert
            Assert.False(combined.IsSuccess);
            Assert.Equal("PREDICATE_FAILED", combined.Error.Code);
        }
    }
}