using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using DemoApp.Attributes;
using FluentAssertions;

namespace DemoApp.UnitTests;

public class PasswordComplexityAttributeTests
{
    private readonly PasswordComplexityAttribute _attribute = new();

    [Theory]
    [InlineData("Password1!")]
    [InlineData("Complex@123")]
    public void IsValid_WithValidPassword_ShouldReturnSuccess(string password)
    {
        // Act
        var result = _attribute.GetValidationResult(password, new ValidationContext(new object()));

        // Assert
        result.Should().Be(ValidationResult.Success);
    }

    [Theory]
    [InlineData("P@1")]
    [InlineData("Abc123")]
    [InlineData("Password!")]
    [InlineData("12345678!")]
    public void IsValid_WithInvalidPassword_ShouldReturnError(string password)
    {
        // Act
        var result = _attribute.GetValidationResult(password, new ValidationContext(new object()));

        // Assert
        result.Should().NotBe(ValidationResult.Success);
        Debug.Assert(result != null, nameof(result) + " != null");
        result.ErrorMessage.Should().Be("Password do not meet complexity rules.");
    }
}