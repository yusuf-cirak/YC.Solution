using Xunit;

namespace YC.Monad.UnitTests
{
    public class ResultRailwayMapTests
    {
        [Fact]
        public void Map_OnSuccess_TransformsValue()
        {
            // Arrange
            var result = Result<int>.Success(5);

            // Act
            var mapped = result.Map(x => x * 2);
            // Assert
            Assert.True(mapped.IsSuccess);
            Assert.Equal(10, mapped.Value);
        }

        [Fact]
        public void Map_OnFailure_PropagatesError()
        {
            // Arrange
            var error = Error.Create("MAP_ERROR", "Error during map");
            var result = Result<int>.Failure(error);

            // Act
            var mapped = result.Map(x => x * 2);

            // Assert
            Assert.False(mapped.IsSuccess);
            Assert.Equal(error, mapped.Error);
        }

        [Fact]
        public void Map_WithTypeConversion_WorksCorrectly()
        {
            // Arrange
            var result = Result<int>.Success(42);

            // Act
            var mapped = result.Map(x => x.ToString());

            // Assert
            Assert.True(mapped.IsSuccess);
            Assert.Equal("42", mapped.Value);
        }

        [Fact]
        public void Map_WithComplexTransformation_WorksCorrectly()
        {
            // Arrange
            var result = Result<string>.Success("hello");

            // Act
            var mapped = result.Map(x => new { Length = x.Length, Upper = x.ToUpper() });

            // Assert
            Assert.True(mapped.IsSuccess);
            Assert.Equal(5, mapped.Value.Length);
            Assert.Equal("HELLO", mapped.Value.Upper);
        }

        [Fact]
        public void Map_ChainedCalls_WorksCorrectly()
        {
            // Arrange
            var result = Result<int>.Success(2);

            // Act
            var mapped = result
                .Map(x => x * 2)
                .Map(x => x + 3);

            // Assert
            Assert.True(mapped.IsSuccess);
            Assert.Equal(7, mapped.Value);
        }
    }

    public class ResultRailwayMapAsyncTests
    {
        [Fact]
        public async Task MapAsync_OnSuccess_TransformsValueAsynchronously()
        {
            // Arrange
            var result = Result<int>.Success(5);

            // Act
            var mapped = await result.MapAsync(async x => await Task.FromResult(x * 2));

            // Assert
            Assert.True(mapped.IsSuccess);
            Assert.Equal(10, mapped.Value);
        }

        [Fact]
        public async Task MapAsync_OnFailure_PropagatesError()
        {
            // Arrange
            var error = Error.Create("MAP_ASYNC_ERROR", "Error during async map");
            var result = Result<int>.Failure(error);

            // Act
            var mapped = await result.MapAsync(async x => await Task.FromResult(x * 2));

            // Assert
            Assert.False(mapped.IsSuccess);
            Assert.Equal(error, mapped.Error);
        }

        [Fact]
        public async Task MapAsync_WithTypeConversion_WorksCorrectly()
        {
            // Arrange
            var result = Result<int>.Success(42);

            // Act
            var mapped = await result.MapAsync(async x =>
            {
                await Task.Delay(5);
                return x.ToString();
            });

            // Assert
            Assert.True(mapped.IsSuccess);
            Assert.Equal("42", mapped.Value);
        }

