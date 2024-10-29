using DemoApp.Services.Contracts;

namespace DemoApp.Services.Services;

public class UserService : IUserService
{
    public async Task<bool> LoginAsync(string username, string password)
    {
        await Task.Delay(100);
        return true;
    }
}