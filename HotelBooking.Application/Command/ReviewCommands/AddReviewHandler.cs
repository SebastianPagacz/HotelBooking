using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBooking.Domain.DTOs;
using HotelBooking.Domain.Models;
using HotelBooking.Domain.Repositories;
using MediatR;

namespace HotelBooking.Application.Command;

public class AddReviewHandler(IRepository repository) : IRequestHandler<AddReviewCommand, ReviewDTO>
{
    public async Task<ReviewDTO> Handle(AddReviewCommand request, CancellationToken cancellationToken)
    {
        var review = new Review
        {
            Title = request.Titile,
            Description = request.Description,
            Rating = request.Rating,
            CreatedAt = DateTime.Now,
            IsDeleted = false,
            ProductId = request.ProductId,
        };

        await repository.AddReviewAsync(review);

        var result = new ReviewDTO
        {
            Title = request.Titile,
            Description = request.Description,
            Rating = request.Rating,
        };

        return result;
    }
}
