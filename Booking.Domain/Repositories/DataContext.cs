using Microsoft.EntityFrameworkCore;
using Booking.Domain.Models;

namespace Booking.Domain.Repositories;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    public DbSet<BookingModel> Bookings { get; set; }
}
