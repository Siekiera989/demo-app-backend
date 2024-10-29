namespace DemoApp.Services.Contracts;

public interface IUserService
{
    Task<bool> LoginAsync(string username, string password);
}