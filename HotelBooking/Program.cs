using Microsoft.EntityFrameworkCore;
using HotelBooking.Domain.Repositories;
using System.Threading.Tasks;
using HotelBooking.Domain.Seeders;

namespace HotelBooking;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddDbContext<DataContext>(options => options.UseInMemoryDatabase("TestDb"));
        builder.Services.AddScoped<IRepository, Repository>();
        builder.Services.AddScoped<IHotelBookingSeeder, HotelBookingSeeder>();

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

        var scope = app.Services.CreateScope();
        var seeder = scope.ServiceProvider.GetRequiredService<IHotelBookingSeeder>();
        await seeder.SeedAsync();

        app.Run();
    }
}
