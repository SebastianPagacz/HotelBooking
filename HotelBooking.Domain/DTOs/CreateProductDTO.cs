using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Domain.DTOs;

public class CreateProductDTO
{
    public string Name { get; set; } = string.Empty;
    public int NumberOfRooms { get; set; }
    public int NumberOfPeople { get; set; }
    public bool IsDeleted { get; set; }
    public decimal Price { get; set; }
}
