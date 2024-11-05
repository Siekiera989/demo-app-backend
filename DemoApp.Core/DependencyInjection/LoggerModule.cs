using Autofac;
using Serilog;

namespace DemoApp.Core.DependencyInjection;

public class LoggerModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterInstance(Log.Logger).As<ILogger>().SingleInstance();
    }
}