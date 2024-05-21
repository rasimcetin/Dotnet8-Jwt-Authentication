using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Dotnet8_Jwt_Authentication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Dotnet8_Jwt_Authentication;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController(IUserService userService) : ControllerBase
{
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var token = await userService.Authenticate(loginDto);

        if (string.IsNullOrEmpty(token))
        {
            return Unauthorized();
        }

        return Ok (token);
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(RegisterDto registerDto){
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await userService.Register(registerDto);
            return Ok("User registered successfully");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
