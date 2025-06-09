using Booking.Domain.DTOs;
using MediatR;

namespace Booking.Application.Command;

public class AddBookingCommand : IRequest<BookingDTO>
{
    public int ProductId { get; set; }
    public int ClientId { get; set; }
    //public int PaymentId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
}
