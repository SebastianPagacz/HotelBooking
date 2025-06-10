using HotelBooking.Application.Services;
using HotelBooking.Domain.DTOs;
using HotelBooking.Domain.Exceptions.ReviewExceptions;
using HotelBooking.Domain.Models;
using HotelBooking.Domain.Repositories;
using MediatR;

namespace HotelBooking.Application.Query;

public class GetReviewByIdHandler(IRepository repository, IRedisCacheService cache) : IRequestHandler<GetReviewByIdQuery, ReviewDTO>
{
    public async Task<ReviewDTO> Handle(GetReviewByIdQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"review:{request.Id}";
        var cachedReview = await cache.GetAsync<Review>(cacheKey);

        if (cachedReview != null) 
        {
            //TODO: add automapper
            var cachedDto = new ReviewDTO
            {
                Title = cachedReview.Title,
                Description = cachedReview.Description,
                Rating = cachedReview.Rating,
            };
            return cachedDto;
        }

        var review = await repository.GetReviewByIdAsync(request.Id) ?? throw new ReviewNotFoundException();

        await cache.SetAsync(cacheKey, review, TimeSpan.FromMinutes(5));

        var result = new ReviewDTO
        {
            Title = review.Title,
            Description = review.Description,
            Rating = review.Rating,
        };

        return result;
    }
}
