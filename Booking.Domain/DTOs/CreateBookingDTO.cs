namespace Booking.Domain.DTOs;

public record CreateBookingDTO
{
    public int ProductId { get; set; }
    public int ClientId { get; set; }
    //public int PaymentId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
}
