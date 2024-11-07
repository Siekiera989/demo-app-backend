using DemoApp.DataAccessLayer.Entities;
using DemoApp.DataAccessLayer.Repository;
using DemoApp.Services.Contracts;

namespace DemoApp.Services.Services;

public class UserService(
    IRepository<User> userRepository,
    IRepository<RefreshToken> refreshTokenRepository,
    IPasswordService passwordService) : IUserService
{
    public async Task<bool> LoginAsync(string username, string password)
    {
        var user = await userRepository.GetAsync(x => x.Email == username || x.UserName == username);

        if (user == null)
            return false;

        var isPasswordCorrect = passwordService.VerifyPassword(password, user.PasswordHash);

        return isPasswordCorrect;
    }

    public async Task<bool> RegisterAsync(string requestEmail, string requestPassword)
    {
        var hashedPassword = passwordService.HashPassword(requestPassword);
        var newUser = new User()
        {
            UserName = requestEmail,
            Email = requestEmail,
            PasswordHash = hashedPassword,
            CreatedAt = DateTime.UtcNow,
        };

        userRepository.Add(newUser);
        var saveResult = await userRepository.SaveChangesAsync();

        return saveResult == 1;
    }

    public async Task Logout(Guid userId)
    {
        var refreshTokens = await refreshTokenRepository.GetListAsync(rt =>
            rt.UserId == userId && !rt.IsRevoked && rt.ExpiresAt > DateTime.UtcNow);

        if (!refreshTokens.Any())
        {
            return;
        }

        foreach (var token in refreshTokens)
        {
            token.IsRevoked = true;
            token.ExpiresAt = DateTime.UtcNow;
        }

        await refreshTokenRepository.SaveChangesAsync();
    }
}