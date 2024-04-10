using Dotnet8_Jwt_Authentication.Dto;

namespace Dotnet8_Jwt_Authentication.Services;

public interface IUserService
{
    Task<IEnumerable<User>> GetUsers();
    Task<UserDto> GetUser(Guid id);
    UserDto CreateUser(CreateUserDto createUserDto);
    UserDto UpdateUser(Guid id, UpdateUserDto updateUserDto);
    void DeleteUser(Guid id);

    User Authenticate(LoginDto loginDto);

}
