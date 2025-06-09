using Booking.Application.Command;
using Booking.Domain.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookingController(IMediator mediator) : ControllerBase
{
    [HttpPost("AddBooking")]
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
}
