using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

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
}
