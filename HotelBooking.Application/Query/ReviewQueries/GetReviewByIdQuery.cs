using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBooking.Domain.DTOs;
using MediatR;

namespace HotelBooking.Application.Query;

public class GetReviewByIdQuery : IRequest<ReviewDTO>
{
    public int Id { get; set; }
}
