using Autofac;
using DemoApp.Core.Configuration;
using Microsoft.Extensions.Configuration;

namespace DemoApp.Core.DependencyInjection;

public class ConfigurationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        var configuration = configurationBuilder.Build();
        
        var jwtConfiguration = new JwtConfiguration();
        configuration.GetSection(nameof(JwtConfiguration)).Bind(jwtConfiguration);
        
        builder.RegisterInstance(jwtConfiguration)
            .As<IJwtConfiguration>()
            .SingleInstance();
    }
}