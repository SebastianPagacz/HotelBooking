using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using HotelBooking.Domain.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;

namespace HotelBooking.Tests.IntegrationTests;

public class ReviewControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ReviewControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Get_ReturnsReviews()
    {
        var response = await _client.GetAsync("/api/review?productId=1");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var reviews = await response.Content.ReadFromJsonAsync<IEnumerable<ReviewDTO>>();
        Assert.NotNull(reviews);
    }

    [Fact]
    public async Task GetById_ReturnsReview()
    {
        var response = await _client.GetAsync("/api/review/1");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var review = await response.Content.ReadFromJsonAsync<ReviewDTO>();
        Assert.NotNull(review);
    }

    [Fact]
    public async Task Post_ReturnsCreatedReview()
    {
        var dto = new CreateReviewDTO
        {
            ProductId = 1,
            Title = "Test Review",
            Description = "Integration",
            Rating = 4
        };

        var response = await _client.PostAsJsonAsync("/api/review", dto);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var review = await response.Content.ReadFromJsonAsync<ReviewDTO>();
        Assert.NotNull(review);
        Assert.Equal(dto.Title, review!.Title);
    }

    [Fact]
    public async Task Put_ReturnsUpdatedReview()
    {
        var dto = new ReviewDTO
        {
            Title = "Updated",
            Description = "Updated",
            Rating = 5
        };

        var response = await _client.PutAsJsonAsync("/api/review/1", dto);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var review = await response.Content.ReadFromJsonAsync<ReviewDTO>();
        Assert.NotNull(review);
        Assert.Equal(dto.Title, review!.Title);
    }

    [Fact]
    public async Task Delete_ReturnsOk()
    {
        var response = await _client.DeleteAsync("/api/review/5");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}