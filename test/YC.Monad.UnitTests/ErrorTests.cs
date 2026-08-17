using Xunit;

namespace YC.Monad.UnitTests
{
    public class ErrorTests
    {
        [Fact]
        public void Error_Create_WithCodeAndMessage_SetsPropertiesCorrectly()
        {
            // Arrange
            var code = "Error.Test";
            var message = "This is a test error";

            // Act
            var error = Error.Create(code, message);

            // Assert
            Assert.Equal(code, error.Code);
            Assert.Equal(message, error.Message);
            Assert.Equal(0, error.Status);
        }

        [Fact]
        public void Error_Create_WithCodeMessageAndStatus_SetsPropertiesCorrectly()
        {
            // Arrange
            var code = "Error.Test";
            var message = "This is a test error";
            var status = 500;

            // Act
            var error = Error.Create(code, message, status);

            // Assert
            Assert.Equal(code, error.Code);
            Assert.Equal(message, error.Message);
            Assert.Equal(status, error.Status);
        }

        [Fact]
        public void Error_ImplicitConversion_FromString_SetsMessageCorrectly()
        {
            // Arrange
            string errorMessage = "Implicit error message";

            // Act
            Error error = errorMessage;

            // Assert
            Assert.Equal(string.Empty, error.Code);
            Assert.Equal(errorMessage, error.Message);
            Assert.Equal(0, error.Status);
        }

        [Fact]
        public void ErrorCache_HasCorrectDefaultErrors()
        {
            // Assert
            Assert.Equal("Error.Unauthorized", ErrorCache.Unauthorized.Code);
            Assert.Equal("Unauthorized", ErrorCache.Unauthorized.Message);
            Assert.Equal(401, ErrorCache.Unauthorized.Status);

            Assert.Equal("Error.BadRequest", ErrorCache.BadRequest.Code);
            Assert.Equal("Bad Request", ErrorCache.BadRequest.Message);
            Assert.Equal(400, ErrorCache.BadRequest.Status);

            Assert.Equal("Error.NotFound", ErrorCache.NotFound.Code);
            Assert.Equal("Not Found", ErrorCache.NotFound.Message);
            Assert.Equal(404, ErrorCache.NotFound.Status);

            Assert.Equal("Error.Forbidden", ErrorCache.Forbidden.Code);
            Assert.Equal("Forbidden", ErrorCache.Forbidden.Message);
            Assert.Equal(403, ErrorCache.Forbidden.Status);
        }

        [Fact]
        public void Error_IsValueType()
        {
            // Assert
            Assert.True(typeof(Error).IsValueType);
        }

        [Fact]
        public void Error_WithAttribute_CanBeReadBackViaTryGetAttribute()
        {
            // Arrange
            var error = Error.Create("Error.Test", "message");

            // Act
            var withAttribute = error.WithAttribute("userId", 42);

            // Assert
            Assert.True(withAttribute.TryGetAttribute("userId", out var value));
            Assert.Equal(42, value);
        }

        [Fact]
        public void Error_TryGetAttribute_ReturnsFalseWhenMissing()
        {
            // Arrange
            var error = Error.Create("Error.Test", "message");

            // Act
            var found = error.TryGetAttribute("missing", out var value);

            // Assert
            Assert.False(found);
            Assert.Null(value);
        }

        [Fact]
        public void Error_WithAttribute_DoesNotMutateOriginalInstance()
        {
            // Arrange
            var original = Error.Create("Error.Test", "message");

            // Act
            var withAttribute = original.WithAttribute("key", "value");

            // Assert
            Assert.False(original.TryGetAttribute("key", out _));
            Assert.True(withAttribute.TryGetAttribute("key", out var value));
            Assert.Equal("value", value);
        }

        [Fact]
        public void Error_WithAttribute_OnStructCopy_DoesNotAffectOriginal()
        {
            // Regression test: Error is a struct, so assigning it copies the struct.
            // WithAttribute must not mutate a Dictionary shared between the copy and the original.

            // Arrange
            var original = Error.Create("Error.Test", "message");
            var copy = original;

            // Act
            copy.WithAttribute("key", "value");

            // Assert
            Assert.False(original.TryGetAttribute("key", out _));
        }

        [Fact]
        public void Error_WithAttributes_MergesMultipleAttributes()
        {
            // Arrange
            var error = Error.Create("Error.Test", "message");
            var attributes = new Dictionary<string, object>
            {
                ["a"] = 1,
                ["b"] = "two",
            };

            // Act
            var withAttributes = error.WithAttributes(attributes);

            // Assert
            Assert.True(withAttributes.TryGetAttribute("a", out var a));
            Assert.Equal(1, a);
            Assert.True(withAttributes.TryGetAttribute("b", out var b));
            Assert.Equal("two", b);
        }

        [Fact]
        public void Error_WithAttribute_Chained_KeepsAllAttributes()
        {
            // Arrange
            var error = Error.Create("Error.Test", "message");

            // Act
            var result = error
                .WithAttribute("a", 1)
                .WithAttribute("b", 2);

            // Assert
            Assert.True(result.TryGetAttribute("a", out var a));
            Assert.Equal(1, a);
            Assert.True(result.TryGetAttribute("b", out var b));
            Assert.Equal(2, b);
        }

        [Fact]
        public void Error_Default_TryGetAttribute_ReturnsFalseInsteadOfThrowing()
        {
            // Regression test: default(Error) bypasses field initializers (it's a struct),
            // so the attribute store must tolerate being unset without throwing.

            // Act
            var found = default(Error).TryGetAttribute("key", out var value);

            // Assert
            Assert.False(found);
            Assert.Null(value);
        }

        [Fact]
        public void Error_Default_WithAttribute_DoesNotThrow()
        {
            // Act
            var error = default(Error).WithAttribute("key", "value");

            // Assert
            Assert.True(error.TryGetAttribute("key", out var value));
            Assert.Equal("value", value);
        }
    }
}
