using HotelBooking.Application.Query.BookingQueries;
using HotelBooking.Domain.Models.Requests;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookingController(IMediator mediator) : ControllerBase
{
    [HttpPost("create-booking/{id}")]
    public async Task<IActionResult> CreateBooking(int id, [FromBody] BookingRequest request)
    {
        var result = await mediator.Send(new BookingQuery
        {
            Id = id,
            ClientEmail = request.ClientEmail,
            StartDate = request.StartDate,
            EndDate = request.EndDate
        });

        return StatusCode(200, "Success");
    }
}
