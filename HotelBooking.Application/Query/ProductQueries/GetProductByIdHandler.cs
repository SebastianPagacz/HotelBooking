using HotelBooking.Application.Services;
using HotelBooking.Domain.DTOs;
using HotelBooking.Domain.Exceptions.ProductExceptions;
using HotelBooking.Domain.Models;
using HotelBooking.Domain.Repositories;
using MediatR;

namespace HotelBooking.Application.Query;

public class GetProductByIdHandler(IRepository repository, IRedisCacheService cache) : IRequestHandler<GetProductByIdQuery, ProductDTO>
{
    public async Task<ProductDTO> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"product:{request.Id}";
        var cachedProduct = await cache.GetAsync<Product>(cacheKey);

        if (cachedProduct != null)
        {
            // TODO: add automapper
            var cachedDto = new ProductDTO
            {
                Name = cachedProduct.Name,
                NumberOfPeople = cachedProduct.NumberOfPeople,
                NumberOfRooms = cachedProduct.NumberOfRooms,
                Price = cachedProduct.Price,
            };
            return cachedDto;
        }

        var product = await repository.GetProductByIdAsync(request.Id);
        
        if (product is null || product.IsDeleted)
            throw new ProductNotFoundException();

        await cache.SetAsync(cacheKey, product, TimeSpan.FromMinutes(10));

        var productDTO = new ProductDTO
        {
            Name = product.Name,
            NumberOfPeople = product.NumberOfPeople,
            NumberOfRooms = product.NumberOfRooms,
            Price = product.Price,
        };

        return productDTO;
    }
}
