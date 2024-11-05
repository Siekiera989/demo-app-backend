using Microsoft.Extensions.Configuration;

namespace DemoApp.Core.Extensions;

public static class ConfigurationExtensions
{
    public static T BindConfig<T>(this IConfiguration configuration) where T : new()
    {
        var configInstance = new T();
        configuration.GetSection(typeof(T).Name).Bind(configInstance);
        return configInstance;
    }
}