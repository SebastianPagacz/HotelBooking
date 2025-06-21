using HotelBooking.Domain.Models.EventModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Application.Query.BookingQueries;

public class BookingQuery : IRequest<BookingCreatedEvent>
{
    public int Id { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
}
