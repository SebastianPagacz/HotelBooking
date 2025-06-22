using HotelBooking.Application.Command;
using HotelBooking.Application.Command.ReviewCommands;
using HotelBooking.Application.Services;
using HotelBooking.Domain.Exceptions.ReviewExceptions;
using HotelBooking.Domain.Models;
using HotelBooking.Domain.Repositories;
using Moq;

namespace HotelBooking.Tests.UnitTests;

public class ReviewHandlerTests
{
    [Fact]
    public async Task AddReviewHandler_AddsReviewAndCachesIt()
    {
        var mockRepo = new Mock<IRepository>();
        var mockCache = new Mock<IRedisCacheService>();
        var handler = new AddReviewHandler(mockRepo.Object, mockCache.Object);
        var command = new AddReviewCommand
        {
            Title = "title",
            Description = "desc",
            Rating = 4,
            ProductId = 1,
        };

        await handler.Handle(command, CancellationToken.None);

        mockRepo.Verify(r => r.AddReviewAsync(It.Is<Review>(rev =>
            rev.Title == command.Title && rev.Description == command.Description && rev.Rating == command.Rating && rev.ProductId == command.ProductId)), Times.Once);
        mockCache.Verify(c => c.SetAsync(It.Is<string>(k => k.StartsWith("review:")), It.IsAny<Review>(), It.IsAny<TimeSpan>()), Times.Once);
    }

    [Fact]
    public async Task UpdateReviewHandler_ExistingReview_UpdatesRepoAndCache()
    {
        var existingReview = new Review { Id = 1, Title = "old", Description = "desc", Rating = 2, ProductId = 1 };
        var mockRepo = new Mock<IRepository>();
        mockRepo.Setup(r => r.GetReviewByIdAsync(existingReview.Id)).ReturnsAsync(existingReview);
        var mockCache = new Mock<IRedisCacheService>();
        var handler = new UpdateReviewHandler(mockRepo.Object, mockCache.Object);
        var command = new UpdateReviewCommand { Id = 1, Title = "new", Description = "newdesc", Rating = 5 };

        await handler.Handle(command, CancellationToken.None);

        mockRepo.Verify(r => r.UpdateReviewAsync(It.Is<Review>(rev =>
            rev.Id == existingReview.Id && rev.Title == command.Title && rev.Description == command.Description && rev.Rating == command.Rating)), Times.Once);
        mockCache.Verify(c => c.SetAsync(It.Is<string>(k => k == $"review:{command.Id}"), existingReview, It.IsAny<TimeSpan>()), Times.Once);
    }

    [Fact]
    public async Task UpdateReviewHandler_ReviewNotFound_ThrowsException()
    {
        var mockRepo = new Mock<IRepository>();
        mockRepo.Setup(r => r.GetReviewByIdAsync(It.IsAny<int>())).ReturnsAsync((Review?)null);
        var mockCache = new Mock<IRedisCacheService>();
        var handler = new UpdateReviewHandler(mockRepo.Object, mockCache.Object);
        var command = new UpdateReviewCommand { Id = 1, Title = "t", Description = "d", Rating = 1 };

        await Assert.ThrowsAsync<ReviewNotFoundException>(() => handler.Handle(command, CancellationToken.None));
        mockRepo.Verify(r => r.UpdateReviewAsync(It.IsAny<Review>()), Times.Never);
        mockCache.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<Review>(), It.IsAny<TimeSpan>()), Times.Never);
    }

    [Fact]
    public async Task DeleteReviewHandler_ExistingReview_UpdatesRepoAndRemovesCache()
    {
        var review = new Review { Id = 1, Title = "t", Description = "d", Rating = 3, ProductId = 1 };
        var mockRepo = new Mock<IRepository>();
        mockRepo.Setup(r => r.GetReviewByIdAsync(review.Id)).ReturnsAsync(review);
        var mockCache = new Mock<IRedisCacheService>();
        var handler = new DeleteReviewHandler(mockRepo.Object, mockCache.Object);
        var command = new DeleteReviewCommand { Id = 1 };

        var result = await handler.Handle(command, CancellationToken.None);

        mockRepo.Verify(r => r.UpdateReviewAsync(review), Times.Once);
        mockCache.Verify(c => c.RemoveAsync($"review:{command.Id}"), Times.Once);
        Assert.Equal(command.Id, result);
    }

    [Fact]
    public async Task DeleteReviewHandler_ReviewNotFound_ThrowsException()
    {
        var mockRepo = new Mock<IRepository>();
        mockRepo.Setup(r => r.GetReviewByIdAsync(It.IsAny<int>())).ReturnsAsync((Review?)null);
        var mockCache = new Mock<IRedisCacheService>();
        var handler = new DeleteReviewHandler(mockRepo.Object, mockCache.Object);
        var command = new DeleteReviewCommand { Id = 1 };

        await Assert.ThrowsAsync<ReviewNotFoundException>(() => handler.Handle(command, CancellationToken.None));
        mockRepo.Verify(r => r.UpdateReviewAsync(It.IsAny<Review>()), Times.Never);
        mockCache.Verify(c => c.RemoveAsync(It.IsAny<string>()), Times.Never);
    }
}