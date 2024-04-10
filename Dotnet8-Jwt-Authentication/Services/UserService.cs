
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Dotnet8_Jwt_Authentication.Data;
using Dotnet8_Jwt_Authentication.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
namespace Dotnet8_Jwt_Authentication.Services;

public class UserService(UserDbContext userDbContext, IConfiguration configuration) : IUserService
{
    public Task<string> Authenticate(LoginDto loginDto)
    {
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

        string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return Task.FromResult(tokenString);
    }

    public async Task<Guid> CreateUser(CreateUserDto createUserDto)
    {
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

        return user.Id;

    }

    public async Task DeleteUser(Guid id)
    {
        var user = await userDbContext.Users.FindAsync(id);
        if (user == null) 
            throw new Exception("User not found");
        
        userDbContext.Users.Remove(user);
        await userDbContext.SaveChangesAsync();

    }

    public async Task<UserDto> GetUser(Guid id)
    {
        var user = await userDbContext.Users.FindAsync(id);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        var userDto = new UserDto(user.Id, user.Name, user.SurName, user.Email, user.Username, user.Role, user.CreatedAt, user.UpdatedAt);

        return userDto;
    }

    public async Task<IEnumerable<UserDto>> GetUsers()
    {
       var users = await ((from user in userDbContext.Users
                           orderby user.CreatedAt descending
                           select new UserDto(user.Id, 
                                              user.Name, 
                                              user.SurName, 
                                              user.Email, 
                                              user.Username, 
                                              user.Role, 
                                              user.CreatedAt, 
                                              user.UpdatedAt))
                     .ToListAsync());
        return users;
    }

    public async Task<UserDto> UpdateUser(Guid id, UpdateUserDto updateUserDto)
    {
        var user = await userDbContext.Users.FindAsync(id);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        user.UpdatedAt = DateTimeOffset.UtcNow;
        user.Name = updateUserDto.Name;
        user.SurName = updateUserDto.SurName;
        user.Email = updateUserDto.Email;
        user.Username = updateUserDto.Username;

        userDbContext.Users.Update(user);
        await userDbContext.SaveChangesAsync();
        
        return new UserDto(user.Id, user.Name, user.SurName, user.Email, user.Username, user.Role, user.CreatedAt, user.UpdatedAt);
    }
}
