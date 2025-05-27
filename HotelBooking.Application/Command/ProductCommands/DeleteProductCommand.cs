using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace HotelBooking.Application.Command;

public record DeleteProductCommand : IRequest<int>
{
    public int Id { get; set; }
}
