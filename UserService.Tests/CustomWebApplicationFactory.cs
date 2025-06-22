using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using UserService;
using User.Domain.Repository;
using User.Application.Services;

namespace UserService.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureServices(services =>
        {
            // replace DbContext with in memory
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<DataContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }
            services.AddDbContext<DataContext>(options => options.UseInMemoryDatabase("InMemoryDb" + Guid.NewGuid()));

            // replace token service with fake implementation
            services.RemoveAll<IJWTTokenService>();
            services.AddSingleton<IJWTTokenService, FakeJwtTokenService>();

            // configure test authentication scheme
            services.AddAuthentication(defaultScheme: TestAuthHandler.AuthenticationScheme)
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(TestAuthHandler.AuthenticationScheme, options => { });
        });
    }
}

public class FakeJwtTokenService : IJWTTokenService
{
    public string GenerateToken(int userId, List<string> roles)
    {
        return $"{userId}:{string.Join(',', roles)}";
    }
}

public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public const string AuthenticationScheme = "Test";

    public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var parts = token.Split(':', 2);
        var userId = parts[0];
        var roles = parts.Length > 1 && !string.IsNullOrEmpty(parts[1]) ? parts[1].Split(',', StringSplitOptions.RemoveEmptyEntries) : Array.Empty<string>();

        var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, userId) };
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var identity = new ClaimsIdentity(claims, AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, AuthenticationScheme);
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}