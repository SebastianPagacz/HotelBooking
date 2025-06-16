using HotelBooking.Application.Services;
using HotelBooking.Domain.DTOs;
using HotelBooking.Domain.Exceptions.ProductExceptions;
using HotelBooking.Domain.Models;
using HotelBooking.Domain.Repositories;
using MediatR;

namespace HotelBooking.Application.Command;

public class AddProductHandler(IRepository repository, IRedisCacheService cache) : IRequestHandler<AddProductCommand, ProductDTO>
{
    public async Task<ProductDTO> Handle(AddProductCommand request, CancellationToken cancellationToken)
    {
        var existingProduct = await repository.GetProductByNameAsync(request.Name);
        if (existingProduct != null)
            throw new ProductAlreadyExistsException();

        var newProduct = new Product
        {
            Name = request.Name,
            NumberOfPeople = request.NumberOfPeople,
            NumberOfRooms = request.NumberOfRooms,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsDeleted = request.IsDeleted,
            Price = request.Price,
        };

        await repository.AddProductAsync(newProduct);

        var cacheKey = $"product:{newProduct.Id}";
        await cache.SetAsync(cacheKey, newProduct, TimeSpan.FromMinutes(10)); 

        // TODO: add automapper
        var newProductDto = new ProductDTO 
        { 
            Name = request.Name, 
            NumberOfPeople = request.NumberOfPeople, 
            NumberOfRooms = request.NumberOfRooms,
            Price = request.Price,
        };

        return newProductDto;
    }
}
