using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBooking.Domain.Repositories;
using HotelBooking.Domain.Models;
using System.Reflection;

namespace HotelBooking.Domain.Seeders;

public class HotelBookingSeeder(DataContext _context) : IHotelBookingSeeder
{
    public async Task SeedAsync()
    {
        if (!_context.Products.Any())
        {
            var products = new List<Product>
            {
                new Product { Name = "Domek maly", NumberOfPeople = 3, NumberOfRooms = 2, Price = 100  },
                new Product { Name = "Domek sredni", NumberOfPeople = 6, NumberOfRooms = 3, IsDeleted = true , Price = 200 },
                new Product { Name = "Domek duzy", NumberOfPeople = 9, NumberOfRooms = 5 , Price = 150 },
                new Product { Name = "Domek u Basisty", NumberOfPeople = 8, NumberOfRooms = 2 , Price = 500 }
            };
            _context.Products.AddRange(products);
            await _context.SaveChangesAsync();
        }

        if (!_context.Reviews.Any())
        {
            var reviews = new List<Review>
            {
                new Review { Title = "Super domek", Description = "Test Test Test", Rating = 5, ProductId = 4 },
                new Review { Title = "Slaby domek", Description = "Test Test Test", Rating = 1, ProductId = 1 },
                new Review { Title = "Basista rzadi", Description = "Test Test Test" , Rating = 5, ProductId = 4 },
                new Review { Title = "Maly domek", Description = "Test", Rating = 3, ProductId = 1 },
                new Review { Title = "Duzy dom", Description = "Test", Rating = 4, ProductId = 3 },
            };
            _context.Reviews.AddRange(reviews);
            await _context.SaveChangesAsync();
        }

    }
}
