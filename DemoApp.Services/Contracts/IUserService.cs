namespace DemoApp.Services.Contracts;

public interface IUserService
{
    Task<bool> LoginAsync(string username, string password);
    Task<bool> RegisterAsync(string requestEmail, string requestPassword);
    Task Logout(Guid userId);
}