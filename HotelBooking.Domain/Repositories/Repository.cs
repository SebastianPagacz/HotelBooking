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
    #region Product
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
        return await context.Products
            .ToListAsync();
    }

    public async Task<Product> GetProductByIdAsync(int id)
    {
        // Query handles business logic
        return await context.Products.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Product> UpdateProductAsync(Product product)
    {
        context.Products.Update(product);
        await context.SaveChangesAsync();
        return product;
    }
    #endregion

    #region Review
    public async Task<Review> AddReviewAsync(Review review)
    {
        await context.Reviews
            .AddAsync(review);

        await context.SaveChangesAsync();

        return review;
    }

    public async Task<Review> GetReviewByIdAsync(int id)
    {
        return await context.Reviews.FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<IEnumerable<Review>> GetAllReviewsForProductAsync(int productId)
    {
        return await context.Reviews
            .Where(r => r.ProductId == productId)
            .ToListAsync();
    }

    public async Task<Review> UpdateReviewAsync(Review review)
    {
        context.Reviews.Update(review);
        await context.SaveChangesAsync();

        return review;
    }
    #endregion
}
