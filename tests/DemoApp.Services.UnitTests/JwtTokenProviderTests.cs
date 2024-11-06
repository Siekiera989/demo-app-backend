using System.IdentityModel.Tokens.Jwt;
using DemoApp.Core.Constants;
using DemoApp.Core.Services;
using DemoApp.Services.Exceptions;
using DemoApp.Services.Services;
using Moq;
using Serilog;

namespace DemoApp.Services.UnitTests;

public class JwtTokenProviderTests
{
    private readonly Mock<ILogger> _loggerMock;
    private readonly Mock<IEnvironmentVariableProvider> _environmentVariableProviderMock;
    private readonly JwtTokenProvider _jwtTokenProvider;

    public JwtTokenProviderTests()
    {
        _loggerMock = new Mock<ILogger>();
        _environmentVariableProviderMock = new Mock<IEnvironmentVariableProvider>();
        _jwtTokenProvider = new JwtTokenProvider(_loggerMock.Object, _environmentVariableProviderMock.Object);
    }

    [Fact]
    public void GenerateToken_ShouldReturnToken_WhenEnvironmentVariablesAreSet()
    {
        // Arrange
        const string email = "test@example.com";
        const string secretKey = "9e91f0d235cf791f505bc00025de041681e0c5818c75e38aa11c3097a1031d07";
        const string issuer = "TestIssuer";
        const string audience = "TestAudience";

        _environmentVariableProviderMock
            .Setup(env => env.GetEnvironmentVariable(EnvironmentVariables.JwtSecretKey))
            .Returns(secretKey);
        _environmentVariableProviderMock
            .Setup(env => env.GetEnvironmentVariable(EnvironmentVariables.Issuer))
            .Returns(issuer);
        _environmentVariableProviderMock
            .Setup(env => env.GetEnvironmentVariable(EnvironmentVariables.Audience))
            .Returns(audience);

        // Act
        var token = _jwtTokenProvider.GenerateToken(email);

        // Assert
        Assert.False(string.IsNullOrEmpty(token));
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
        Assert.NotNull(securityToken);
        Assert.Contains(securityToken.Claims,
            claim => claim.Type == JwtRegisteredClaimNames.Email && claim.Value == email);
        Assert.Equal(issuer, securityToken.Issuer);
        Assert.Equal(audience, securityToken.Audiences.FirstOrDefault());
    }

    [Fact]
    public void GenerateToken_ShouldThrowException_WhenJwtSecretKeyIsMissing()
    {
        // Arrange
        const string email = "test@example.com";

        _environmentVariableProviderMock
            .Setup(env => env.GetEnvironmentVariable(EnvironmentVariables.JwtSecretKey))
            .Returns(string.Empty);

        // Act & Assert
        var exception = Assert.Throws<JwtTokenProviderException>(() => _jwtTokenProvider.GenerateToken(email));
        Assert.Equal($"Environment variable {EnvironmentVariables.JwtSecretKey} not found", exception.Message);

        _loggerMock.Verify(
            logger => logger.Error(It.Is<string>(msg => msg.Contains(EnvironmentVariables.JwtSecretKey))),
            Times.Once);
    }
}