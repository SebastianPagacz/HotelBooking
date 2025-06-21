namespace HotelBooking.Domain.Models.EventModels;

public class BookingCreatedEvent
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal PricePerNight { get; set; }
}
