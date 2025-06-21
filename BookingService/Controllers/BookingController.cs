using Booking.Application.Command;
using Booking.Application.Queries;
using Booking.Domain.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookingController(IMediator mediator) : ControllerBase
{
    [HttpPost("add-booking")]
    public async Task<IActionResult> PostAsync(CreateBookingDTO bookingDto)
    {
        var result = await mediator.Send(new AddBookingCommand
        {
            ProductId = bookingDto.ProductId,
            ClientId = bookingDto.ClientId,
            StartDate = bookingDto.StartDate,
            EndDate = bookingDto.EndDate,
        });

        return StatusCode(200, result);
    }
    [HttpGet("get-bookings")]
    public async Task<IActionResult> GetBookingsAsync()
    {
        var result = await mediator.Send(new GetBookingsQuery());

        return StatusCode(200, result);
    }
}
