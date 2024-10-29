namespace DemoApp.Models.Request.Abstract;

public abstract class HttpRequestBase<T>
{
    public string Method { get; set; }
    public Uri Url { get; set; }
    public T Body { get; set; }

    protected HttpRequestBase(string method, Uri url, T body = default)
    {
        Method = method;
        Url = url;
        Body = body;
    }

    public override string ToString()
    {
        return $"Method: {Method}, Url: {Url}, Body: {Body}";
    }
}