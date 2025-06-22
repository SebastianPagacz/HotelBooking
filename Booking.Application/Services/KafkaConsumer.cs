using Booking.Application.Command;
using Booking.Domain.Models;
using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Booking.Application.Services;

public class KafkaConsumer : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public KafkaConsumer(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await ConsumeAsync("booking", stoppingToken);
    }

    public async Task ConsumeAsync(string topic, CancellationToken stoppingToken) 
    {
        var config = new ConsumerConfig
        {
            GroupId = "booking",
            BootstrapServers = "kafka:9092",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        using var consumer = new ConsumerBuilder<Null, string>(config).Build();
        consumer.Subscribe(topic);

        while (!stoppingToken.IsCancellationRequested) 
        {
            try
            {
                var consumerResult = consumer.Consume(stoppingToken);

                using (var scope = _scopeFactory.CreateScope())
                {
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                    var product = JsonConvert.DeserializeObject<BookingEvent>(consumerResult.Message.Value);
                    await mediator.Send(new AddBookingCommand
                    {
                        ProductId = product.Id,
                        ClientId = 1,
                        ClientEmail = product.ClientEmail,
                        PricePerNight = product.PricePerNight,
                        StartDate = product.StartDate,
                        EndDate = product.EndDate,
                    });
                }
            }
            catch(ConsumeException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
