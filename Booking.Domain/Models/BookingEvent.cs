using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Models;

public class BookingEvent
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ClientEmail { get; set; } = string.Empty;
    public decimal PricePerNight { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
}
