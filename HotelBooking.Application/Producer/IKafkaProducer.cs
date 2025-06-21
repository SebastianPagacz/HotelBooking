using HotelBooking.Domain.Models.EventModels;

namespace HotelBooking.Application.Producer;

public interface IKafkaProducer
{
    Task SendBookingCreatedAsync(BookingCreatedEvent message);
}
