using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBooking.Domain.Exceptions.ProductExceptions;
using HotelBooking.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Domain.Repositories;

public class Repository(DataContext context) : IRepository
{
    public async Task<Product> AddProductAsync(Product product)
    {
        // Checking if product already exisits, if it exists throw exception
        var exisitingProduct = await context.Products.FirstOrDefaultAsync(p => p.Id == product.Id);
        if (exisitingProduct != null)
            throw new ProductAlreadyExistsException();

        await context.Products
            .AddAsync(product);
        
        await context.SaveChangesAsync();

        return product;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        // If there are no products in the db throw exception
        if (!context.Products.Any())
            throw new ProductNotFoundException();

        return await context.Products
            .Where(p => p.IsDeleted == false)
            .ToListAsync();
    }

    public async Task<Product> GetProductByIdAsync(int id)
    {
        // If product does not exist throw exception
        return await context.Products.FirstOrDefaultAsync(p => p.Id == id) ?? throw new ProductNotFoundException();
    }

    public async Task<Product> UpdateProductAsync(Product product)
    {
        // If product does not exist throw exception
        var exisitingProduct = context.Products.FirstOrDefault(p => p.Id == product.Id) ?? throw new ProductNotFoundException();

        exisitingProduct.Name = product.Name;
        exisitingProduct.NumberOfPeople = product.NumberOfPeople;
        exisitingProduct.NumberOfRooms = product.NumberOfRooms;
        exisitingProduct.UpdatedAt = DateTime.Now;

        context.Products.Update(exisitingProduct);
        await context.SaveChangesAsync();
        return exisitingProduct;
    }
}
