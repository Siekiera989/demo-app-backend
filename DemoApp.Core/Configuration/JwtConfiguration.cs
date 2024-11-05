namespace DemoApp.Core.Configuration;

public class JwtConfiguration : IJwtConfiguration
{
    public string JwtSecret { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
}

public interface IJwtConfiguration
{
    public string JwtSecret { get;  }
    public string Issuer { get; }
    public string Audience { get; }
}