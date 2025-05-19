using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Domain.Models;

public class Product : BaseModel
{
    public string Name { get; set; } = string.Empty;
    public int NumberOfRooms { get; set; }
    public int NumberOfPeople { get; set; }
    public decimal Price { get; set; }
    public List<Review> Reviews { get; set; } = new List<Review>();
}
