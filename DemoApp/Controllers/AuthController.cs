using System.Net;
using DemoApp.Models.Request;
using DemoApp.Models.Response;
using DemoApp.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace DemoApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IUserService userService, IJwtTokenProvider jwtTokenProvider)
    : Controller
{
    private readonly IJwtTokenProvider _jwtTokenProvider = jwtTokenProvider;

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
    {
        var result = await userService.LoginAsync(request.Body.Username, request.Body.Password);

        if (!result)
        {
            return new LoginResponse(HttpStatusCode.Unauthorized, new LoginResponseBody()
            {
                Token = string.Empty,
                Message = "Login failed"
            });
        }
        
        var token = _jwtTokenProvider.GenerateToken(request.Body.Username);
        
        return new LoginResponse(HttpStatusCode.OK, new LoginResponseBody()
        {
            Token = token,
            Message = "Login successful"
        });
    }
}