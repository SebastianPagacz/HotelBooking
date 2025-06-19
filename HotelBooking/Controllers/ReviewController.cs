using HotelBooking.Application.Command;
using HotelBooking.Application.Command.ReviewCommands;
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
    public async Task<IActionResult> GetReviews(int productId)
    {
        var reviews = await mediator.Send(new GetAllReviewsQuery { Id = productId });

        return StatusCode(200, reviews);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetReview(int id)
    {
        var review = await mediator.Send(new GetReviewByIdQuery { Id = id });

        return StatusCode(200, review);
    }

    [HttpPost]
    public async Task<IActionResult> AddReview([FromBody] CreateReviewDTO review)
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
    public async Task<IActionResult> DeleteReview(int id)
    {
        await mediator.Send(new DeleteReviewCommand { Id = id });
        
        return StatusCode(200, $"Review with Id {id} has been deleted.");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutReview(int id, [FromBody] ReviewDTO request)
    {
        var review = await mediator.Send(new UpdateReviewCommand 
        { 
            Id = id,
            Title = request.Title,
            Description = request.Description,
            Rating = request.Rating
        });

        return StatusCode(200, review);
    }
}
