using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBooking.Domain.Exceptions.ReviewExceptions;
using HotelBooking.Domain.Repositories;
using MediatR;

namespace HotelBooking.Application.Command.ReviewCommands;

public class DeleteReviewHandler(IRepository repository) : IRequestHandler<DeleteReviewCommand, int>
{
    public async Task<int> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
    {
        var review = await repository.GetReviewByIdAsync(request.Id) ?? throw new ReviewNotFoundException();

        review.IsDeleted = true;

        await repository.UpdateReviewAsync(review);
        
        return review.Id;
    }
}
