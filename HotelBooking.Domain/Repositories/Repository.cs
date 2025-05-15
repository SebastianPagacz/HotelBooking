using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBooking.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Domain.Repositories;

public class Repository(DataContext context) : IRepository
{
    public async Task<Product> AddProductAsync(Product product)
    {
        await context.Products
            .AddAsync(product);
        
        await context.SaveChangesAsync();

        return product;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await context.Products
            .Where(p => p.IsDeleted == false)
            .ToListAsync();
    }

    public async Task<Product> GetProductByIdAsync(int id)
    {
        return await context.Products.FirstOrDefaultAsync(p => p.Id == id) ?? throw new NotImplementedException();
    }

    public async Task<Product> UpdateProductAsync(Product product)
    {

        var exisitingProduct = context.Products.FirstOrDefault(p => p.Id == product.Id) ?? throw new NotImplementedException();

        exisitingProduct.Name = product.Name;
        exisitingProduct.NumberOfPeople = product.NumberOfPeople;
        exisitingProduct.NumberOfRooms = product.NumberOfRooms;
        exisitingProduct.UpdatedAt = DateTime.Now;

        context.Products.Update(exisitingProduct);
        await context.SaveChangesAsync();
        return exisitingProduct;
    }
}
