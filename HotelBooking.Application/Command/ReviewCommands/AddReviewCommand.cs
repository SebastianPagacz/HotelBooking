using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBooking.Domain.DTOs;
using MediatR;

namespace HotelBooking.Application.Command;

public record AddReviewCommand : IRequest<ReviewDTO>
{
    public string Titile { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Rating { get; set; }
    public int ProductId { get; set; }
}
