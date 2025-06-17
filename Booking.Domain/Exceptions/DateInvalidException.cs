namespace Booking.Domain.Exceptions;

public class DateInvalidException : Exception
{
    public DateInvalidException() : base() { }
    public DateInvalidException(string message) : base(message) { }
    public DateInvalidException(string message, Exception innerException) : base(message, innerException) { }
}
