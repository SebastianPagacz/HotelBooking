using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using HotelBooking.Domain.Models;
using HotelBooking.Domain.Repositories;

namespace HotelBooking.Application.Queries;

public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<Product>>
{
    private readonly IRepository _repository;
    public GetAllProductsHandler(IRepository repository)
    {
        _repository = repository;
    }
    public async Task<IEnumerable<Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetAllProductsAsync();
    }
}
