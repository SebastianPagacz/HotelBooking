using System.Net;
using System.Net.Http.Json;
using Booking.Domain.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace BookingService.Tests.IntegrationTests;

public class BookingControllerTests : IClassFixture<BookingServiceWebApplicationFactory>
{
    private readonly HttpClient _client;

    public BookingControllerTests(BookingServiceWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Post_AddBooking_ValidDates_ReturnsOk()
    {
        var dto = new CreateBookingDTO
        {
            ProductId = 1,
            ClientId = 1,
            StartDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)),
            EndDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(2))
        };

        var response = await _client.PostAsJsonAsync("/api/Booking/AddBooking", dto);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Post_AddBooking_InvalidDates_ReturnsNotAcceptable()
    {
        var dto = new CreateBookingDTO
        {
            ProductId = 1,
            ClientId = 1,
            StartDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(2)),
            EndDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1))
        };

        var response = await _client.PostAsJsonAsync("/api/Booking/AddBooking", dto);
        
        Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
    }
}