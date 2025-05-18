using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using HotelBooking.Domain.Models;
using HotelBooking.Domain.DTOs;

namespace HotelBooking.Application.Query;

public record GetAllProductsQuery() : IRequest<IEnumerable<ProductDTO>>;