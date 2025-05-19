using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Domain.DTOs;

public class ProductDTO
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int NumberOfRooms { get; set; }
    public int NumberOfPeople { get; set; }
}
