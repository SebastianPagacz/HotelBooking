using HotelBooking.Domain.Models;
using HotelBooking.Domain.Repositories;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReviewController(IRepository repository) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<Review>> GetAsync(int produtcId)
    {
        return await repository.GetAllReviewsForProductAsync(produtcId);
    }
    [HttpGet("{id}")]
    public async Task<Review> GetByIdAsync(int id)
    {
        return await repository.GetReviewByIdAsync(id);
    }
}
