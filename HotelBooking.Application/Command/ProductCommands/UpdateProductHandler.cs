using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBooking.Domain.DTOs;
using HotelBooking.Domain.Exceptions.ProductExceptions;
using HotelBooking.Domain.Repositories;
using MediatR;

namespace HotelBooking.Application.Command;

public class UpdateProductHandler(IRepository repository) : IRequestHandler<UpdateProductCommand, ProductDTO>
{
    public async Task<ProductDTO> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await repository.GetProductByIdAsync(request.Id);
        
        if (product is null)
            throw new ProductNotFoundException();
        
        product.Name = request.Name;
        product.NumberOfPeople = request.NumberOfPeople;
        product.NumberOfRooms = request.NumberOfRooms;
        product.IsDeleted = request.IsDeleted;
        product.UpdatedAt = DateTime.UtcNow;

        var productDTO = new ProductDTO
        {
            Name = product.Name,
            NumberOfPeople = product.NumberOfPeople,
            NumberOfRooms = product.NumberOfRooms,
            Price = product.Price,
        };

        await repository.UpdateProductAsync(product);

        return productDTO;
    }
}
