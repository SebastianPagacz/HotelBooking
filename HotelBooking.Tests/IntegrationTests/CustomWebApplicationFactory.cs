using System.Linq;
using HotelBooking.Domain.Repositories;
using HotelBooking.Domain.Seeders;
using HotelBooking.Application.Services;
using StackExchange.Redis;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HotelBooking.Tests.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<DataContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            var redisDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(IConnectionMultiplexer));
            if (redisDescriptor != null)
            {
                services.Remove(redisDescriptor);
            }

            services.AddSingleton<IRedisCacheService, FakeRedisCacheService>();

            services.AddDbContext<DataContext>(options =>
            {
                options.UseInMemoryDatabase("TestDb");
            });

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<DataContext>();
            db.Database.EnsureCreated();
            var seeder = scopedServices.GetRequiredService<IHotelBookingSeeder>();
            seeder.SeedAsync().GetAwaiter().GetResult();
        });
    }
}