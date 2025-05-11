using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBooking.Domain.Repositories;
using HotelBooking.Domain.Models;

namespace HotelBooking.Domain.Seeders;

public class HotelBookingSeeder(DataContext _context) : IHotelBookingSeeder
{
    public async Task SeedAsync()
    {
        if (!_context.Products.Any())
        {
            var products = new List<Product>
            {
                new Product { Name = "Domek maly", NumberOfPeople = 3, NumberOfRooms = 2 },
                new Product { Name = "Domek sredni", NumberOfPeople = 6, NumberOfRooms = 3 },
                new Product { Name = "Domek duzy", NumberOfPeople = 9, NumberOfRooms = 5 }
            };
            _context.Products.AddRange(products);
            await _context.SaveChangesAsync();
        }
    }
}
