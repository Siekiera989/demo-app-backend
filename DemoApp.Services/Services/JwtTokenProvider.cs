using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DemoApp.Core.Configuration;
using DemoApp.Core.Constants;
using DemoApp.Core.Services;
using DemoApp.Services.Contracts;
using DemoApp.Services.Exceptions;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace DemoApp.Services.Services;

public class JwtTokenProvider(
    ILogger logger,
    IEnvironmentVariableProvider environmentVariableProvider,
    IJwtConfiguration jwtConfiguration)
    : IJwtTokenProvider
{
    public string GenerateToken(string email)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = environmentVariableProvider.GetEnvironmentVariable(EnvironmentVariables.JwtSecretKey);

        if (string.IsNullOrWhiteSpace(key))
        {
            var message = $"Environment variable {EnvironmentVariables.JwtSecretKey} not found";
            logger.Error(message);
            throw new JwtTokenProviderException(message);
        }

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, email),
            new(JwtRegisteredClaimNames.Email, email)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(jwtConfiguration.TokenLifetime),
            Issuer = environmentVariableProvider.GetEnvironmentVariable(EnvironmentVariables.Issuer),
            Audience = environmentVariableProvider.GetEnvironmentVariable(EnvironmentVariables.Audience),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                    SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}