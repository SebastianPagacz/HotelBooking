using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBooking.Domain.DTOs;
using MediatR;

namespace HotelBooking.Application.Command;

public record AddProductCommand : IRequest<ProductDTO>
{
    public string Name { get; set; } = string.Empty;
    public int NumberOfRooms { get; set; }
    public int NumberOfPeople { get; set; }
    public bool IsDeleted { get; set; }
    public decimal Price { get; set; }
}
