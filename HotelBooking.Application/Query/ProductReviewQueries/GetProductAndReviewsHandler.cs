using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBooking.Domain.DTOs;
using HotelBooking.Domain.Exceptions.ProductExceptions;
using HotelBooking.Domain.Repositories;
using MediatR;

namespace HotelBooking.Application.Query;

public class GetProductAndReviewsHandler(IRepository repository) : IRequestHandler<GetProductAndReviewsQuery, ProductReviewDTO>
{
    public async Task<ProductReviewDTO> Handle(GetProductAndReviewsQuery request, CancellationToken cancellationToken)
    {
        var product = await repository.GetProductWithReviewsAsync(request.ProductId) ?? throw new ProductNotFoundException();

        // Overall rating calculation
        double overallRating = Math.Round(product.Reviews.Average(r => r.Rating), 2);

        var reviewsMapped = product.Reviews
            .Where(p => !p.IsDeleted)
            .Select(p => new ReviewDTO
            {
                Title = p.Title,
                Description = p.Description,
                Rating = p.Rating,
            });

        var result = new ProductReviewDTO
        {
            Name = product.Name,
            NumberOfRooms = product.NumberOfRooms,
            NumberOfPeople = product.NumberOfPeople,
            Price = product.Price,
            OverallRating = overallRating,
            Reviews = reviewsMapped.ToList()
        };

        return result;
    }
}
