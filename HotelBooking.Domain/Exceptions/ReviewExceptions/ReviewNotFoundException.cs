using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Domain.Exceptions.ReviewExceptions;

public class ReviewNotFoundException : Exception
{
    public ReviewNotFoundException() : base("Review not found") { }
    public ReviewNotFoundException(Exception innerException) : base("Review not found", innerException) { }
}
