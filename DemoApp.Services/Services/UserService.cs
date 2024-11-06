using DemoApp.DataAccessLayer.Entities;
using DemoApp.DataAccessLayer.Repository;
using DemoApp.Services.Contracts;

namespace DemoApp.Services.Services;

public class UserService(IRepository<User> repository, IPasswordService passwordService) : IUserService
{
    private readonly IRepository<User> _repository = repository;
    private readonly IPasswordService _passwordService = passwordService;

    public async Task<bool> LoginAsync(string username, string password)
    {
        var user = await _repository.GetAsync(x => x.Email == username || x.UserName == username);

        if (user == null)
            return false;

        var isPasswordCorrect = _passwordService.VerifyPassword(password, user.PasswordHash);

        return isPasswordCorrect;
    }
}