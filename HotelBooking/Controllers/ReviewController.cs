using HotelBooking.Application.Query;
using HotelBooking.Domain.DTOs;
using HotelBooking.Domain.Models;
using HotelBooking.Domain.Repositories;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReviewController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReviewDTO>>> GetAsync(int productId)
    {
        var reviewsDTO = await mediator.Send(new GetAllReviewsQuery { Id = productId });

        return StatusCode(200, reviewsDTO);
    }
    //[HttpGet("{id}")]
    //public async Task<Review> GetByIdAsync(int id)
    //{
    //    return await repository.GetReviewByIdAsync(id);
    //}
}
