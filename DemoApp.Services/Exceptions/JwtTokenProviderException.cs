using System.Runtime.Serialization;

namespace DemoApp.Services.Exceptions;

[Serializable]
public class JwtTokenProviderException : Exception
{
    public JwtTokenProviderException()
    {
    }

    public JwtTokenProviderException(string? message) : base(message)
    {
    }

    public JwtTokenProviderException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}