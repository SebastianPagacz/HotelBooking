using Booking.Domain.Models;

namespace Booking.Domain.Repositories;

public interface IRepository
{
    Task<BookingModel> AddAsync(BookingModel booking);
    Task<IEnumerable<BookingModel>> GetAsync();
    Task<BookingModel> GetByIdAsync(BookingModel booking);
    Task<BookingModel> UpdateAsync(BookingModel booking);
}
