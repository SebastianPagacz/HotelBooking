using HotelBooking.Application.Command;
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
    
    [HttpGet("{id}")]
    public async Task<ActionResult<ReviewDTO>> GetByIdAsync(int id)
    {
        var review = await mediator.Send(new GetReviewByIdQuery { Id = id });

        return StatusCode(200, review);
    }

    [HttpPost]
    public async Task<ActionResult<ReviewDTO>> AddAsync([FromBody] CreateReviewDTO review)
    {
        await mediator.Send(new AddReviewCommand 
        { 
            Titile = review.Title,
            Description = review.Description,
            Rating = review.Raiting,
            ProductId = review.ProductId,
        });

        return StatusCode(200, review);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<int>> DeleteAsync(int id)
    {
        await mediator.Send(new DeleteReviewCommand { Id = id });
        
        return StatusCode(200, $"Review with Id {id} has been deleted.");
    }
}
