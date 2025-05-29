using HotelBooking.Application.Query;
using HotelBooking.Domain.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductReviewController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductReviewDTO>> GetById(int id)
    {
        var product = await mediator.Send(new GetProductAndReviewsQuery { ProductId = id });

        return StatusCode(200, product);
    }
}
