using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBooking.Domain.Exceptions.ProductExceptions;
using HotelBooking.Domain.Repositories;
using MediatR;

namespace HotelBooking.Application.Command;

public class DeleteProductHandler(IRepository repository) : IRequestHandler<DeleteProductCommand, int>
{
    public async Task<int> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var exsitingProduct = await repository.GetProductByIdAsync(request.Id);

        if (exsitingProduct is null)
            throw new ProductNotFoundException();

        exsitingProduct.IsDeleted = true;

        await repository.UpdateProductAsync(exsitingProduct);

        return exsitingProduct.Id;
    }
}
