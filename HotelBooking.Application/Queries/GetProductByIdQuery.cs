using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBooking.Domain.DTOs;
using MediatR;

namespace HotelBooking.Application.Queries;

public record GetProductByIdQuery : IRequest<ProductDTO>
{
    public int Id { get; set; }
}
