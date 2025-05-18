using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBooking.Domain.DTOs;
using HotelBooking.Domain.Exceptions.ProductExceptions;
using HotelBooking.Domain.Models;
using HotelBooking.Domain.Repositories;
using MediatR;

namespace HotelBooking.Application.Query;

public class GetProductByIdHandler(IRepository repository) : IRequestHandler<GetProductByIdQuery, ProductDTO>
{
    public async Task<ProductDTO> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await repository.GetProductByIdAsync(request.Id);
        
        if (product is null || product.IsDeleted)
            throw new ProductNotFoundException();

        var productDTO = new ProductDTO
        {
            Id = product.Id,
            Name = product.Name,
            NumberOfPeople = product.NumberOfPeople,
            NumberOfRooms = product.NumberOfRooms
        };

        return productDTO;
    }
}
