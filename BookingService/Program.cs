using Booking.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BookingService;
public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddDbContext<DataContext>(options =>
            options.UseInMemoryDatabase("TestBookingDb"));
        builder.Services.AddScoped<IRepository, Repository>();

        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Booking.Application.AssemblyReference).Assembly));

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
