using Autofac;
using Autofac.Extensions.DependencyInjection;
using DemoApp.Core.DependencyInjection;
using DemoApp.DataAccessLayer.DependencyInjection;
using DemoApp.Services.DependencyInjection;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule<ConfigurationModule>();
    containerBuilder.RegisterModule<DataAccessLayerModule>();
    containerBuilder.RegisterModule<CoreServicesModule>();
    containerBuilder.RegisterModule<LoggerModule>();
    containerBuilder.RegisterModule<ServicesModule>();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();