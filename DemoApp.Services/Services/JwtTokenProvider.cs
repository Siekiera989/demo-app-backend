using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DemoApp.Core.Constants;
using DemoApp.Core.Services;
using DemoApp.Services.Contracts;
using DemoApp.Services.Exceptions;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace DemoApp.Services.Services;

public class JwtTokenProvider : IJwtTokenProvider
{
    private readonly ILogger _logger;
    private readonly EnvironmentVariableService _environmentVariableService;

    public JwtTokenProvider(ILogger logger, EnvironmentVariableService environmentVariableService)
    {
        _logger = logger;
        _environmentVariableService = environmentVariableService;
    }
        public string GenerateToken(string email)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key =_environmentVariableService.GetEnvironmentVariable(EnvironmentVariables.JwtSecretKey);

            if (string.IsNullOrWhiteSpace(key))
            {
                var message = $"Environment variable {EnvironmentVariables.JwtSecretKey} not found";
                _logger.Error(message);
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
                Expires = DateTime.UtcNow.AddMinutes(60),
                Issuer = _environmentVariableService.GetEnvironmentVariable(EnvironmentVariables.Issuer),
                Audience = _environmentVariableService.GetEnvironmentVariable(EnvironmentVariables.Audience),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), 
                        SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
}