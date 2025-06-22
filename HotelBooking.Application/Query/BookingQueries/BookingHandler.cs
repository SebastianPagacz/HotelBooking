using HotelBooking.Application.Producer;
using HotelBooking.Application.Services;
using HotelBooking.Domain.DTOs;
using HotelBooking.Domain.Exceptions.ProductExceptions;
using HotelBooking.Domain.Models;
using HotelBooking.Domain.Models.EventModels;
using HotelBooking.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Application.Query.BookingQueries;

public class BookingHandler(IRepository repository, IRedisCacheService cache, IKafkaProducer kafkaProducer) : IRequestHandler<BookingQuery, BookingCreatedEvent>
{
    public async Task<BookingCreatedEvent> Handle(BookingQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"product:{request.Id}";
        var cachedProduct = await cache.GetAsync<Product>(cacheKey);

        if (cachedProduct != null)
        {
            // TODO: add automapper
            var cachedEvent = new BookingCreatedEvent
            {
                Id = cachedProduct.Id,
                Name = cachedProduct.Name,
                ClientEmail = request.ClientEmail,
                PricePerNight = cachedProduct.Price,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
            };
            // refactor needed
            await kafkaProducer.SendBookingCreatedAsync(cachedEvent);

            return cachedEvent;
        }

        var product = await repository.GetProductByIdAsync(request.Id);
        
        if (product is null || product.IsDeleted)
            throw new ProductNotFoundException();

        var bookingEvent = new BookingCreatedEvent
        {
            Id = product.Id,
            Name = product.Name,
            ClientEmail = request.ClientEmail,
            PricePerNight = product.Price,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
        };

        await kafkaProducer.SendBookingCreatedAsync(bookingEvent);

        return bookingEvent;
    }
}
