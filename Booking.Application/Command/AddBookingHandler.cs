using Booking.Domain.DTOs;
using Booking.Domain.Models;
using Booking.Domain.Repositories;
using Booking.Domain.Enums;
using MediatR;

namespace Booking.Application.Command;

public class AddBookingHandler(IRepository repository) : IRequestHandler<AddBookingCommand, BookingDTO>
{
    public async Task<BookingDTO> Handle(AddBookingCommand request, CancellationToken cancellationToken)
    {
        var calcPrice = 100;

        var newBooking = new BookingModel
        {
            ProductId = request.ProductId,
            ClientId = request.ClientId,
            //PaymentId = request.PaymentId,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            CaluclatedPrice = calcPrice,
            CreatedAt = DateTime.UtcNow,
            Status = BookingStatusEnum.Pending,
        };

        await repository.AddAsync(newBooking);

        var bookingDTO = new BookingDTO
        {
            Id = newBooking.Id,
            ProductId = newBooking.ProductId,
            StartDate = newBooking.StartDate,
            EndDate = newBooking.EndDate,
            CaluclatedPrice = newBooking.CaluclatedPrice,
        };

        return bookingDTO;
    }
}
