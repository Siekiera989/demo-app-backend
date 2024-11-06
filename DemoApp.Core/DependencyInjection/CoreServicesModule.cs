using Autofac;
using DemoApp.Core.Services;
using Microsoft.Extensions.Hosting;

namespace DemoApp.Core.DependencyInjection;

public class CoreServicesModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<EnvironmentVariableService>()
            .As<IHostedService>()
            .As<IEnvironmentVariableProvider>()
            .AsSelf()
            .SingleInstance();
    }
}