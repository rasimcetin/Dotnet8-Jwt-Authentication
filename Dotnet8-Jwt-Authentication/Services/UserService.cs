
using Dotnet8_Jwt_Authentication.Data;
using Dotnet8_Jwt_Authentication.Dto;
using Dotnet8_Jwt_Authentication.Services;

namespace Dotnet8_Jwt_Authentication;

public class UserService(UserDbContext userDbContext) : IUserService
{
    public User Authenticate(LoginDto loginDto)
    {
        throw new NotImplementedException();
    }

    public UserDto CreateUser(CreateUserDto createUserDto)
    {
        throw new NotImplementedException();
    }

    public void DeleteUser(Guid id)
    {
        throw new NotImplementedException();
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

    public async Task<IEnumerable<User>> GetUsers()
    {
        throw new NotImplementedException();
    }

    public UserDto UpdateUser(Guid id, UpdateUserDto updateUserDto)
    {
        throw new NotImplementedException();
    }
}