        [Fact]
        public async Task MapAsync_WithExceptionInAsyncFunc_PropagatesError()
        {
            // Arrange
            var result = Result<int>.Success(5);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                _ = await result.MapAsync<int, int>(async x =>
                {
                    await Task.Delay(1);
                    throw new InvalidOperationException("Async operation failed");
                });
            });
        }

        [Fact]
        public async Task MapAsync_ChainedCalls_WorksCorrectly()
        {
            // Arrange
            var result = Result<int>.Success(2);

            // Act
            var mapped1 = await result.MapAsync(async x => await Task.FromResult(x * 2));
            var mapped2 = await mapped1.MapAsync(async x => await Task.FromResult(x + 3));

            // Assert
            Assert.True(mapped2.IsSuccess);
            Assert.Equal(7, mapped2.Value);
        }
    }

    public class ResultRailwayBindTests
    {
        [Fact]
        public void Bind_OnSuccess_ReturnsResultFromFunc()
        {
            // Arrange
            var result = Result<int>.Success(5);

            // Act
            var bound = result.Bind(x => Result<int>.Success(x * 2));

            // Assert
            Assert.True(bound.IsSuccess);
            Assert.Equal(10, bound.Value);
        }

        [Fact]
        public void Bind_OnSuccessWithFailure_PropagatesNewError()
        {
            // Arrange
            var newError = Error.Create("BIND_ERROR", "Error from bind function");
            var result = Result<int>.Success(5);

            // Act
            var bound = result.Bind(x => Result<int>.Failure(newError));

            // Assert
            Assert.False(bound.IsSuccess);
            Assert.Equal(newError, bound.Error);
        }

        [Fact]
        public void Bind_OnFailure_PropagatesOriginalError()
        {
            // Arrange
            var originalError = Error.Create("ORIGINAL_ERROR", "Original error");
            var result = Result<int>.Failure(originalError);
            var bindError = Error.Create("BIND_ERROR", "Should not be used");

            // Act
            var bound = result.Bind(x => Result<int>.Failure(bindError));

            // Assert
            Assert.False(bound.IsSuccess);
            Assert.Equal(originalError, bound.Error);
        }

        [Fact]
        public void Bind_WithTypeConversion_WorksCorrectly()
        {
            // Arrange
            var result = Result<int>.Success(42);

            // Act
            var bound = result.Bind(x => Result<string>.Success(x.ToString()));

            // Assert
            Assert.True(bound.IsSuccess);
            Assert.Equal("42", bound.Value);
        }

        [Fact]
        public void Bind_ChainedCalls_WorksCorrectly()
        {
            // Arrange
            var result = Result<int>.Success(2);

            // Act
            var bound = result
                .Bind(x => Result<int>.Success(x * 2))
                .Bind(x => Result<int>.Success(x + 3));

            // Assert
            Assert.True(bound.IsSuccess);
            Assert.Equal(7, bound.Value);
        }

        [Fact]
        public void Bind_StopsOnFirstFailure()
        {
            // Arrange
            var error1 = Error.Create("ERROR1", "First error");
            var error2 = Error.Create("ERROR2", "Second error");

            var result = Result<int>.Success(5)
                .Bind(x => Result<int>.Failure(error1));

            // Act
            var bound = result.Bind(x => Result<int>.Failure(error2));

            // Assert
            Assert.False(bound.IsSuccess);
            Assert.Equal(error1, bound.Error);
        }
    }

    public class ResultRailwayBindAsyncTests
    {
        [Fact]
        public async Task BindAsync_OnSuccess_ReturnsResultFromAsyncFunc()
        {
            // Arrange
            var result = Result<int>.Success(5);

            // Act
            var bound = await result.BindAsync(async x =>
                await Task.FromResult(Result<int>.Success(x * 2)));

            // Assert
            Assert.True(bound.IsSuccess);
            Assert.Equal(10, bound.Value);
        }

        [Fact]
        public async Task BindAsync_OnSuccessWithFailure_PropagatesNewError()
        {
            // Arrange
            var newError = Error.Create("BIND_ASYNC_ERROR", "Error from async bind function");
            var result = Result<int>.Success(5);

            // Act
            var bound = await result.BindAsync(async x =>
                await Task.FromResult(Result<int>.Failure(newError)));

            // Assert
            Assert.False(bound.IsSuccess);
            Assert.Equal(newError, bound.Error);
        }

        [Fact]
        public async Task BindAsync_OnFailure_PropagatesOriginalError()
        {
            // Arrange
            var originalError = Error.Create("ORIGINAL_ERROR", "Original error");
            var result = Result<int>.Failure(originalError);
            var bindError = Error.Create("BIND_ERROR", "Should not be used");

            // Act
            var bound = await result.BindAsync(async x =>
                await Task.FromResult(Result<int>.Failure(bindError)));

            // Assert
            Assert.False(bound.IsSuccess);
            Assert.Equal(originalError, bound.Error);
        }

        [Fact]
        public async Task BindAsync_WithTypeConversion_WorksCorrectly()
        {
            // Arrange
            var result = Result<int>.Success(42);

            // Act
            var bound = await result.BindAsync(async x =>
                await Task.FromResult(Result<string>.Success(x.ToString())));

            // Assert
            Assert.True(bound.IsSuccess);
            Assert.Equal("42", bound.Value);
        }

        [Fact]
        public async Task BindAsync_WithAsyncDelay_WorksCorrectly()
        {
            // Arrange
            var result = Result<int>.Success(5);

            // Act
            var bound = await result.BindAsync(async x =>
            {
                await Task.Delay(10);
                return Result<int>.Success(x * 2);
            });

            // Assert
            Assert.True(bound.IsSuccess);
            Assert.Equal(10, bound.Value);
        }

        [Fact]
        public async Task BindAsync_ChainedCalls_WorksCorrectly()
        {
            // Arrange
            var result = Result<int>.Success(2);

            // Act
            var bound1 = await result.BindAsync(async x => await Task.FromResult(Result<int>.Success(x * 2)));
            var bound2 = await bound1.BindAsync(async x => await Task.FromResult(Result<int>.Success(x + 3)));

            // Assert
            Assert.True(bound2.IsSuccess);
            Assert.Equal(7, bound2.Value);
        }
    }

    public class ResultRailwayTapTests
    {
        [Fact]
        public void Tap_OnSuccess_ExecutesSideEffect()
        {
            // Arrange
            var result = Result<int>.Success(5);
            var sideEffectExecuted = false;
            int capturedValue = 0;

            // Act
            result.Tap(x =>
            {
                sideEffectExecuted = true;
                capturedValue = x;
            });

            // Assert
            Assert.True(sideEffectExecuted);
            Assert.Equal(5, capturedValue);
        }

        [Fact]
        public void Tap_OnSuccess_ReturnsOriginalResult()
        {
            // Arrange
            var result = Result<int>.Success(5);

            // Act
            var tapped = result.Tap(x => { });

            // Assert
            Assert.True(tapped.IsSuccess);
            Assert.Equal(5, tapped.Value);
        }

        [Fact]
        public void Tap_OnFailure_DoesNotExecuteSideEffect()
        {
            // Arrange
            var error = Error.Create("TAP_ERROR", "Error for tap test");
            var result = Result<int>.Failure(error);
            var sideEffectExecuted = false;

            // Act
            result.Tap(x => sideEffectExecuted = true);

            // Assert
            Assert.False(sideEffectExecuted);
            Assert.False(result.IsSuccess);
            Assert.Equal(error, result.Error);
        }

        [Fact]
        public void Tap_OnFailure_ReturnsOriginalResult()
        {
            // Arrange
            var error = Error.Create("TAP_ERROR", "Error for tap test");
            var result = Result<int>.Failure(error);

            // Act
            var tapped = result.Tap(x => { });

            // Assert
            Assert.False(tapped.IsSuccess);
            Assert.Equal(error, tapped.Error);
        }

        [Fact]
        public void Tap_AllowsChaining_WithOtherMethods()
        {
            // Arrange
            var result = Result<int>.Success(5);
            var sideEffectValue = 0;

            // Act
            var final = result
                .Tap(x => sideEffectValue = x)
                .Map(x => x * 2);

            // Assert
            Assert.True(final.IsSuccess);
            Assert.Equal(10, final.Value);
            Assert.Equal(5, sideEffectValue);
        }

        [Fact]
        public void Tap_MultipleCalls_ExecuteAllSideEffects()
        {
            // Arrange
            var result = Result<string>.Success("test");
            var sideEffect1Called = false;
            var sideEffect2Called = false;

            // Act
            result
                .Tap(x => sideEffect1Called = true)
                .Tap(x => sideEffect2Called = true);

            // Assert
            Assert.True(sideEffect1Called);
            Assert.True(sideEffect2Called);
        }
    }

    public class ResultRailwayTapErrorTests
    {
        [Fact]
        public void TapError_OnFailure_ExecutesSideEffect()
        {
            // Arrange
            var error = Error.Create("TEST_ERROR", "Test error message");
            var result = Result<int>.Failure(error);
            var sideEffectExecuted = false;
            Error? capturedError = null;

            // Act
            result.TapError(e =>
            {
                sideEffectExecuted = true;
                capturedError = e;
            });

            // Assert
            Assert.True(sideEffectExecuted);
            Assert.Equal(error, capturedError);
        }

        [Fact]
        public void TapError_OnFailure_ReturnsOriginalResult()
        {
            // Arrange
            var error = Error.Create("TEST_ERROR", "Test error message");
            var result = Result<int>.Failure(error);

            // Act
            var tappedError = result.TapError(e => { });

            // Assert
            Assert.False(tappedError.IsSuccess);
            Assert.Equal(error, tappedError.Error);
        }

        [Fact]
        public void TapError_OnSuccess_DoesNotExecuteSideEffect()
        {
            // Arrange
            var result = Result<int>.Success(5);
            var sideEffectExecuted = false;

            // Act
            result.TapError(e => sideEffectExecuted = true);

            // Assert
            Assert.False(sideEffectExecuted);
            Assert.True(result.IsSuccess);
            Assert.Equal(5, result.Value);
        }

        [Fact]
        public void TapError_OnSuccess_ReturnsOriginalResult()
        {
            // Arrange
            var result = Result<int>.Success(5);

            // Act
            var tappedError = result.TapError(e => { });

            // Assert
            Assert.True(tappedError.IsSuccess);
            Assert.Equal(5, tappedError.Value);
        }

        [Fact]
        public void TapError_AllowsChaining_WithOtherMethods()
        {
            // Arrange
            var error = Error.Create("ERROR", "Test error");
            var result = Result<int>.Failure(error);
            var errorHandled = false;

            // Act
            var final = result
                .TapError(e => errorHandled = true)
                .Map(x => x * 2);

            // Assert
            Assert.False(final.IsSuccess);
            Assert.Equal(error, final.Error);
            Assert.True(errorHandled);
        }

        [Fact]
        public void TapError_MultipleCalls_ExecuteAllSideEffects()
        {
            // Arrange
            var error = Error.Create("ERROR", "Test error");
            var result = Result<int>.Failure(error);
            var errorHandled1 = false;
            var errorHandled2 = false;

            // Act
            result
                .TapError(e => errorHandled1 = true)
                .TapError(e => errorHandled2 = true);

            // Assert
            Assert.True(errorHandled1);
            Assert.True(errorHandled2);
        }
    }

    public class ResultRailwayIntegrationTests
    {
        [Fact]
        public void ComplexRailwayPipeline_WithSuccess_WorksCorrectly()
        {
            // Arrange
            var result = Result<int>.Success(5);

            // Act
            var final = result
                .Tap(x =>
                {
                    /* logging */
                })
                .Map(x => x * 2)
                .Bind(x => Result<int>.Success(x + 3))
                .Map(x => x.ToString());

            // Assert
            Assert.True(final.IsSuccess);
            Assert.Equal("13", final.Value);
        }

        [Fact]
        public void ComplexRailwayPipeline_WithFailureAtStart_PropagatesError()
        {
            // Arrange
            var error = Error.Create("INITIAL_ERROR", "Initial error");
            var result = Result<int>.Failure(error);
            var mapExecuted = false;

            // Act
            var final = result
                .Map(x =>
                {
                    mapExecuted = true;
                    return x * 2;
                })
                .Bind(x => Result<int>.Success(x + 3));

            // Assert
            Assert.False(final.IsSuccess);
            Assert.Equal(error, final.Error);
            Assert.False(mapExecuted);
        }

        [Fact]
        public void ComplexRailwayPipeline_WithFailureInMiddle_StopsProcessing()
        {
            // Arrange
            var error = Error.Create("BIND_ERROR", "Bind failed");
            var mapAfterBindExecuted = false;

            var result = Result<int>.Success(5)
                .Map(x => x * 2)
                .Bind(x => Result<int>.Failure(error));

            // Act
            var final = result
                .Map(x =>
                {
                    mapAfterBindExecuted = true;
                    return x + 3;
                });

            // Assert
            Assert.False(final.IsSuccess);
            Assert.Equal(error, final.Error);
            Assert.False(mapAfterBindExecuted);
        }

        [Fact]
        public async Task ComplexAsyncRailwayPipeline_WithSuccess_WorksCorrectly()
        {
            // Arrange
            var result = Result<int>.Success(5);

            // Act
            var mapped = await result.MapAsync(async x =>
            {
                await Task.Delay(5);
                return x * 2;
            });

            var bound = await mapped.BindAsync(async x =>
            {
                await Task.Delay(5);
                return Result<int>.Success(x + 3);
            });

            var final = await bound.MapAsync(async x =>
                await Task.FromResult(x.ToString()));

            // Assert
            Assert.True(final.IsSuccess);
            Assert.Equal("13", final.Value);
        }

        [Fact]
        public void RailwayPattern_WithTapAndTapError_HandlesAllCases()
        {
            // Arrange
            var successLog = new List<string>();
            var errorLog = new List<string>();

            var successResult = Result<int>.Success(10);

            // Act
            successResult
                .Tap(x => successLog.Add($"Success: {x}"))
                .TapError(e => errorLog.Add($"Error: {e.Message}"));

            // Assert
            Assert.Single(successLog);
            Assert.Empty(errorLog);

            // Arrange
            var failureLog = new List<string>();
            var failureLogErrors = new List<string>();
            var error = Error.Create("TEST", "Test error");
            var failureResult = Result<int>.Failure(error);

            // Act
            failureResult
                .Tap(x => failureLog.Add($"Success: {x}"))
                .TapError(e => failureLogErrors.Add($"Error: {e.Message}"));

            // Assert
            Assert.Empty(failureLog);
            Assert.Single(failureLogErrors);
        }
    }
}