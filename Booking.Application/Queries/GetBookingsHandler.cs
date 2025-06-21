using Booking.Domain.Models;
using Booking.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Application.Queries
{
    public class GetBookingsHandler(IRepository repository) : IRequestHandler<GetBookingsQuery, IEnumerable<BookingModel>>
    {
        public async Task<IEnumerable<BookingModel>> Handle(GetBookingsQuery request, CancellationToken cancellationToken)
        {
            return await repository.GetAsync() ?? throw new NotImplementedException();
        }
    }
}
