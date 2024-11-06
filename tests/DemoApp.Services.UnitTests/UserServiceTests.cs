using System.Linq.Expressions;
using DemoApp.DataAccessLayer.Entities;
using DemoApp.DataAccessLayer.Repository;
using DemoApp.Services.Services;
using Moq;

namespace DemoApp.Services.UnitTests;

public class UserServiceTests
{
    private readonly Mock<IRepository<User>> _repositoryMock;
    private readonly Mock<IPasswordService> _passwordServiceMock;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _repositoryMock = new Mock<IRepository<User>>();
        _passwordServiceMock = new Mock<IPasswordService>();
        _userService = new UserService(_repositoryMock.Object, _passwordServiceMock.Object);
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnTrue_WhenUserExistsAndPasswordIsCorrect()
    {
        // Arrange
        const string username = "testuser";
        const string password = "correctPassword";
        var user = new User { UserName = username, PasswordHash = "hashedPassword" };

        _repositoryMock
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

        _repositoryMock
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

        _repositoryMock
            .Setup(r => r.GetAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync((User)null);

        // Act
        var result = await _userService.LoginAsync(username, password);

        // Assert
        Assert.False(result);
    }
}