using Confluent.Kafka;
using HotelBooking.Domain.Models.EventModels;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace HotelBooking.Application.Producer;

public class KafkaProducer : IKafkaProducer
{
    private readonly IProducer<Null, string> _producer;

    public KafkaProducer(IConfiguration configuration)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = configuration["Kafka:BootstrapServers"],
            AllowAutoCreateTopics = true
        };
        _producer = new ProducerBuilder<Null, string>(config).Build();
    }

    public async Task SendBookingCreatedAsync(BookingCreatedEvent message)
    {
        var jsonString = JsonSerializer.Serialize(message);

        await _producer.ProduceAsync("booking", new Message<Null, string>
        {
            Value = jsonString
        });
    }
}
