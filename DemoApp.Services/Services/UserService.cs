using DemoApp.DataAccessLayer.Entities;
using DemoApp.DataAccessLayer.Repository;
using DemoApp.Services.Contracts;

namespace DemoApp.Services.Services;

public class UserService(IRepository<User> repository, IPasswordService passwordService) : IUserService
{
    public async Task<bool> LoginAsync(string username, string password)
    {
        var user = await repository.GetAsync(x => x.Email == username || x.UserName == username);

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

        repository.Add(newUser);
        var saveResult = await repository.SaveChangesAsync();

        return saveResult == 1;
    }
}