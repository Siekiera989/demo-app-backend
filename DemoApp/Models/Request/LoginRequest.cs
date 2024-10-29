using DemoApp.Models.Request.Abstract;

namespace DemoApp.Models.Request;

public class LoginRequest : HttpRequestBase<LoginRequestBody>
{
    public LoginRequest(Uri url, LoginRequestBody body)
        : base("POST", url, body) { }
}

public class LoginRequestBody
{
    public string Username { get; set; }
    public string Password { get; set; }
}
