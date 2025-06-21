using Booking.Domain.Models;
using Microsoft.EntityFrameworkCore;

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

    public async Task<IEnumerable<BookingModel>> GetAsync()
    {
        return await context.Bookings.ToListAsync();
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
