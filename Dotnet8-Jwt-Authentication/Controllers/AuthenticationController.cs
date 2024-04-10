using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Dotnet8_Jwt_Authentication;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController(IConfiguration configuration) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, loginDto.Username),
            new Claim (ClaimTypes.Role, Role.Admin.ToString())    
        };

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddDays(int.Parse(configuration["Jwt:AccessTokenExpiration"])),
            signingCredentials: credentials
        );

        return Ok (new { token = new JwtSecurityTokenHandler().WriteToken(token) });
    }
}
