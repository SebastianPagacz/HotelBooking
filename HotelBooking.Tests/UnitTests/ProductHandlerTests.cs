using HotelBooking.Application.Command;
using HotelBooking.Application.Services;
using HotelBooking.Domain.Exceptions.ProductExceptions;
using HotelBooking.Domain.Models;
using HotelBooking.Domain.Repositories;
using Moq;

namespace HotelBooking.Tests.UnitTests;

public class ProductHandlerTests
{
    [Fact]
    public async Task AddProductHandler_ExistingName_ThrowsException()
    {
        var repoMock = new Mock<IRepository>();
        var cacheMock = new Mock<IRedisCacheService>();
        repoMock.Setup(r => r.GetProductByNameAsync("Hotel")).ReturnsAsync(new Product());
        var handler = new AddProductHandler(repoMock.Object, cacheMock.Object);
        var command = new AddProductCommand
        {
            Name = "Hotel",
            NumberOfRooms = 1,
            NumberOfPeople = 1,
            Price = 100,
            IsDeleted = false
        };

        await Assert.ThrowsAsync<ProductAlreadyExistsException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task UpdateProductHandler_UpdatesRepositoryAndCache()
    {
        var repoMock = new Mock<IRepository>();
        var cacheMock = new Mock<IRedisCacheService>();
        var product = new Product
        {
            Id = 1,
            Name = "Old",
            NumberOfRooms = 1,
            NumberOfPeople = 1,
            Price = 10
        };
        repoMock.Setup(r => r.GetProductByIdAsync(1)).ReturnsAsync(product);
        var handler = new UpdateProductHandler(repoMock.Object, cacheMock.Object);

        var command = new UpdateProductCommand
        {
            Id = 1,
            Name = "New",
            NumberOfRooms = 2,
            NumberOfPeople = 3,
            IsDeleted = false,
            Price = 20
        };

        await handler.Handle(command, CancellationToken.None);

        repoMock.Verify(r => r.UpdateProductAsync(It.Is<Product>(p =>
            p.Name == command.Name &&
            p.NumberOfRooms == command.NumberOfRooms &&
            p.NumberOfPeople == command.NumberOfPeople &&
            p.IsDeleted == command.IsDeleted)), Times.Once);
        cacheMock.Verify(c => c.SetAsync(
            "product:1",
            It.Is<Product>(p => p.Name == command.Name),
            It.IsAny<TimeSpan>()), Times.Once);
    }

    [Fact]
    public async Task DeleteProductHandler_MarksDeletedAndRemovesCache()
    {
        var repoMock = new Mock<IRepository>();
        var cacheMock = new Mock<IRedisCacheService>();
        var product = new Product { Id = 2, IsDeleted = false };
        repoMock.Setup(r => r.GetProductByIdAsync(2)).ReturnsAsync(product);
        var handler = new DeleteProductHandler(repoMock.Object, cacheMock.Object);
        var command = new DeleteProductCommand { Id = 2 };

        await handler.Handle(command, CancellationToken.None);

        Assert.True(product.IsDeleted);
        repoMock.Verify(r => r.UpdateProductAsync(product), Times.Once);
        cacheMock.Verify(c => c.RemoveAsync("product:2"), Times.Once);
    }
}