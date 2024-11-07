using System.Linq.Expressions;
using DemoApp.DataAccessLayer.Entities;
using DemoApp.DataAccessLayer.Repository;
using DemoApp.Services.Services;
using Moq;

namespace DemoApp.Services.UnitTests;

public class UserServiceTests
{
    private readonly Mock<IRepository<User>> _userRepositoryMock;
    private readonly Mock<IRepository<RefreshToken>> _refreshTokenRepositoryMock;
    private readonly Mock<IPasswordService> _passwordServiceMock;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _userRepositoryMock = new();
        _refreshTokenRepositoryMock = new();
        _passwordServiceMock = new();
        _userService = new(_userRepositoryMock.Object, _refreshTokenRepositoryMock.Object, _passwordServiceMock.Object);
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnTrue_WhenUserExistsAndPasswordIsCorrect()
    {
        // Arrange
        const string username = "testuser";
        const string password = "correctPassword";
        var user = new User { UserName = username, PasswordHash = "hashedPassword" };

        _userRepositoryMock
            .Setup(r => r.GetAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(user);

        _passwordServiceMock
            .Setup(p => p.VerifyPassword(password, user.PasswordHash))
            .Returns(true);

        // Act
        var result = await _userService.LoginAsync(username, password);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnFalse_WhenUserExistsAndPasswordIsIncorrect()
    {
        // Arrange
        const string username = "testuser";
        const string password = "incorrectPassword";
        var user = new User { UserName = username, PasswordHash = "hashedPassword" };

        _userRepositoryMock
            .Setup(r => r.GetAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(user);

        _passwordServiceMock
            .Setup(p => p.VerifyPassword(password, user.PasswordHash))
            .Returns(false);

        // Act
        var result = await _userService.LoginAsync(username, password);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnFalse_WhenUserDoesNotExist()
    {
        // Arrange
        const string username = "nonexistentuser";
        const string password = "anyPassword";

        _userRepositoryMock
            .Setup(r => r.GetAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync((User)null);

        // Act
        var result = await _userService.LoginAsync(username, password);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task RegisterAsync_ShouldReturnTrue_WhenUserIsRegisteredSuccessfully()
    {
        // Arrange
        const string requestEmail = "test@example.com";
        const string requestPassword = "password123";
        const string hashedPassword = "hashedPassword123";

        _passwordServiceMock
            .Setup(ps => ps.HashPassword(requestPassword))
            .Returns(hashedPassword);

        _userRepositoryMock
            .Setup(r => r.Add(It.IsAny<User>()))
            .Verifiable();

        _userRepositoryMock
            .Setup(r => r.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        var result = await _userService.RegisterAsync(requestEmail, requestPassword);

        // Assert
        Assert.True(result);
        _passwordServiceMock.Verify(ps => ps.HashPassword(requestPassword), Times.Once);
        _userRepositoryMock.Verify(r => r.Add(It.Is<User>(u =>
            u.UserName == requestEmail &&
            u.Email == requestEmail &&
            u.PasswordHash == hashedPassword)), Times.Once);
        _userRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task RegisterAsync_ShouldReturnFalse_WhenSaveChangesFails()
    {
        // Arrange
        const string requestEmail = "test@example.com";
        const string requestPassword = "password123";
        const string hashedPassword = "hashedPassword123";

        _passwordServiceMock
            .Setup(ps => ps.HashPassword(requestPassword))
            .Returns(hashedPassword);

        _userRepositoryMock
            .Setup(r => r.Add(It.IsAny<User>()))
            .Verifiable();

        _userRepositoryMock
            .Setup(r => r.SaveChangesAsync())
            .ReturnsAsync(0);

        // Act
        var result = await _userService.RegisterAsync(requestEmail, requestPassword);

        // Assert
        Assert.False(result);
        _passwordServiceMock.Verify(ps => ps.HashPassword(requestPassword), Times.Once);
        _userRepositoryMock.Verify(r => r.Add(It.Is<User>(u =>
            u.UserName == requestEmail &&
            u.Email == requestEmail &&
            u.PasswordHash == hashedPassword)), Times.Once);
        _userRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Logout_Should_Revoke_ActiveTokens()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var refreshTokens = new List<RefreshToken>
        {
            new RefreshToken { UserId = userId, IsRevoked = false, ExpiresAt = DateTime.UtcNow.AddMinutes(10) },
            new RefreshToken { UserId = userId, IsRevoked = false, ExpiresAt = DateTime.UtcNow.AddMinutes(20) }
        };

        // Mock the repository to return the test tokens
        _refreshTokenRepositoryMock
            .Setup(repo => repo.GetListAsync(It.IsAny<Expression<Func<RefreshToken, bool>>>()))
            .ReturnsAsync(refreshTokens);

        // Act
        await _userService.Logout(userId);

        // Assert
        foreach (var token in refreshTokens)
        {
            Assert.True(token.IsRevoked);
            Assert.True(token.ExpiresAt <= DateTime.UtcNow);
        }

        _refreshTokenRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Logout_Should_Not_Revoke_AlreadyRevokedTokens()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var refreshTokens = new List<RefreshToken>
        {
            new RefreshToken { UserId = userId, IsRevoked = true, ExpiresAt = DateTime.UtcNow.AddMinutes(10) },
            new RefreshToken { UserId = userId, IsRevoked = false, ExpiresAt = DateTime.UtcNow.AddMinutes(20) }
        };

        _refreshTokenRepositoryMock
            .Setup(repo => repo.GetListAsync(It.IsAny<Expression<Func<RefreshToken, bool>>>()))
            .ReturnsAsync(refreshTokens);

        // Act
        await _userService.Logout(userId);

        // Assert
        Assert.True(refreshTokens[0].IsRevoked);
        Assert.True(refreshTokens[1].IsRevoked);
        _refreshTokenRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Logout_Should_Not_Call_SaveChanges_When_No_Tokens_To_Revoke()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var refreshTokens = new List<RefreshToken>();

        _refreshTokenRepositoryMock
            .Setup(repo => repo.GetListAsync(It.IsAny<Expression<Func<RefreshToken, bool>>>()))
            .ReturnsAsync(refreshTokens);

        // Act
        await _userService.Logout(userId);

        // Assert
        _refreshTokenRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Never);
    }
}