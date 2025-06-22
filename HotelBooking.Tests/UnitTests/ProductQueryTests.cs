using HotelBooking.Application.Query;
using HotelBooking.Domain.DTOs;
using HotelBooking.Domain.Exceptions.ProductExceptions;
using HotelBooking.Domain.Models;
using HotelBooking.Domain.Repositories;
using HotelBooking.Application.Services;
using HotelBooking.Application.Producer;
using Moq;

namespace HotelBooking.Tests.UnitTests;

public class ProductQueryTests
{
    [Fact]
    public async Task GetProductById_CachedProduct_ReturnsCachedDtoWithoutRepositoryAccess()
    {
        // arrange
        var product = new Product
        {
            Name = "Cached",
            NumberOfPeople = 2,
            NumberOfRooms = 1,
            Price = 100,
        };
        var cacheKey = "product:1";
        var mockCache = new Mock<IRedisCacheService>();
        mockCache.Setup(c => c.GetAsync<Product>(cacheKey)).ReturnsAsync(product);
        var mockRepo = new Mock<IRepository>();
        var mockProducer = new Mock<IKafkaProducer>();
        var handler = new GetProductByIdHandler(mockRepo.Object, mockCache.Object, mockProducer.Object);
        var query = new GetProductByIdQuery { Id = 1 };

        // act
        var result = await handler.Handle(query, CancellationToken.None);

        // assert
        Assert.Equal(product.Name, result.Name);
        mockRepo.Verify(r => r.GetProductByIdAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task GetProductById_NotCached_FetchesFromRepositoryAndCaches()
    {
        // arrange
        var product = new Product
        {
            Name = "FromRepo",
            NumberOfPeople = 3,
            NumberOfRooms = 2,
            Price = 200,
        };
        var cacheKey = "product:1";
        var mockCache = new Mock<IRedisCacheService>();
        mockCache.Setup(c => c.GetAsync<Product>(cacheKey)).ReturnsAsync((Product?)null);
        var mockRepo = new Mock<IRepository>();
        mockRepo.Setup(r => r.GetProductByIdAsync(1)).ReturnsAsync(product);
        var mockProducer = new Mock<IKafkaProducer>();
        var handler = new GetProductByIdHandler(mockRepo.Object, mockCache.Object, mockProducer.Object);
        var query = new GetProductByIdQuery { Id = 1 };

        // act
        var result = await handler.Handle(query, CancellationToken.None);

        // assert
        Assert.Equal(product.Price, result.Price);
        mockRepo.Verify(r => r.GetProductByIdAsync(1), Times.Once);
        mockCache.Verify(c => c.SetAsync(cacheKey, product, It.IsAny<TimeSpan>()), Times.Once);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("deleted")]
    public async Task GetProductById_MissingOrDeleted_ThrowsProductNotFoundException(string? scenario)
    {
        // arrange
        var cacheKey = "product:1";
        var mockCache = new Mock<IRedisCacheService>();
        mockCache.Setup(c => c.GetAsync<Product>(cacheKey)).ReturnsAsync((Product?)null);
        var mockRepo = new Mock<IRepository>();
        var mockProducer = new Mock<IKafkaProducer>();
        if (scenario == "deleted")
        {
            mockRepo.Setup(r => r.GetProductByIdAsync(1)).ReturnsAsync(new Product { IsDeleted = true });
        }
        else
        {
            mockRepo.Setup(r => r.GetProductByIdAsync(1)).ReturnsAsync((Product?)null);
        }
        var handler = new GetProductByIdHandler(mockRepo.Object, mockCache.Object, mockProducer.Object);
        var query = new GetProductByIdQuery { Id = 1 };

        // act & assert
        await Assert.ThrowsAsync<ProductNotFoundException>(() => handler.Handle(query, CancellationToken.None));
    }

    [Fact]
    public async Task GetAllProducts_FiltersDeletedProducts()
    {
        // arrange
        var products = new List<Product>
        {
            new Product { Name = "A", NumberOfRooms = 1, NumberOfPeople = 1, Price = 10, IsDeleted = false },
            new Product { Name = "B", NumberOfRooms = 2, NumberOfPeople = 2, Price = 20, IsDeleted = true },
        };
        var mockRepo = new Mock<IRepository>();
        mockRepo.Setup(r => r.GetAllProductsAsync()).ReturnsAsync(products);
        var handler = new GetAllProductsHandler(mockRepo.Object);

        // act
        var result = await handler.Handle(new GetAllProductsQuery(), CancellationToken.None);

        // assert
        Assert.Single(result);
        Assert.Equal("A", result.First().Name);
    }

    [Fact]
    public async Task GetAllProducts_NoProducts_ThrowsProductNotFoundException()
    {
        // arrange
        var mockRepo = new Mock<IRepository>();
        mockRepo.Setup(r => r.GetAllProductsAsync()).ReturnsAsync([]);
        var handler = new GetAllProductsHandler(mockRepo.Object);

        // act & assert
        await Assert.ThrowsAsync<ProductNotFoundException>(() => handler.Handle(new GetAllProductsQuery(), CancellationToken.None));
    }
}