using Dotnet8_Jwt_Authentication.Data;
using Dotnet8_Jwt_Authentication.Dto;
using Dotnet8_Jwt_Authentication.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dotnet8_Jwt_Authentication.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserService userService) : ControllerBase
{
    [HttpGet]
    [Authorize(Policy = "UserOrAdmin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        var users = await userService.GetUsers();
        return Ok(users);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "UserOrAdmin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
    public async Task<ActionResult<UserDto>> GetUser(Guid id)
    {
        try
        {
            var userDto = await userService.GetUser(id);
            return Ok(userDto);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    [Authorize(Policy = "Admin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
    
    public async Task<ActionResult<Guid>> CreateUser(CreateUserDto createUserDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = await userService.CreateUser(createUserDto);
       
        return CreatedAtAction(nameof(GetUser), new { id = userId }, userId);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
    public async Task<ActionResult<UserDto>> UpdateUser(Guid id, UpdateUserDto updateUserDto)
    {
        if (!ModelState.IsValid) 
            return BadRequest(ModelState);
        
        try
        {
            var userDto = await userService.UpdateUser(id, updateUserDto);
            return Ok(userDto);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        try{
            await userService.DeleteUser(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}
