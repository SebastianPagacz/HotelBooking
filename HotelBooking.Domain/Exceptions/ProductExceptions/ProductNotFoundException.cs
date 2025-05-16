using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Domain.Exceptions.ProductExceptions;

public class ProductNotFoundException : Exception
{
    public ProductNotFoundException() : base("Product was not found") { }
    public ProductNotFoundException(Exception innerException) : base("Product was not found", innerException) { }

}
