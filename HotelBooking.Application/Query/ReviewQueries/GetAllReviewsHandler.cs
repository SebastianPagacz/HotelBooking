using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBooking.Domain.DTOs;
using HotelBooking.Domain.Repositories;
using MediatR;

namespace HotelBooking.Application.Query;

public class GetAllReviewsHandler(IRepository repository) : IRequestHandler<GetAllReviewsQuery, IEnumerable<ReviewDTO>>
{
    public async Task<IEnumerable<ReviewDTO>> Handle(GetAllReviewsQuery request, CancellationToken cancellationToken)
    {
        // if no reviews for the product an empty list will be returned
        var reviews = await repository.GetAllReviewsForProductAsync(request.Id);

        var reviewsDto = reviews
            .Where(r => !r.IsDeleted)
            .Select(r => new ReviewDTO
            {
                Title = r.Title,
                Description = r.Description,
                Raiting = r.Rating,
            });

        return reviewsDto;
    }
}
