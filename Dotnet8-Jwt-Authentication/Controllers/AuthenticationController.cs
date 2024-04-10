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
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var token = await userService.Authenticate(loginDto);

        return Ok (token);
    }
}
