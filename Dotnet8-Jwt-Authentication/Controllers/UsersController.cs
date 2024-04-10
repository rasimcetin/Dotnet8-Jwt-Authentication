using Dotnet8_Jwt_Authentication.Data;
using Dotnet8_Jwt_Authentication.Dto;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dotnet8_Jwt_Authentication.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(UserDbContext userDbContext) : ControllerBase
{
    [HttpGet]
    [Authorize(Policy = "Admin")]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        var users = await ((from user in userDbContext.Users
                           orderby user.CreatedAt descending
                           select new UserDto(user.Id, user.Name, user.SurName, user.Email, user.Username, user.Role, user.CreatedAt, user.UpdatedAt)).ToListAsync());
        return Ok(users);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "User")]
    public async Task<ActionResult<UserDto>> GetUser(Guid id)
    {
        var user = await userDbContext.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        var userDto = new UserDto(user.Id, user.Name, user.SurName, user.Email, user.Username, user.Role, user.CreatedAt, user.UpdatedAt);
        return Ok(userDto);
    }

    [HttpPost]
    //[Authorize(Policy = "Admin")]
    public async Task<ActionResult<UserDto>> CreateUser(CreateUserDto createUserDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTimeOffset.UtcNow, 
            UpdatedAt = DateTimeOffset.UtcNow, 
            Name = createUserDto.Name, 
            SurName = createUserDto.SurName, 
            Email = createUserDto.Email, 
            Username = createUserDto.Username,
            Role = createUserDto.Role
        };

        userDbContext.Users.Add(user);
        await userDbContext.SaveChangesAsync();
        return Ok(user);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "Admin")]
    public async Task<ActionResult<UserDto>> UpdateUser(Guid id, UpdateUserDto updateUserDto)
    {
        if (!ModelState.IsValid) 
            return BadRequest(ModelState);

        var user = await userDbContext.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        user.UpdatedAt = DateTimeOffset.UtcNow;
        user.Name = updateUserDto.Name;
        user.SurName = updateUserDto.SurName;
        user.Email = updateUserDto.Email;
        user.Username = updateUserDto.Username;

        userDbContext.Users.Update(user);
        await userDbContext.SaveChangesAsync();
        
        var userDto = new UserDto(user.Id, user.Name, user.SurName, user.Email, user.Username, user.Role, user.CreatedAt, user.UpdatedAt);
        return Ok(userDto);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var user = await userDbContext.Users.FindAsync(id);
        if (user == null) return NotFound();
        
        userDbContext.Users.Remove(user);
        await userDbContext.SaveChangesAsync();
        return NoContent();
    }
}
