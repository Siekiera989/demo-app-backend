namespace DemoApp.Core.Configuration;

public class DbConfiguration : IDbConfiguration
{
    public string ConnectionString { get; set; }
}

public interface IDbConfiguration
{
    string ConnectionString { get; }
}