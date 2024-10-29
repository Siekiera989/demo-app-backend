using System.Net;

namespace DemoApp.Models.Response.Abstract;

public abstract class HttpResponseBase<T>
{
    public HttpStatusCode StatusCode { get; set; }
    public T Body { get; set; }

    protected HttpResponseBase(HttpStatusCode statusCode, T body = default)
    {
        StatusCode = statusCode;
        Body = body;
    }

    public override string ToString()
    {
        return $"StatusCode: {StatusCode}, Body: {Body}";
    }
}