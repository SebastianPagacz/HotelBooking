using Booking.Application.Command;
using Booking.Domain.Exceptions;
using Booking.Domain.Models;
using Booking.Domain.Repositories;
using Booking.Application.Services;
using Moq;

namespace HotelBooking.Tests.UnitTests;

public class AddBookingHandlerTests
{
    [Fact]
    public async Task Handle_StartDateAfterEndDate_ThrowsDateInvalidException()
    {
        var mockRepo = new Mock<IRepository>();
        var mockEmail = new Mock<IEmailService>();
        var handler = new AddBookingHandler(mockRepo.Object, mockEmail.Object);
        var command = new AddBookingCommand
        {
            ProductId = 1,
            ClientId = 1,
            StartDate = DateOnly.FromDateTime(DateTime.Now).AddDays(5),
            EndDate = DateOnly.FromDateTime(DateTime.Now).AddDays(1)
        };

        await Assert.ThrowsAsync<DateInvalidException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_StartDateInPast_ThrowsDateInvalidException()
    {
        var mockRepo = new Mock<IRepository>();
        var mockEmail = new Mock<IEmailService>();
        var handler = new AddBookingHandler(mockRepo.Object, mockEmail.Object);
        var command = new AddBookingCommand
        {
            ProductId = 1,
            ClientId = 1,
            StartDate = DateOnly.FromDateTime(DateTime.Now).AddDays(-1),
            EndDate = DateOnly.FromDateTime(DateTime.Now).AddDays(1)
        };

        await Assert.ThrowsAsync<DateInvalidException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ValidDates_SavesBooking()
    {
        var mockRepo = new Mock<IRepository>();
        var mockEmail = new Mock<IEmailService>();
        var handler = new AddBookingHandler(mockRepo.Object, mockEmail.Object);
        var command = new AddBookingCommand
        {
            ProductId = 1,
            ClientId = 1,
            StartDate = DateOnly.FromDateTime(DateTime.Now).AddDays(1),
            EndDate = DateOnly.FromDateTime(DateTime.Now).AddDays(2)
        };

        await handler.Handle(command, CancellationToken.None);

        mockRepo.Verify(r => r.AddAsync(It.IsAny<BookingModel>()), Times.Once);
    }
}