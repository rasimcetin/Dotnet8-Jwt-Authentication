namespace Dotnet8_Jwt_Authentication.Dto;

public record UserDto(Guid Id, string? Name, string? SurName, string? Email, string? Username, Role Role, DateTimeOffset CreatedAt, DateTimeOffset UpdatedAt);

public record CreateUserDto(string Name, string SurName, string Email, string Username, string Password, Role Role);

public record UpdateUserDto(string Name, string SurName, string Email, string Username, Role Role);
