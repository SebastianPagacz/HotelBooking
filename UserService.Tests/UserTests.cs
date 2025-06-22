using System.Net;
using System.Net.Http.Json;
using User.Domain.Models.Request;
using User.Domain.Models.DTOs;

namespace UserService.Tests;

public class UserTests
{
    private HttpClient CreateClient()
    {
        var factory = new CustomWebApplicationFactory();
        return factory.CreateClient();
    }

    [Fact]
    public async Task RegisterUser_ReturnsSuccess()
    {
        using var client = CreateClient();
        var dto = new RegisterDTO { Username = "testuser", Email = "test@example.com", Password = "Pass123$" };
        var response = await client.PostAsJsonAsync("/api/auth/register", dto);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task LoginUser_ReturnsToken()
    {
        using var client = CreateClient();
        var dto = new RegisterDTO { Username = "loginuser", Email = "login@example.com", Password = "Pass123$" };
        await client.PostAsJsonAsync("/api/auth/register", dto);
        var login = new LoginRequest { Username = dto.Username, Password = dto.Password };
        var response = await client.PostAsJsonAsync("/api/auth/login", login);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var token = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrWhiteSpace(token));
    }

    [Fact]
    public async Task ResetPasswordFlow_WorksCorrectly()
    {
        using var client = CreateClient();
        var dto = new RegisterDTO { Username = "resetuser", Email = "reset@example.com", Password = "Pass123$" };
        await client.PostAsJsonAsync("/api/auth/register", dto);
        var forgot = await client.PostAsync($"/api/user/forgot-password?email={dto.Email}", null);
        Assert.Equal(HttpStatusCode.OK, forgot.StatusCode);
        var resetDto = await forgot.Content.ReadFromJsonAsync<ResetPasswordDto>();
        Assert.NotNull(resetDto);
        Assert.False(string.IsNullOrEmpty(resetDto!.Token));

        var resetRequest = new ResetPasswordRequestDto { Email = dto.Email, NewPassword = "NewPass123$", Token = resetDto.Token };
        var resetResponse = await client.PostAsJsonAsync("/api/user/reset-password", resetRequest);
        Assert.Equal(HttpStatusCode.OK, resetResponse.StatusCode);

        var login = new LoginRequest { Username = dto.Username, Password = "NewPass123$" };
        var loginRes = await client.PostAsJsonAsync("/api/auth/login", login);
        Assert.Equal(HttpStatusCode.OK, loginRes.StatusCode);
    }

    [Fact]
    public async Task GetUser_WithValidToken_ReturnsUser()
    {
        using var client = CreateClient();
        var dto = new RegisterDTO { Username = "infoUser", Email = "info@example.com", Password = "Pass123$" };
        await client.PostAsJsonAsync("/api/auth/register", dto);
        var login = new LoginRequest { Username = dto.Username, Password = dto.Password };
        var loginRes = await client.PostAsJsonAsync("/api/auth/login", login);
        var token = await loginRes.Content.ReadAsStringAsync();
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var userResponse = await client.GetAsync("/api/user/1");
        Assert.Equal(HttpStatusCode.OK, userResponse.StatusCode);
    }
}