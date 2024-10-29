using System.Net;
using DemoApp.Models.Response.Abstract;

namespace DemoApp.Models.Response;

public class LoginResponse(HttpStatusCode statusCode, LoginResponseBody body)
    : HttpResponseBase<LoginResponseBody>(statusCode, body);

public class LoginResponseBody
{
    public string Token { get; set; }
    public string Message { get; set; }
}