using System.Security.Cryptography.Xml;
using Dotnet8_Jwt_Authentication;
using Dotnet8_Jwt_Authentication.Data;
using Dotnet8_Jwt_Authentication.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<UserDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("userDb"));
});

builder.Services.AddScoped<IUserService, UserService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
        .WithOrigins("http://localhost:5173")
        .AllowAnyMethod()
        .AllowAnyHeader());
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters{
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"])),
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"]
    };
});


builder.Services.AddAuthorizationBuilder()
.AddPolicy("Admin", policy => {
    policy.RequireAuthenticatedUser();
    policy.RequireRole("Admin");
})
.AddPolicy("User", policy => {
    policy.RequireAuthenticatedUser();
    policy.RequireRole("User");
})
.AddPolicy("UserOrAdmin", policy => {
    policy.RequireAuthenticatedUser();
    policy.RequireRole("Admin", "User");
});

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Dotnet8 JWT Authentication",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "Bearer",
                Name = "Bearer",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header
            },
            new List<string>()
        }
    });
}
);

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using (var scope = app.Services.CreateScope()){
        var dbContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();
        User adminUser = new User
        {
            Id = Guid.NewGuid(),
            Username = "admin",
            Password = "admin",
            Role = Role.Admin,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow,
            Name = "Admin",
            SurName = "Admin",
            Email = "admin@localhost"
        };

        User user = new User
        {
            Id = Guid.NewGuid(),
            Username = "user",
            Password = "user",
            Role = Role.User,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow,
            Name = "User",
            SurName = "User",
            Email = "user@localhost"
        };

        dbContext.Users.Add(adminUser);
        dbContext.Users.Add(user);
        dbContext.SaveChanges();
        
    }
}

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
