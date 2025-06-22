using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBooking.Application.Command;
using HotelBooking.Application.Services;
using HotelBooking.Domain.Models;
using HotelBooking.Domain.Repositories;
using System.Threading;
using Moq;

namespace HotelBooking.Tests.UnitTests;

public class CQRSTests
{
    [Fact]
    public async Task Handle_ValidCommand_AddsProductsToRepository()
    {
        // arrange
        var mockRepo = new Mock<IRepository>();
        var mockCache = new Mock<IRedisCacheService>();
        var handler = new AddProductHandler(mockRepo.Object, mockCache.Object);
        var command = new AddProductCommand
        {
            Name = "test",
            NumberOfRooms = 100,
            NumberOfPeople = 1,
            IsDeleted = false,
            Price = 200,
        };

        // act
        var result = await handler.Handle(command, CancellationToken.None);

        // assert
        mockRepo.Verify(r => r.AddProductAsync(It.Is<Product>(p =>
            p.Name == command.Name && p.Price == command.Price && p.NumberOfRooms == command.NumberOfRooms)), Times.Once);
    }
}
