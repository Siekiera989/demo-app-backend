namespace DemoApp.Models.Request.Abstract;

public abstract class HttpRequestBase<T>
{ 
    public T Body { get; set; }

    protected HttpRequestBase(T body = default)
    {
        Body = body;
    }

    public override string ToString()
    {
        return $"Body: {Body}";
    }
}