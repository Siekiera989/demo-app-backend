using Autofac;
using DemoApp.Services.Contracts;
using DemoApp.Services.Services;

namespace DemoApp.Services.DependencyInjection;

public class ServicesModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<UserService>().As<IUserService>().SingleInstance();
        builder.RegisterType<JwtTokenProvider>().As<IJwtTokenProvider>().SingleInstance();
    }
}