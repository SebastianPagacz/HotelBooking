using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Domain.Exceptions.ProductExceptions;

public class ProductAlreadyExistsException : Exception
{
    public ProductAlreadyExistsException() : base("Product already exisits") { }
    public ProductAlreadyExistsException(Exception innerException) : base("Product already exisits", innerException) { }
}
