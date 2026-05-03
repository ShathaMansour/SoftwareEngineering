using StarterApp.Database.Data;
using StarterApp.Database.Models;
using StarterApp.Tests.Fixtures;

namespace StarterApp.Tests.Repositories;

public class ReviewRepositoryTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;
    private readonly ReviewRepository _repository;

    public ReviewRepositoryTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
        _repository = new ReviewRepository(_fixture.Context);
    }

    [Fact]
    public async Task CreateAsync_ShouldAddReview()
    {
        // Arrange
        var review = new Review
        {
            RentalId = 2,
            ReviewerId = 2,
            ReviewerName = "Test Borrower",
            Rating = 5,
            Comment = "Great item!",
            CreatedAt = DateTime.UtcNow
        };

        // Act
        var created = await _repository.CreateAsync(review);

        // Assert
        Assert.NotNull(created);
        Assert.True(created.Id > 0);
        Assert.Equal(5, created.Rating);
    }

    [Fact]
    public async Task GetByItemIdAsync_ShouldReturnReviews()
    {
        // Arrange & Act
        var reviews = await _repository.GetByItemIdAsync(1);

        // Assert
        Assert.NotNull(reviews);
    }

    [Fact]
    public async Task GetByUserIdAsync_ShouldReturnReviews()
    {
        // Arrange & Act
        var reviews = await _repository.GetByUserIdAsync(1);

        // Assert
        Assert.NotNull(reviews);
    }

    [Fact]
    public async Task ExistsForRentalAsync_WhenNotExists_ShouldReturnFalse()
    {
        // Arrange & Act
        var exists = await _repository.ExistsForRentalAsync(999, 999);

        // Assert
        Assert.False(exists);
    }

    [Fact]
    public async Task ExistsForRentalAsync_WhenExists_ShouldReturnTrue()
    {
        // Arrange
        var review = new Review
        {
            RentalId = 1,
            ReviewerId = 1,
            ReviewerName = "Test",
            Rating = 4,
            CreatedAt = DateTime.UtcNow
        };
        await _repository.CreateAsync(review);

        // Act
        var exists = await _repository.ExistsForRentalAsync(1, 1);

        // Assert
        Assert.True(exists);
    }
}
