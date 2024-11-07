using System.Net;

namespace DemoApp.Models.Response.Abstract;

public abstract class HttpResponseBase<T>(HttpStatusCode statusCode, T body = default)
{
    public HttpStatusCode StatusCode { get; set; } = statusCode;
    public T Body { get; set; } = body;

    public override string ToString()
    {
        return $"StatusCode: {StatusCode}, Body: {Body}";
    }
}