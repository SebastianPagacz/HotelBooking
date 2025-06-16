using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBooking.Application.Services;
using HotelBooking.Domain.DTOs;
using HotelBooking.Domain.Exceptions.ProductExceptions;
using HotelBooking.Domain.Models;
using HotelBooking.Domain.Repositories;
using MediatR;

namespace HotelBooking.Application.Command;

public class UpdateProductHandler(IRepository repository, IRedisCacheService cache) : IRequestHandler<UpdateProductCommand, ProductDTO>
{
    public async Task<ProductDTO> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await repository.GetProductByIdAsync(request.Id) ?? throw new ProductNotFoundException();

        var cacheKey = $"product:{request.Id}";

        product.Name = request.Name;
        product.NumberOfPeople = request.NumberOfPeople;
        product.NumberOfRooms = request.NumberOfRooms;
        product.IsDeleted = request.IsDeleted;
        product.UpdatedAt = DateTime.UtcNow;

        var productDTO = new ProductDTO
        {
            Name = request.Name,
            NumberOfPeople = request.NumberOfPeople,
            NumberOfRooms = request.NumberOfRooms,
            Price = request.Price,
        };

        await repository.UpdateProductAsync(product);

        await cache.SetAsync(cacheKey, product, TimeSpan.FromMinutes(5));

        return productDTO;
    }
}
