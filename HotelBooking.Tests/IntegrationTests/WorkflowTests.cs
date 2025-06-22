using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Booking.Domain.DTOs;
using HotelBooking.Domain.DTOs;
using HotelBooking.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using User.Domain.Models.Request;
using Xunit;
using HotelBooking.Tests.IntegrationTests.Factories;

public class WorkflowTests : IClassFixture<HotelBookingFactory>, IClassFixture<BookingServiceFactory>, IClassFixture<UserServiceFactory>
{
    private readonly HttpClient _hotelClient;
    private readonly HttpClient _bookingClient;
    private readonly HttpClient _userClient;

    public WorkflowTests(HotelBookingFactory hotelFactory, BookingServiceFactory bookingFactory, UserServiceFactory userFactory)
    {
        _hotelClient = hotelFactory.CreateClient();
        _bookingClient = bookingFactory.CreateClient();
        _userClient = userFactory.CreateClient();
    }

    [Fact]
    public async Task RegisterLoginCreateProductAndBook()
    {
        var register = new RegisterDTO
        {
            Username = "tester",
            Email = "tester@example.com",
            Password = "Pass123$"
        };
        var registerContent = new StringContent(JsonSerializer.Serialize(register), Encoding.UTF8, "application/json");
        var registerResponse = await _userClient.PostAsync("/api/auth/register", registerContent);
        registerResponse.EnsureSuccessStatusCode();

        var login = new LoginRequest
        {
            Username = register.Username,
            Password = register.Password
        };
        var loginContent = new StringContent(JsonSerializer.Serialize(login), Encoding.UTF8, "application/json");
        var loginResponse = await _userClient.PostAsync("/api/auth/login", loginContent);
        loginResponse.EnsureSuccessStatusCode();
        var token = await loginResponse.Content.ReadAsStringAsync();

        var product = new CreateProductDTO
        {
            Name = "Test room",
            NumberOfPeople = 2,
            NumberOfRooms = 1,
            Price = 50,
            IsDeleted = false
        };
        _hotelClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var productContent = new StringContent(JsonSerializer.Serialize(product), Encoding.UTF8, "application/json");
        var productResponse = await _hotelClient.PostAsync("/api/product", productContent);
        productResponse.EnsureSuccessStatusCode();

        var listResponse = await _hotelClient.GetAsync("/api/product");
        listResponse.EnsureSuccessStatusCode();
        var products = JsonSerializer.Deserialize<List<Product>>(await listResponse.Content.ReadAsStringAsync(), new JsonSerializerOptions{PropertyNameCaseInsensitive = true});
        var createdProduct = products!.Last();

        var booking = new CreateBookingDTO
        {
            ProductId = createdProduct.Id,
            ClientId = 1,
            StartDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)),
            EndDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(2))
        };
        var bookingContent = new StringContent(JsonSerializer.Serialize(booking), Encoding.UTF8, "application/json");
        var bookingResponse = await _bookingClient.PostAsync("/api/booking/AddBooking", bookingContent);
        bookingResponse.EnsureSuccessStatusCode();
        var bookingResult = JsonSerializer.Deserialize<BookingDTO>(await bookingResponse.Content.ReadAsStringAsync(), new JsonSerializerOptions{PropertyNameCaseInsensitive = true});

        Assert.Equal(createdProduct.Id, bookingResult!.ProductId);
    }
}