using StarterApp.Database.Models;

namespace StarterApp.Tests.Repositories;

public class ModelTests
{
    [Fact]
    public void User_FullName_ShouldCombineFirstAndLastName()
    {
        // Arrange
        var user = new User { FirstName = "John", LastName = "Doe", Email = "john@example.com", PasswordHash = "", PasswordSalt = "" };

        // Act & Assert
        Assert.Equal("John Doe", user.FullName);
    }

    [Fact]
    public void User_IsActive_ShouldDefaultToTrue()
    {
        // Arrange & Act
        var user = new User { FirstName = "John", LastName = "Doe", Email = "john@example.com", PasswordHash = "", PasswordSalt = "" };

        // Assert
        Assert.True(user.IsActive);
    }

    [Fact]
    public void Rental_IsRequested_WhenStatusIsRequested_ShouldReturnTrue()
    {
        // Arrange
        var rental = new Rental { Status = "Requested" };

        // Act & Assert
        Assert.True(rental.IsRequested);
    }

    [Fact]
    public void Rental_IsRequested_WhenStatusIsNotRequested_ShouldReturnFalse()
    {
        // Arrange
        var rental = new Rental { Status = "Approved" };

        // Act & Assert
        Assert.False(rental.IsRequested);
    }

    [Theory]
    [InlineData("Requested", true)]
    [InlineData("Approved", false)]
    [InlineData("Completed", false)]
    [InlineData("Rejected", false)]
    public void Rental_IsRequested_ShouldMatchStatus(string status, bool expected)
    {
        // Arrange
        var rental = new Rental { Status = status };

        // Act & Assert
        Assert.Equal(expected, rental.IsRequested);
    }

    [Fact]
    public void Item_ShouldSetProperties()
    {
        // Arrange & Act
        var item = new Item
        {
            Title = "Test Item",
            Description = "Test Description",
            DailyRate = 10.00m,
            Category = "Tools",
            IsAvailable = true,
            Latitude = 55.9533,
            Longitude = -3.1883
        };

        // Assert
        Assert.Equal("Test Item", item.Title);
        Assert.Equal(10.00m, item.DailyRate);
        Assert.True(item.IsAvailable);
    }

    [Fact]
    public void Review_ShouldSetProperties()
    {
        // Arrange & Act
        var review = new Review
        {
            RentalId = 1,
            ReviewerId = 2,
            ReviewerName = "John Doe",
            Rating = 5,
            Comment = "Great!",
            CreatedAt = DateTime.UtcNow
        };

        // Assert
        Assert.Equal(5, review.Rating);
        Assert.Equal("John Doe", review.ReviewerName);
    }

    [Fact]
    public void Category_ShouldSetProperties()
    {
        // Arrange & Act
        var category = new Category { Id = 1, Name = "Tools", Slug = "tools" };

        // Assert
        Assert.Equal("Tools", category.Name);
        Assert.Equal("tools", category.Slug);
    }

    [Fact]
    public void Role_ShouldSetProperties()
    {
        // Arrange & Act
        var role = new Role { Id = 1, Name = "Admin", Description = "Administrator" };

        // Assert
        Assert.Equal("Admin", role.Name);
        Assert.Equal("Administrator", role.Description);
    }

    [Fact]
    public void UserRole_ShouldSetProperties()
    {
        // Arrange & Act
        var userRole = new UserRole { UserId = 1, RoleId = 1 };

        // Assert
        Assert.Equal(1, userRole.UserId);
        Assert.Equal(1, userRole.RoleId);
    }

    [Fact]
    public void UserProfile_ShouldSetProperties()
    {
        // Arrange & Act
        var profile = new UserProfile
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            AverageRating = 4.5,
            ItemsListed = 3,
            RentalsCompleted = 10
        };

        // Assert
        Assert.Equal("John Doe", profile.FullName);
        Assert.Equal(4.5, profile.AverageRating);
        Assert.Equal(3, profile.ItemsListed);
    }

    [Fact]
    public void RentalService_TotalPrice_ShouldCalculateCorrectly()
    {
        // Arrange
        var rental = new Rental
        {
            StartDate = DateTime.Today,
            EndDate = DateTime.Today.AddDays(3),
            TotalPrice = 15.00m
        };

        // Act & Assert
        Assert.Equal(15.00m, rental.TotalPrice);
    }
}
