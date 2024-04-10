using Microsoft.EntityFrameworkCore;

namespace Dotnet8_Jwt_Authentication.Data;

public class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
}
