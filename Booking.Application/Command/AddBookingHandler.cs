using Booking.Domain.DTOs;
using Booking.Domain.Models;
using Booking.Domain.Repositories;
using Booking.Domain.Enums;
using MediatR;
using Booking.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Booking.Application.Services;

namespace Booking.Application.Command;

public class AddBookingHandler(IRepository repository, IEmailService emailService) : IRequestHandler<AddBookingCommand, BookingDTO>
{
    public async Task<BookingDTO> Handle(AddBookingCommand request, CancellationToken cancellationToken)
    {
        // Date validation
        if (request.StartDate > request.EndDate)
            throw new DateInvalidException("Start date cannot be greater than end date.");

        if (request.StartDate < DateOnly.FromDateTime(DateTime.Now))
            throw new DateInvalidException("Start date cannot be from the past.");

        if (request.EndDate < DateOnly.FromDateTime(DateTime.Now))
            throw new DateInvalidException("End date cannot be from the past.");

        var datePeriod = (request.EndDate.ToDateTime(TimeOnly.MinValue) - request.StartDate.ToDateTime(TimeOnly.MinValue)).Days;
        var calcPrice = request.PricePerNight * datePeriod;

        var newBooking = new BookingModel
        {
            ProductId = request.ProductId,
            ClientId = request.ClientId,
            ClientEmail = request.ClientEmail,
            //PaymentId = request.PaymentId,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            CaluclatedPrice = calcPrice,
            CreatedAt = DateTime.UtcNow,
            Status = BookingStatusEnum.Pending,
        };

        await repository.AddAsync(newBooking);

        var bookingDTO = new BookingDTO
        {
            Id = newBooking.Id,
            ProductId = newBooking.ProductId,
            StartDate = newBooking.StartDate,
            EndDate = newBooking.EndDate,
            CaluclatedPrice = newBooking.CaluclatedPrice,
        };

        string mailMessage = $"You have a new booking with Id {newBooking.Id} that starts at {newBooking.StartDate}, ends at {newBooking.EndDate} and costs {newBooking.CaluclatedPrice}";
        emailService.Send(newBooking.ClientEmail, "s.pagacz123@gmail.com", "new order", mailMessage);

        return bookingDTO;
    }
}
