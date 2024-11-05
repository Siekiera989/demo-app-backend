using DemoApp.Core.Configuration;
using DemoApp.Core.Constants;
using Microsoft.Extensions.Hosting;

namespace DemoApp.Core.Services;

public class EnvironmentVariableService(IJwtConfiguration jwtConfiguration) : IHostedService
{
    private readonly Dictionary<string, string> _requiredVariables = new()
    {
        { EnvironmentVariables.JwtSecretKey, jwtConfiguration.JwtSecret },
        { EnvironmentVariables.Issuer, jwtConfiguration.Issuer },
        { EnvironmentVariables.Audience, jwtConfiguration.Audience },
    };

    public Task StartAsync(CancellationToken cancellationToken)
    {
        foreach (var variable in _requiredVariables
                     .Where(variable => 
                         string.IsNullOrEmpty(Environment.GetEnvironmentVariable(variable.Key))))
        {
            Environment.SetEnvironmentVariable(variable.Key, variable.Value);
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    public string GetEnvironmentVariable(string key)
    {
        return Environment.GetEnvironmentVariable(key) 
               ?? throw new KeyNotFoundException($"Environment variable '{key}' could not found");
    }
}