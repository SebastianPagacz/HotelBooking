using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBooking.Domain.DTOs;
using MediatR;

namespace HotelBooking.Application.Query;

public record GetProductAndReviewsQuery : IRequest<ProductReviewDTO>
{
    public int ProductId { get; set; }
}
