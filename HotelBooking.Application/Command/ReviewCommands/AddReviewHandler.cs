using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBooking.Application.Services;
using HotelBooking.Domain.DTOs;
using HotelBooking.Domain.Models;
using HotelBooking.Domain.Repositories;
using MediatR;

namespace HotelBooking.Application.Command;

public class AddReviewHandler(IRepository repository, IRedisCacheService cache) : IRequestHandler<AddReviewCommand, ReviewDTO>
{
    public async Task<ReviewDTO> Handle(AddReviewCommand request, CancellationToken cancellationToken)
    {
        //TODO: validation of reviews
        var review = new Review
        {
            Title = request.Title,
            Description = request.Description,
            Rating = request.Rating,
            CreatedAt = DateTime.Now,
            IsDeleted = false,
            ProductId = request.ProductId,
        };

        var cacheKey = $"review:{review.Id}";
        await cache.SetAsync(cacheKey, review, TimeSpan.FromMinutes(5));

        await repository.AddReviewAsync(review);

        var result = new ReviewDTO
        {
            Title = request.Title,
            Description = request.Description,
            Rating = request.Rating,
        };

        return result;
    }
}
