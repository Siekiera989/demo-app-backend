using DemoApp.Models.Request.Abstract;

namespace DemoApp.Models.Request;

public class LoginRequest(LoginRequestBody body) : HttpRequestBase<LoginRequestBody>(body);

public class LoginRequestBody
{
    public string Username { get; set; }
    public string Password { get; set; }
}
