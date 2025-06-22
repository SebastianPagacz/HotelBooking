using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using User.Application.Services;
using User.Domain.Models;

namespace HotelBooking.Tests.UnitTests;

public class JWTTokenServiceTests
{
    [Fact]
    public void GenerateToken_ContainsExpectedClaimsAndExpiration()
    {
        // arrange
        var settings = new JwtSettings
        {
            Key = "super_secret_test_key_that_is_long_enough123",
            Issuer = "TestIssuer",
            Audience = "TestAudience",
            ExpiresInMinutes = 60
        };
        var service = new JWTTokenService(Options.Create(settings));
        var userId = 123;
        var roles = new List<string> { "Admin", "User" };
        var expectedExpiration = DateTime.UtcNow.AddMinutes(settings.ExpiresInMinutes);

        // act
        var tokenString = service.GenerateToken(userId, roles);
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(tokenString);

        // assert
        Assert.Equal(settings.Issuer, token.Issuer);
        Assert.Equal(settings.Audience, token.Audiences.Single());
        Assert.Contains(token.Claims, c => c.Type == ClaimTypes.NameIdentifier && c.Value == userId.ToString());
        foreach (var role in roles)
        {
            Assert.Contains(token.Claims, c => c.Type == ClaimTypes.Role && c.Value == role);
        }
        var diff = (token.ValidTo - expectedExpiration).Duration();
        Assert.True(diff < TimeSpan.FromSeconds(5));
    }
}