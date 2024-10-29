namespace DemoApp.Services.Contracts;

public interface IJwtTokenProvider
{
    string GenerateToken(string email);
}