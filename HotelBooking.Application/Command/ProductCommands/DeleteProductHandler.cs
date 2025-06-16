using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBooking.Application.Services;
using HotelBooking.Domain.Exceptions.ProductExceptions;
using HotelBooking.Domain.Repositories;
using MediatR;

namespace HotelBooking.Application.Command;

public class DeleteProductHandler(IRepository repository, IRedisCacheService cache) : IRequestHandler<DeleteProductCommand, int>
{
    public async Task<int> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await repository.GetProductByIdAsync(request.Id) ?? throw new ProductNotFoundException();

        product.IsDeleted = true;

        await repository.UpdateProductAsync(product);

        var cacheKey = $"product:{request.Id}";
        await cache.RemoveAsync(cacheKey);

        return product.Id;
    }
}
