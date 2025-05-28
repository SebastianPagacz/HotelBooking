using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBooking.Domain.DTOs;
using HotelBooking.Domain.Exceptions.ReviewExceptions;
using HotelBooking.Domain.Repositories;
using MediatR;

namespace HotelBooking.Application.Query;

public class GetReviewByIdHandler(IRepository repository) : IRequestHandler<GetReviewByIdQuery, ReviewDTO>
{
    public async Task<ReviewDTO> Handle(GetReviewByIdQuery request, CancellationToken cancellationToken)
    {
        var review = await repository.GetReviewByIdAsync(request.Id) ?? throw new ReviewNotFoundException();

        var result = new ReviewDTO
        {
            Title = review.Title,
            Description = review.Description,
            Rating = review.Rating,
        };

        return result;
    }
}
