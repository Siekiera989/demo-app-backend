using DemoApp.Services.Services;

namespace DemoApp.Services.UnitTests;

public class PasswordServiceTests
{
    private readonly PasswordService _passwordService = new();

    [Fact]
    public void HashPassword_ShouldReturnDifferentHashesForSamePassword()
    {
        // Arrange
        string password = "TestPassword123";

        // Act
        string hash1 = _passwordService.HashPassword(password);
        string hash2 = _passwordService.HashPassword(password);

        // Assert
        Assert.NotEqual(hash1, hash2);
    }

    [Fact]
    public void VerifyPassword_ShouldReturnTrueForCorrectPassword()
    {
        // Arrange
        string password = "TestPassword123";
        string hashedPassword = _passwordService.HashPassword(password);

        // Act
        bool isPasswordValid = _passwordService.VerifyPassword(password, hashedPassword);

        // Assert
        Assert.True(isPasswordValid);
    }

    [Fact]
    public void VerifyPassword_ShouldReturnFalseForIncorrectPassword()
    {
        // Arrange
        string password = "TestPassword123";
        string incorrectPassword = "WrongPassword456";
        string hashedPassword = _passwordService.HashPassword(password);

        // Act
        bool isPasswordValid = _passwordService.VerifyPassword(incorrectPassword, hashedPassword);

        // Assert
        Assert.False(isPasswordValid);
    }

    [Fact]
    public void VerifyPassword_ShouldThrowFormatExceptionForInvalidHashFormat()
    {
        // Arrange
        string password = "TestPassword123";
        string invalidHash = "InvalidHashFormat";

        // Act & Assert
        Assert.Throws<FormatException>(() => _passwordService.VerifyPassword(password, invalidHash));
    }
}