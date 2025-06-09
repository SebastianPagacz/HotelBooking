namespace Booking.Domain.DTOs;

public class BookingDTO
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public decimal CaluclatedPrice { get; set; }
}
