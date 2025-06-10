using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBooking.Domain.Models;

namespace HotelBooking.Domain.Repositories;

public interface IRepository
{
    #region Product
    public Task<Product> AddProductAsync(Product product);
    public Task<IEnumerable<Product>> GetAllProductsAsync();
    public Task<Product> GetProductByIdAsync(int id);
    public Task<Product> UpdateProductAsync(Product product);
    public Task<Product> GetProductByNameAsync(string name);
    #endregion

    #region Review
    public Task<Review> AddReviewAsync(Review review);
    public Task<IEnumerable<Review>> GetAllReviewsForProductAsync(int productId);
    public Task<Review> GetReviewByIdAsync(int id);
    public Task<Review> UpdateReviewAsync(Review review);
    #endregion

    #region ProductReview
    public Task<Product> GetProductWithReviewsAsync(int productId);
    #endregion
}
