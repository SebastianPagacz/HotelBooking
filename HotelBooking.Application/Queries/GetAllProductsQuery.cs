using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using HotelBooking.Domain.Models;

namespace HotelBooking.Application.Queries;

public record GetAllProductsQuery() : IRequest<IEnumerable<Product>>;