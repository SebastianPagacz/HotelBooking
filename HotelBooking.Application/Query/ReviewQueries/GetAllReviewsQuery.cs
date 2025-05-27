using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBooking.Domain.DTOs;
using HotelBooking.Domain.Models;
using MediatR;

namespace HotelBooking.Application.Query;

public record GetAllReviewsQuery : IRequest<IEnumerable<ReviewDTO>>
{
    public int Id { get; set; }
}
