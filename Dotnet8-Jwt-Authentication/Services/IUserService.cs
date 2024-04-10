using Dotnet8_Jwt_Authentication.Dto;

namespace Dotnet8_Jwt_Authentication.Services;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetUsers();
    Task<UserDto> GetUser(Guid id);
    Task<Guid> CreateUser(CreateUserDto createUserDto);
    Task<UserDto> UpdateUser(Guid id, UpdateUserDto updateUserDto);
    Task DeleteUser(Guid id);

    Task<string> Authenticate(LoginDto loginDto);

}
