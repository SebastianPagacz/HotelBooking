using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Domain.DTOs;

public class CreateReviewDTO
{
    // Might change that to action in controller
    public int ProductId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Rating { get; set; }
}
