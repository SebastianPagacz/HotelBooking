using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using HotelBooking.Domain.Models;
using HotelBooking.Domain.Repositories;
using HotelBooking.Domain.Exceptions.ProductExceptions;
using HotelBooking.Domain.DTOs;

namespace HotelBooking.Application.Query;

public class GetAllProductsHandler(IRepository repository) : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductDTO>>
{

    public async Task<IEnumerable<ProductDTO>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await repository.GetAllProductsAsync();
        
        if (!products.Any())
            throw new ProductNotFoundException();

        var availableProducts = products
            .Where(p => !p.IsDeleted)
            .Select(p => new ProductDTO
            {
                Name = p.Name,
                NumberOfRooms = p.NumberOfRooms,
                NumberOfPeople = p.NumberOfPeople,
                Price = p.Price,
            });

        return availableProducts;
    }
}
