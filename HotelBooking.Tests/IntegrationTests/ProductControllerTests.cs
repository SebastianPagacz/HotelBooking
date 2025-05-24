using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HotelBooking.Domain.DTOs;
using HotelBooking.Domain.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;

namespace HotelBooking.Tests.IntegrationTests;

public class ProductControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    public ProductControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Get_CheckBaseParams_ReturnsSuccessStatusCode()
    {
        // arrange & act
        var response = await _client.GetAsync("/api/product");

        // assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetById_CheckBaseParams_ReturnsSuccessStatusCode()
    {
        // arrange & act
        var response = await _client.GetAsync("/api/product/1");

        // assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetById_NonExistentId_ReturnsNotFoundStatusCode()
    {
        // arrange & act
        var response = await _client.GetAsync("/api/product/999");

        // assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Post_CreateNewProduct_ReturnsSuccess()
    {
        // arange
        var product = new CreateProductDTO
        {
            Name = "Test",
            NumberOfPeople = 1,
            NumberOfRooms = 1,
            Price = 100,
            IsDeleted = false,
        };

        var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(product),
            Encoding.UTF8,
            "application/json");

        // act
        var response = await _client.PostAsync("api/product", content);

        // assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

}
