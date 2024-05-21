namespace Dotnet8_Jwt_Authentication;

public record LoginDto(string Username, string Password);

public record RegisterDto(string Username, string Password, string ConfirmPassword);

