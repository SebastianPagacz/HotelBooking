using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Domain.Models.Requests;

public class BookingDateRequest
{
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
}
