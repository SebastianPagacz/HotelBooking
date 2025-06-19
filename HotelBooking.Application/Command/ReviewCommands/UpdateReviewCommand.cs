using HotelBooking.Domain.DTOs;
using MediatR;

namespace HotelBooking.Application.Command.ReviewCommands;

public record UpdateReviewCommand : IRequest<ReviewDTO>
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Rating { get; set; }
}
