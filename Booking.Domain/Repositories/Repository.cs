using Booking.Domain.Models;

namespace Booking.Domain.Repositories;

public class Repository(DataContext context) : IRepository
{
    public async Task<BookingModel> AddAsync(BookingModel booking)
    {
        await context.Bookings
            .AddAsync(booking);
        await context.SaveChangesAsync();

        return booking;
    }

    public Task<IEnumerable<BookingModel>> GetAsync(BookingModel booking)
    {
        throw new NotImplementedException();
    }

    public Task<BookingModel> GetByIdAsync(BookingModel booking)
    {
        throw new NotImplementedException();
    }

    public Task<BookingModel> UpdateAsync(BookingModel booking)
    {
        throw new NotImplementedException();
    }
}
