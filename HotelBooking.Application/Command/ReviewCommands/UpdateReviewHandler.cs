using HotelBooking.Application.Services;
using HotelBooking.Domain.DTOs;
using HotelBooking.Domain.Exceptions.ReviewExceptions;
using HotelBooking.Domain.Models;
using HotelBooking.Domain.Repositories;
using MediatR;

namespace HotelBooking.Application.Command.ReviewCommands;

public class UpdateReviewHandler(IRepository repository, IRedisCacheService cache) : IRequestHandler<UpdateReviewCommand, ReviewDTO>
{
    public async Task<ReviewDTO> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
    {
        string cacheKey = $"review:{request.Id}";
        await cache.RemoveAsync(cacheKey);

        // add per user validation
        var exisitngReview = await repository.GetReviewByIdAsync(request.Id) ?? throw new ReviewNotFoundException();

        exisitngReview.Title = request.Title;
        exisitngReview.Description = request.Description;
        exisitngReview.Rating = request.Rating;

        await repository.UpdateReviewAsync(exisitngReview);

        await cache.SetAsync<Review>(cacheKey, exisitngReview, TimeSpan.FromMinutes(5));

        var reviewDto = new ReviewDTO
        {
            Title = request.Title,
            Description = request.Description,
            Rating = request.Rating,
        };

        return reviewDto;
    }
}
