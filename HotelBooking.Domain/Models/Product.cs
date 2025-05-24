using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Domain.Models;

public class Product : BaseModel
{
    /// <summary>
    /// Name of the product
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Number of rooms that the product have
    /// </summary>
    public int NumberOfRooms { get; set; }
    /// <summary>
    /// Number of people that the porduct can acomodate
    /// </summary>
    public int NumberOfPeople { get; set; }
    /// <summary>
    /// Price per night
    /// </summary>
    public decimal Price { get; set; }
    /// <summary>
    /// List of all the reviews assigned to the ptoduct
    /// </summary>
    public List<Review> Reviews { get; set; } = new List<Review>();
}
