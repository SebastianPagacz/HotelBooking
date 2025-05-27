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

public class AddProductHandler(IRepository repository) : IRequestHandler<AddProductCommand, ProductDTO>
{
    public async Task<ProductDTO> Handle(AddProductCommand request, CancellationToken cancellationToken)
    {
        var numOfProducts = repository.GetAllProductsAsync().Result.Count() + 1;
        
        var newProduct = new Product
        {
            Id = numOfProducts,
            Name = request.Name,
            NumberOfPeople = request.NumberOfPeople,
            NumberOfRooms = request.NumberOfRooms,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsDeleted = request.IsDeleted,
            Price = request.Price,
        };

        await repository.AddProductAsync(newProduct);

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
