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
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await userService.LoginAsync(request.Email, request.Password);

        if (!result)
        {
            return new LoginResponse(HttpStatusCode.Unauthorized, new LoginResponseBody()
            {
                Token = string.Empty,
                Message = "Login failed"
            });
        }

        var token = jwtTokenProvider.GenerateToken(request.Email);

        return new LoginResponse(HttpStatusCode.OK, new LoginResponseBody()
        {
            Token = token,
            Message = "Login successful"
        });
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RegistrationResponse>> Register(RegisterRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await userService.RegisterAsync(request.Email, request.Password);

        return !result
            ? new RegistrationResponse(HttpStatusCode.BadRequest, new RegistrationResponseBody()
            {
                Message = "Could not register user"
            })
            : new RegistrationResponse(HttpStatusCode.OK, new RegistrationResponseBody()
            {
                Message = "Registration successful"
            });
    }
}