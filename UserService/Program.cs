using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using User.Domain.Models.Entities;
using User.Domain.Repository;
using UserService.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using User.Domain.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using User.Application.Services;
using User.Domain.Seeders;
using System.Security.Claims;
using Microsoft.OpenApi.Models;

namespace UserService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddDbContext<DataContext>(options =>
            options.UseInMemoryDatabase("UserDb"));
        builder.Services.AddScoped<IRepository, Repository>();
        builder.Services.AddScoped<IPasswordHasher<UserEntity>, PasswordHasher<UserEntity>>();
        builder.Services.AddScoped<IJWTTokenService, JWTTokenService>();
        builder.Services.AddScoped<IRoleSeeder, RoleSeeder>();

        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(User.Application.AssemblyReference).Assembly));

        //JwtConfig
        var jwtSettings = builder.Configuration.GetSection("Jwt");
        builder.Services.Configure<JwtSettings>(jwtSettings);

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options =>
            {
                var jwtConfig = jwtSettings.Get<JwtSettings>();
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtConfig.Issuer,
                    ValidAudience = jwtConfig.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Key)),

                    RoleClaimType = ClaimTypes.Role
                };
            });

        builder.Services.AddAuthorization(options => 
        {
            options.AddPolicy("AdminOnly", policy =>
            policy.RequireRole("Admin"));

            options.AddPolicy("EmployeeOnly", policy =>
            policy.RequireRole("Admin", "Employee"));

            options.AddPolicy("CustomerOnly", policy =>
            policy.RequireRole("Admin", "Employee", "Customer"));
        });

        builder.Services.AddControllers();
        
        // swagger config
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "User Service", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "JWT format: Bearer",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference 
                        { 
                            Type = ReferenceType.SecurityScheme, 
                            Id = "Bearer" 
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        var scope = app.Services.CreateScope();
        var seeder = scope.ServiceProvider.GetRequiredService<IRoleSeeder>();
        seeder.SeedAsync();

        app.Run();
    }
}
