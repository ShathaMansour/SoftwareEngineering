using Microsoft.EntityFrameworkCore;
using StarterApp.Database.Models;

namespace StarterApp.Tests.Fixtures;

public class DatabaseFixture : IDisposable
{
    public AppDbContext Context { get; private set; }

    public DatabaseFixture()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase($"TestDatabase_{Guid.NewGuid()}")
            .Options;

        Context = new AppDbContext(options);
        Context.Database.EnsureCreated();
        SeedTestData();
    }

    private void SeedTestData()
    {
        var categories = new List<Category>
        {
            new Category { Id = 1, Name = "Tools", Slug = "tools" },
            new Category { Id = 2, Name = "Camping", Slug = "camping" }
        };
        Context.Categories.AddRange(categories);

        var users = new List<User>
        {
            new User { Id = 1, Email = "owner@example.com", FirstName = "Test", LastName = "Owner" },
            new User { Id = 2, Email = "borrower@example.com", FirstName = "Test", LastName = "Borrower" }
        };
        Context.Users.AddRange(users);

        var items = new List<Item>
        {
            new Item { Id = 1, Title = "Electric Drill", Description = "Cordless drill", DailyRate = 5.00m, Category = "Tools" },
            new Item { Id = 2, Title = "Camping Tent", Description = "4 person tent", DailyRate = 15.00m, Category = "Camping" }
        };
        Context.Items.AddRange(items);

        var rentals = new List<Rental>
        {
            new Rental { Id = 1, ItemId = 1, ItemTitle = "Electric Drill", BorrowerId = 2, OwnerId = 1, StartDate = DateTime.Today, EndDate = DateTime.Today.AddDays(3), Status = "Requested", TotalPrice = 15.00m, RequestedAt = DateTime.UtcNow },
            new Rental { Id = 2, ItemId = 2, ItemTitle = "Camping Tent", BorrowerId = 2, OwnerId = 1, StartDate = DateTime.Today.AddDays(-5), EndDate = DateTime.Today.AddDays(-2), Status = "Completed", TotalPrice = 45.00m, RequestedAt = DateTime.UtcNow }
        };
        Context.Rentals.AddRange(rentals);

        Context.SaveChanges();
    }

    public void Dispose()
    {
        Context.Dispose();
    }
}