using System.Net;
using DemoApp.Models.Response.Abstract;

namespace DemoApp.Models.Response;

public class RegistrationResponse(HttpStatusCode statusCode, RegistrationResponseBody body)
    : HttpResponseBase<RegistrationResponseBody>(statusCode, body);

public class RegistrationResponseBody
{
    public string Message { get; set; }
}