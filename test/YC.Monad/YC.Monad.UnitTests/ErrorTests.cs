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
    }
}
