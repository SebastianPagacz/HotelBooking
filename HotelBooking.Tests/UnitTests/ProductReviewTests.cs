using HotelBooking.Application.Query;
using HotelBooking.Domain.Models;
using HotelBooking.Domain.Repositories;
using Moq;

namespace HotelBooking.Tests.UnitTests;

public class ProductReviewTests
{
    [Fact]
    public async Task Handle_NoReviews_ReturnsZeroRating()
    {
        // arrange
        var mockRepo = new Mock<IRepository>();
        var product = new Product { Reviews = new List<Review>() };
        mockRepo.Setup(r => r.GetProductWithReviewsAsync(It.IsAny<int>())).ReturnsAsync(product);
        var handler = new GetProductAndReviewsHandler(mockRepo.Object);

        var query = new GetProductAndReviewsQuery { ProductId = 1 };

        // act
        var result = await handler.Handle(query, CancellationToken.None);

        // assert
        Assert.Equal(0, result.OverallRating);
    }
}