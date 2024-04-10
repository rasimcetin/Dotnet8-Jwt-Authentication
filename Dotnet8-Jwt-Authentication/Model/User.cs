namespace Dotnet8_Jwt_Authentication;

public class User
{
    public Guid Id { get; set; } 
    public string? Name { get; set; } = null;
    public string? SurName { get; set; } = null;
    public string? Email { get; set; }  = null;
    public string? Username { get; set;}
    public string? Password { get; set; } = null;
    public Role Role { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

}
