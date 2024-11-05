using Autofac;
using DemoApp.Core.Configuration;
using DemoApp.Core.Extensions;
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

        var jwtConfiguration = configuration.BindConfig<JwtConfiguration>();
        var dbConfiguration = configuration.BindConfig<DbConfiguration>();

        builder.RegisterInstance(jwtConfiguration).As<IJwtConfiguration>().SingleInstance();
        builder.RegisterInstance(dbConfiguration).As<IDbConfiguration>().SingleInstance();
    }
}