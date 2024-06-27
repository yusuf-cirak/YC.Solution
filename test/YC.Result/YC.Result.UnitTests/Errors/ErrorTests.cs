namespace YC.Result.UnitTests.Errors;

public class ErrorTests
{
    [Fact]
    public void Create_WithTitleDetailStatus_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        string expectedTitle = "Test Title";
        string expectedDetail = "Test Detail";
        int expectedStatus = 500;

        // Act
        Error error = Error.Create(expectedTitle, expectedDetail, expectedStatus);

        // Assert
        Assert.Equal(expectedTitle, error.Title);
        Assert.Equal(expectedDetail, error.Detail);
        Assert.Equal(expectedStatus, error.Status);
    }

    [Fact]
    public void Create_WithDetail_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        string expectedDetail = "Test Detail";
        int expectedStatus = 400;

        // Act
        Error error = Error.Create(expectedDetail);

        // Assert
        Assert.Equal(string.Empty, error.Title);
        Assert.Equal(expectedDetail, error.Detail);
        Assert.Equal(expectedStatus, error.Status);
    }

    [Fact]
    public void Create_WithStatus_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        int expectedStatus = 401;

        // Act
        Error error = Error.Create(expectedStatus);

        // Assert
        Assert.Equal(string.Empty, error.Title);
        Assert.Equal(string.Empty, error.Detail);
        Assert.Equal(expectedStatus, error.Status);
    }

    [Fact]
    public void None_ShouldReturnDefaultError()
    {
        // Arrange
        Error noneError = Error.None;

        // Assert
        Assert.Equal(string.Empty, noneError.Title);
        Assert.Equal(string.Empty, noneError.Detail);
        Assert.Equal(0, noneError.Status);
    }
}