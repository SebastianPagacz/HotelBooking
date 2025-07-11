using Microsoft.EntityFrameworkCore;
using HotelBooking.Domain.Repositories;
using HotelBooking.Domain.Seeders;
using HotelBooking.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using HotelBooking.Domain.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using Microsoft.Extensions.DependencyInjection;
using HotelBooking.Application.Services;
using HotelBooking.Application.Producer;

namespace HotelBooking;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddDbContext<DataContext>(options => options.UseInMemoryDatabase("TestDb"));
        builder.Services.AddScoped<IRepository, Repository>();
        builder.Services.AddScoped<IHotelBookingSeeder, HotelBookingSeeder>();
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(HotelBooking.Application.AssemblyReference).Assembly));

        builder.Services.AddSingleton<IKafkaProducer, KafkaProducer>();

        // JwtConfig
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
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtConfig.Issuer,

                    ValidateAudience = false,

                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Key)),
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

        // Swagger config        
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Products Service", Version = "v1" });

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

        builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var config = builder.Configuration.GetSection("ConnectionStrings")["Redis"];
            return ConnectionMultiplexer.Connect(config);
        });

        builder.Services.AddScoped<IRedisCacheService, RedisCacheService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.UseAuthorization();


        app.MapControllers();

        var scope = app.Services.CreateScope();
        var seeder = scope.ServiceProvider.GetRequiredService<IHotelBookingSeeder>();
        await seeder.SeedAsync();

        app.Run();
    }
}
